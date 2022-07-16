using Microsoft.Data.Sqlite;
public class Database
{
    public static void CreateIfNotExist()
    {
        Directory.CreateDirectory("C:\\papka");
        var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate");
        connection.Open();
        SqliteCommand command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = "CREATE TABLE IF NOT EXISTS Triggers(time VARCHAR(20))";
        command.ExecuteNonQuery();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name VARCHAR(50) NOT NULL, email VARCHAR(50) NOT NULL UNIQUE, password VARCHAR(100) NOT NULL, isAdmin BOOL)";
        command.ExecuteNonQuery();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Tasks(id INTEGER REFERENCES Users (id),name VARCHAR (50)  NOT NULL,title VARCHAR(200) NOT NULL,api INTEGER NOT NULL,api_option INTEGER NOT NULL, last_trigger VARCHAR(20)  DEFAULT 'xxx',start VARCHAR(20),end VARCHAR(20),cron_period VARCHAR(20),taskId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE)";
        command.ExecuteNonQuery();
    }
    public static List<EmailTask> Update()
    {
        using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
        {
            string sqlExpression = "SELECT email, api, api_option, Tasks.name, title, start, cron_period, taskId FROM Users INNER JOIN Tasks ON Tasks.id = Users.id";
            connection.Open();
            List<EmailTask> tasks = new List<EmailTask>();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmailTask task = new EmailTask();
                        task.Email = reader.GetString(0);
                        task.Api = Convert.ToInt32(reader.GetValue(1));
                        task.ApiOption = Convert.ToInt32(reader.GetValue(2));
                        task.Name = reader.GetString(3);
                        task.Title = reader.GetString(4);
                        task.Start1 = reader.GetString(5);
                        task.CronPeriod = reader.GetString(6);
                        task.TaskId = Convert.ToInt32(reader.GetValue(7));
                        tasks.Add(task);
                    }
                }
                return tasks;
            }
        }
    } 

    public static void Trigger(EmailTask task)
    {
        using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"SELECT EXISTS(SELECT * FROM Tasks WHERE taskId = '{task.TaskId}')";
            command.ExecuteNonQuery();
            bool u = false;

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                u = reader.GetBoolean(0);   
            }
            if (u)
            {
                command.CommandText = $"UPDATE Tasks SET last_trigger = '{DateTime.Now}' WHERE taskId = '{task.TaskId}'";
                command.ExecuteNonQuery();
            }
            
            command.CommandText = $"INSERT INTO Triggers(time) VALUES('{DateTime.Now}')";
            command.ExecuteNonQuery();
        }
    }
}
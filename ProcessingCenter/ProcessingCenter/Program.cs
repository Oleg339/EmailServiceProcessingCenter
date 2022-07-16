public class ProcessingCenter
{
    public static List<string> toDeleteFiles = new List<string>();
    public static void Main()
    {
        int countOfThreads = 4;

        Database.CreateIfNotExist();
        DirectoryInfo dirInfo = new DirectoryInfo("C:\\papka\\1");
        Directory.CreateDirectory(dirInfo.FullName);
        foreach (FileInfo file in dirInfo.GetFiles())
        {
            file.Delete();
        }


        List<EmailTask> emailTasks = new List<EmailTask>();
        TimeSpan Duracity = DateTime.Parse(DateTime.Now.AddMinutes(1).ToString()[..^3]) - DateTime.Now;

        if (Duracity < TimeSpan.FromMinutes(1))
        {
            Thread.Sleep(Duracity);
        }
        while (true)
        {
            emailTasks = Database.Update();

            int t = emailTasks.Count / countOfThreads;

            List<EmailTask>[] emailTasks0 = new List<EmailTask>[countOfThreads];
            for (int i = 0; i < countOfThreads; i++)
            {
                emailTasks0[i] = new List<EmailTask>();
            }

            int j1 = 0, j, w;
            for (int i = 0; i < countOfThreads; i++)
            {
                w = t * (i == 0 ? 1 : i + 1);
                if (i == countOfThreads - 1)
                {
                    w = w + (emailTasks.Count % countOfThreads);
                }

                for (j = j1; j < w; j++)
                {
                    emailTasks0[i].Add(emailTasks[j]);
                }
                j1 = j;
            }

            List<ParameterizedThreadStart> TreadStarts = new List<ParameterizedThreadStart>();
            List<Thread> Treads = new List<Thread>();
            for (int i = 0; i < emailTasks0.Length; i++)
            {
                TreadStarts.Add(new ParameterizedThreadStart(process));
                Treads.Add(new Thread(TreadStarts[i]));
                Treads[i].Start(emailTasks0[i]);
            }
            Duracity = DateTime.Parse(DateTime.Now.AddMinutes(1).ToString()[..^3]) - DateTime.Now;
            Console.WriteLine(Duracity + " left until the next update");
            Thread.Sleep(Duracity);
            for(int i = 0; i < emailTasks0.Length; i++)
            {
                Treads[i].Interrupt();
            }
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    Console.WriteLine("");
                }
            }
        }
    }

    public static void process(object tasks0)
    {
        List<EmailTask> tasks = tasks0 as List<EmailTask>;

        foreach (var task in tasks)
        {
            var dates = task.datetimes();
            foreach (var date in dates)
            {
                if (DateTime.Now.ToString()[..^3] == date.ToString()[..^3])
                {
                    task.CurrentThread = Thread.CurrentThread.GetHashCode();
                    toDeleteFiles.Add((Thread.CurrentThread.GetHashCode().ToString() + task.TaskId.ToString()));
                    EmailSender.SendEmail(task);
                    Database.Trigger(task);
                }
            }
        }
    }
}
    



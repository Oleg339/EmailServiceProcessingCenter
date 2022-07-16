using System.Net;
using System.Net.Mail;
static class EmailSender
{
    public static void SendEmail(object task1)
    {
        EmailTask task = task1 as EmailTask;
        if (task.Api == 1)
        {
            CsvConverter.WriteInCsv(WeatherApi.Weather(task.ApiOption), task.CurrentThread, task.TaskId);
        }
        else if (task.Api == 2)
        {
            CsvConverter.WriteInCsv(CryptoApi.Crypto(task.ApiOption), task.CurrentThread, task.TaskId);
        }
        else
        {
            CsvConverter.WriteInCsv(GreatQuotesApi.Quotes(), task.CurrentThread, task.TaskId);
        }

        MailAddress from = new MailAddress("baharoleg224@mail.ru", "Oleg");
        MailAddress to = new MailAddress(task.Email);
        MailMessage m = new MailMessage(from, to);
        m.Attachments.Add(new Attachment($"C:\\papka\\1\\{task.CurrentThread}{task.TaskId}.csv"));
        m.Subject = task.Name;
        m.Body = task.Title;
        SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential("baharoleg224@mail.ru", "jkxhqoayz8ZfWSeCNoEY");
        smtp.EnableSsl = true;
        smtp.Send(m);
        Console.WriteLine("Thread: " + Thread.CurrentThread.GetHashCode() + ". Task: " + task.Name + " Done" + "\n");
    }
}

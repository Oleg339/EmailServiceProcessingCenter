using NCrontab;
public class EmailTask
{
    public int Api { get; set; }
    public int ApiOption { get; set; }
    public int TaskId { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Start1 { get; set; }
    public string? CronPeriod { get; set; }
    public string? LastTrigger { get; set; }
    public int CurrentThread { get; set; }

    public List<DateTime> datetimes()
    {
        try
        {
            return CrontabSchedule.Parse(CronPeriod).GetNextOccurrences
            (DateTime.Parse(Start1), DateTime.Now.AddHours(1)).ToList();
        }
        catch
        {
            Console.WriteLine("Error with CronPeriod: " + Name + ": " + Title + " Task ID: " + TaskId);
            return CrontabSchedule.Parse("* * 1 * *").GetNextOccurrences
                (DateTime.Now.AddYears(100), DateTime.Now.AddHours(101)).ToList();
        }
    }
}

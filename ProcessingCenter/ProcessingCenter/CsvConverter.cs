using CsvHelper;
using System.Text;

class CsvConverter
{
    public static void WriteInCsv(List<Response> res, int thread, int TaskId)
    {
        string PathCsv = $"C:\\papka\\1\\{thread}{TaskId}.csv";
        var csv = new StringBuilder();
        foreach(Response r in res)
        {
            var first = r.name;
            var second = r.info;
            var newLine = string.Format("{0},{1}", first, second);
            csv.AppendLine(newLine);
        }
        File.WriteAllText(PathCsv, csv.ToString()); 
    }
}


using Newtonsoft.Json;
public class GreatQuotesApi : Api
{
    public static List<Response> Quotes()
    {
        string url = "https://favqs.com/api/qotd", cryptoCurrency;
        

        Root1 myDeserializedClass = null;

        try
        {
            myDeserializedClass = JsonConvert.DeserializeObject<Root1>(response(url));
        }
        catch
        {
            Console.WriteLine("Проблема с подключением");
            Console.ReadKey();
        }
        List<Response> responses = new List<Response>();
        responses.Add(new Response(myDeserializedClass.quote.body, myDeserializedClass.quote.author));
        return responses;
    }

    public class Quote
    {
        public int id { get; set; }
        public bool dialogue { get; set; }
        public bool @private { get; set; }
        public List<string> tags { get; set; }
        public string url { get; set; }
        public int favorites_count { get; set; }
        public int upvotes_count { get; set; }
        public int downvotes_count { get; set; }
        public string author { get; set; }
        public string author_permalink { get; set; }
        public string body { get; set; }
    }

    public class Root1
    {
        public DateTime qotd_date { get; set; }
        public Quote quote { get; set; }
    }
}

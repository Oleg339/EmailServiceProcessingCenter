using Newtonsoft.Json;
public class CryptoApi : Api
{
    public static List<Response> Crypto(int choice)
    {
        string url, cryptoCurrency;
        if (choice == 1)
        {
            cryptoCurrency = "Bitcoin";
            url = "https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD";
        }
        else if(choice == 2)
        {
            cryptoCurrency = "Ethereum";
            url = "https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD";
        }
        else
        {
            cryptoCurrency = "Litecoin";
            url = "https://min-api.cryptocompare.com/data/price?fsym=LTC&tsyms=USD";
        }

        Price myDeserializedClass = null;

        try
        {
            myDeserializedClass = JsonConvert.DeserializeObject<Price>(response(url));
        }
        catch
        {
            Console.WriteLine("Проблема с подключением");
            Console.ReadKey();
        }
        List<Response> responses = new List<Response>();
        responses.Add(new Response(((int)myDeserializedClass.USD).ToString(), cryptoCurrency));
        return responses;


    }
}

public class Price
{
    public double USD { get; set; }
}

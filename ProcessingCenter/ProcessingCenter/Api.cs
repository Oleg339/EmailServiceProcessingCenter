using System.Net;
public class Api
{
    public static string response(string url)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpWebResponse = null;
        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        string response;
        using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
        }
        return response;
    }
}


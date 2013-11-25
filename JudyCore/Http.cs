using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;


/// <summary>
/// HTTP 辅助
/// </summary>
internal class Http
{
    public static string Post(string url, byte[] data, string useragent)
    {
        string s = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.UserAgent = useragent;
        req.ContentType = "application/x-www-form-urlencoded";
        Stream netStream = req.GetRequestStream();
        string text = DateTime.Now.ToString();
        netStream.Write(data, 0, data.Length);
        netStream.Close();
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        StreamReader sr = new StreamReader(resp.GetResponseStream());
        s = sr.ReadToEnd();
        return s;
    }

    public static string Get(string url)
    {
        string text = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.Default);
        text = sr.ReadToEnd();
        return text;

    }
}


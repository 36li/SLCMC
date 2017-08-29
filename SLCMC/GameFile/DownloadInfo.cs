using System;
using Newtonsoft.Json.Linq;

namespace SLCMC.GameFile
{
    public class DownloadInfo
    {
        public DownloadInfo(string sha1 = null, int size = -1, string url = null)
        {
            Sha1 = sha1;
            Size = size;
            Url = url;
        }

        public string Sha1 { get; }
        public int Size { get; }
        public string Url { get; }

        public static DownloadInfo Parse(JObject json)
        {
            JToken temp;

            string sha1 = null, url = null;
            int size = -1;

            if (json.TryGetValue("sha1", out temp) && temp.Type == JTokenType.String)
                sha1 = temp.ToString();
            if (json.TryGetValue("size", out temp) && temp.Type == JTokenType.Integer)
                size = Convert.ToInt32(temp.ToString());
            if (json.TryGetValue("url", out temp) && temp.Type == JTokenType.String)
                url = temp.ToString();

            return new DownloadInfo(sha1, size, url);
        }
    }
}

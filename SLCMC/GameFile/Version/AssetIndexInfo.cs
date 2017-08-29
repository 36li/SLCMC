using System;
using Newtonsoft.Json.Linq;

namespace SLCMC.GameFile.Version
{
    public class AssetIndexInfo : DownloadInfo
    {
        public AssetIndexInfo(string id = null, string sha1 = null, int size = -1, string url = null, int totalSize = -1)
            : base(sha1, size, url)
        {
            Id = id;
            TotalSize = totalSize;
        }

        public string Id { get; }
        public int TotalSize { get; }

        public static new AssetIndexInfo Parse(JObject json)
        {
            JToken temp;

            string id = null, sha1 = null, url = null;
            int size = -1, totalSize = -1;

            if (json.TryGetValue("id", out temp) && temp.Type == JTokenType.String)
                id = temp.ToString();
            if (json.TryGetValue("sha1", out temp) && temp.Type == JTokenType.String)
                sha1 = temp.ToString();
            if (json.TryGetValue("size", out temp) && temp.Type == JTokenType.Integer)
                size = Convert.ToInt32(temp.ToString());
            if (json.TryGetValue("url", out temp) && temp.Type == JTokenType.String)
                url = temp.ToString();
            if (json.TryGetValue("totalSize", out temp) && temp.Type == JTokenType.Integer)
                totalSize = Convert.ToInt32(temp.ToString());

            return new AssetIndexInfo(id, sha1, size, url, totalSize);
        }
    }
}

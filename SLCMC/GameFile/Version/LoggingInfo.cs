using System;
using Newtonsoft.Json.Linq;

namespace SLCMC.GameFile.Version
{
    public class LoggingInfo
    {
        public class LoggingFileInfo : DownloadInfo
        {
            public LoggingFileInfo(string id = null, string sha1 = null, int size = -1, string url = null) : base(sha1, size, url)
            {
                Id = id;
            }

            public string Id { get; }

            public static new LoggingFileInfo Parse(JObject json)
            {
                JToken temp;

                string id = null, sha1 = null, url = null;
                int size = -1;

                if (json.TryGetValue("id", out temp) && temp.Type == JTokenType.String)
                    id = temp.ToString();
                if (json.TryGetValue("sha1", out temp) && temp.Type == JTokenType.String)
                    sha1 = temp.ToString();
                if (json.TryGetValue("size", out temp) && temp.Type == JTokenType.Integer)
                    size = Convert.ToInt32(temp.ToString());
                if (json.TryGetValue("url", out temp) && temp.Type == JTokenType.String)
                    url = temp.ToString();

                return new LoggingFileInfo(id, sha1, size, url);
            }
        }

        public LoggingInfo(LoggingFileInfo file, string argument, string type)
        {
            File = file;
            Argument = argument;
            Type = type;
        }

        public LoggingFileInfo File { get; }
        public string Argument { get; }
        public string Type { get; }

        public static LoggingInfo Parse(JObject json)
        {
            JToken temp;

            string argument = null, type = null;
            LoggingFileInfo file = new LoggingFileInfo();

            if (json.TryGetValue("file", out temp) && temp.Type == JTokenType.Object)
                file = LoggingFileInfo.Parse(JObject.Parse(temp.ToString()));
            if (json.TryGetValue("argument", out temp) && temp.Type == JTokenType.String)
                argument = temp.ToString();
            if (json.TryGetValue("type", out temp) && temp.Type == JTokenType.String)
                type = temp.ToString();

            return new LoggingInfo(file, argument, type);
        }
    }
}

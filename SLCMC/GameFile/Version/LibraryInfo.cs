using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SLCMC.GameFile.Version
{
    public class LibraryInfo
    {
        public class RuleInfo
        {
            public class OsInfo
            {
                public OsInfo(string name = null, string version = null)
                {
                    Name = name;
                    Version = version;
                }

                public string Name { get; }
                public string Version { get; }

                public static OsInfo Parse(JObject json)
                {
                    JToken temp;

                    string name = null, version = null;

                    if (json.TryGetValue("name", out temp) && temp.Type == JTokenType.String)
                        name = temp.ToString();
                    if (json.TryGetValue("version", out temp) && temp.Type == JTokenType.String)
                        version = temp.ToString();

                    return new OsInfo(name, version);
                }
            }

            public RuleInfo(string action, OsInfo os)
            {
                Action = action;
                Os = os;
            }

            public RuleInfo()
            {
                Action = null;
                Os = new OsInfo();
            }

            public string Action { get; }
            public OsInfo Os { get; }

            public static RuleInfo Parse(JObject json)
            {
                JToken temp;

                string action = null;
                OsInfo os = new OsInfo();

                if (json.TryGetValue("action", out temp) && temp.Type == JTokenType.String)
                    action = temp.ToString();
                if (json.TryGetValue("os", out temp) && temp.Type == JTokenType.Object)
                    os = OsInfo.Parse(JObject.Parse(temp.ToString()));

                return new RuleInfo(action, os);
            }
        }

        public class LibraryFileInfo : DownloadInfo
        {
            public LibraryFileInfo(string path = null, string sha1 = null, int size = -1, string url = null) : base(sha1, size, url)
            {
                Path = path;
            }

            public string Path { get; }

            public static new LibraryFileInfo Parse(JObject json)
            {
                JToken temp;

                string path = null, sha1 = null, url = null;
                int size = -1;

                if (json.TryGetValue("path", out temp) && temp.Type == JTokenType.String)
                    path = temp.ToString();
                if (json.TryGetValue("sha1", out temp) && temp.Type == JTokenType.String)
                    sha1 = temp.ToString();
                if (json.TryGetValue("size", out temp) && temp.Type == JTokenType.Integer)
                    size = Convert.ToInt32(temp.ToString());
                if (json.TryGetValue("url", out temp) && temp.Type == JTokenType.String)
                    url = temp.ToString();

                return new LibraryFileInfo(path, sha1, size, url);
            }
        }

        public class LibraryDownloadsInfo
        {
            public LibraryDownloadsInfo(Dictionary<string, LibraryFileInfo> classifiers, LibraryFileInfo artifact)
            {
                Classifiers = classifiers;
                Artifact = artifact;
            }

            public LibraryDownloadsInfo()
            {
                Classifiers = new Dictionary<string, LibraryFileInfo>();
                Artifact = new LibraryFileInfo();
            }

            public Dictionary<string, LibraryFileInfo> Classifiers { get; }
            public LibraryFileInfo Artifact { get; }

            public static LibraryDownloadsInfo Parse(JObject json)
            {
                JToken temp;

                Dictionary<string, LibraryFileInfo>  classifiers = new Dictionary<string, LibraryFileInfo>();
                LibraryFileInfo artifact = new LibraryFileInfo();

                if (json.TryGetValue("classifiers", out temp) && temp.Type == JTokenType.Object)
                    foreach (KeyValuePair<string, JToken> jsonPro in JObject.Parse(temp.ToString()))
                        if(jsonPro.Value.Type == JTokenType.Object)
                            classifiers.Add(jsonPro.Key, LibraryFileInfo.Parse(JObject.Parse(jsonPro.Value.ToString())));
                if (json.TryGetValue("artifact", out temp) && temp.Type == JTokenType.Object)
                    artifact = LibraryFileInfo.Parse(JObject.Parse(temp.ToString()));

                return new LibraryDownloadsInfo(classifiers, artifact);
            }
        }

        public LibraryInfo(string name, Dictionary<string, string> natives, List<RuleInfo> rules, LibraryDownloadsInfo downloads)
        {
            Name = name;
            Natives = natives;
            Rules = rules;
            Downloads = downloads;
        }

        

        public string Name { get; }
        public Dictionary<string, string> Natives { get; }
        public List<RuleInfo> Rules { get; }
        public LibraryDownloadsInfo Downloads { get; }

        public string Path
        {
            get
            {
                string[] text = Name.Split(new string[] {":"}, 3, StringSplitOptions.RemoveEmptyEntries);
                string native = null;
                if (Natives.TryGetValue("windows", out native))
                {
                    if (Environment.Is64BitOperatingSystem)  native.Replace("${arch}", "64");
                    else native.Replace("${arch}", "32");
                    return text[0].Replace('.', '/') + "/" + text[1] + "/" + text[2] + "/" + text[1] + "-" + native + "-" +
                           text[2] + ".jar";
                }  
                else return text[0].Replace('.', '/') + "/" + text[1] + "/" + text[2] + "/" + text[1] + "-" + text[2] + ".jar";
            }
        }

        public LibraryFileInfo GetFileInfo()
        {
            
            string native;
            if (!IsAllow())
                return new LibraryFileInfo();
            if (Natives.TryGetValue("windows", out native))
            {
                if (Environment.Is64BitOperatingSystem) native.Replace("${arch}", "64");
                else native.Replace("${arch}", "32");
                LibraryFileInfo fileInfo;
                if (Downloads.Classifiers.TryGetValue(native, out fileInfo))
                    return fileInfo;
                else
                    return new LibraryFileInfo();
            }
            else
                return Downloads.Artifact;
        }

        public bool IsAllow()
        {
            if (Rules.Count == 0)
                return true;
            bool allow = false;
            foreach (RuleInfo rule in Rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Os.Name) || rule.Os.Name.Equals("windows"))
                    allow = rule.Action.Equals("allow");
            }
            return allow;
        }

        public static LibraryInfo Parse(JObject json)
        {
            JToken temp;

            string name = null;
            Dictionary<string, string> natives = new Dictionary<string, string>();
            List<RuleInfo> rules = new List<RuleInfo>();
            LibraryDownloadsInfo downloads = new LibraryDownloadsInfo();

            if (json.TryGetValue("name", out temp) && temp.Type == JTokenType.String)
                name = temp.ToString();
            if (json.TryGetValue("natives", out temp) && temp.Type == JTokenType.Object)
                foreach (KeyValuePair<string, JToken> jsonPro in JObject.Parse(temp.ToString()))
                    if (jsonPro.Value.Type == JTokenType.String)
                        natives.Add(jsonPro.Key, jsonPro.Value.ToString());
            if (json.TryGetValue("rules", out temp) && temp.Type == JTokenType.Array)
                foreach (JToken jsonTok in JArray.Parse(temp.ToString()))
                    if (jsonTok.Type == JTokenType.Object)
                        rules.Add(RuleInfo.Parse(JObject.Parse(jsonTok.ToString())));
            if (json.TryGetValue("downloads", out temp) && temp.Type == JTokenType.Object)
                downloads = LibraryDownloadsInfo.Parse(JObject.Parse(temp.ToString()));

            return new LibraryInfo(name, natives, rules, downloads);
        }
    }
}

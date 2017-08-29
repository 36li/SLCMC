using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SLCMC.GameFile.Version
{
    public class VersionData
    {
        public VersionData(AssetIndexInfo assetIndex,
                           string assets,
                           Dictionary<string, DownloadInfo> downloads,
                           string id,
                           string inheritsFrom,
                           string jar,
                           List<LibraryInfo> libraries,
                           Dictionary<string, LoggingInfo> logging,
                           string mainClass,
                           string minecraftArguments,
                           int mininumLauncherVersion,
                           string releaseTime,
                           string time,
                           string type)
        {
            AssetIndex = assetIndex;
            Assets = assets;
            Downloads = downloads;
            Id = id;
            InheritsFrom = inheritsFrom;
            Jar = jar;
            Libraries = libraries;
            Logging = logging;
            MainClass = mainClass;
            MinecraftArguments = minecraftArguments;
            MininumLauncherVersion = mininumLauncherVersion;
            ReleaseTime = releaseTime;
            Time = time;
            Type = type;
        }

        public AssetIndexInfo AssetIndex { get; }
        public string Assets { get; }
        public Dictionary<string, DownloadInfo> Downloads { get; }
        public string Id { get; }
        public string InheritsFrom { get; }
        public string Jar { get; }
        public List<LibraryInfo> Libraries { get; }
        public Dictionary<string, LoggingInfo> Logging { get; }
        public string MainClass { get; }
        public string MinecraftArguments { get; }
        public int MininumLauncherVersion { get; }
        public string ReleaseTime { get; }
        public string Time { get; }
        public string Type { get; }

        public static VersionData Parse(JObject json)
        {
            JToken temp ;

            string assets = null, id = null, inheritsFrom = null, jar = null, mainClass = null;
            string minecraftArguments = null, releaseTime = null, time = null, type = null;
            int minimumLauncherVersion = -1;
            Dictionary<string, DownloadInfo> downloads = new Dictionary<string, DownloadInfo>();
            List<LibraryInfo> libraries = new List<LibraryInfo>();
            Dictionary<string, LoggingInfo> logging = new Dictionary<string, Version.LoggingInfo>();
            AssetIndexInfo assetIndex = new AssetIndexInfo();

            if (json.TryGetValue("assetIndex", out temp) && temp.Type == JTokenType.Object)
                assetIndex = AssetIndexInfo.Parse(JObject.Parse(temp.ToString()));
            if (json.TryGetValue("assets", out temp) && temp.Type == JTokenType.String)
                assets = temp.ToString();
            if (json.TryGetValue("downloads", out temp) && temp.Type == JTokenType.Object)
                foreach (KeyValuePair<string, JToken> jsonPro in JObject.Parse(temp.ToString()))
                    if (jsonPro.Value.Type == JTokenType.Object)
                        downloads.Add(jsonPro.Key, DownloadInfo.Parse(JObject.Parse(jsonPro.Value.ToString())));
            if (json.TryGetValue("id", out temp) && temp.Type == JTokenType.String)
                id = temp.ToString();
            if (json.TryGetValue("inheritsFrom", out temp) && temp.Type == JTokenType.String)
                inheritsFrom = temp.ToString();
            if (json.TryGetValue("jar", out temp) && temp.Type == JTokenType.String)
                jar = temp.ToString();
            if (json.TryGetValue("libraries", out temp) && temp.Type == JTokenType.Array)
                foreach (JToken jsonTok in JArray.Parse(temp.ToString()))
                    if (jsonTok.Type == JTokenType.Object)
                        libraries.Add(LibraryInfo.Parse(JObject.Parse(jsonTok.ToString())));
            if (json.TryGetValue("logging", out temp) && temp.Type == JTokenType.Object)
                foreach (KeyValuePair<string, JToken> jsonPro in JObject.Parse(temp.ToString()))
                    if (jsonPro.Value.Type == JTokenType.Object)
                        logging.Add(jsonPro.Key, Version.LoggingInfo.Parse(JObject.Parse(jsonPro.Value.ToString())));
            if (json.TryGetValue("mainClass", out temp) && temp.Type == JTokenType.String)
                mainClass = temp.ToString();
            if (json.TryGetValue("minecraftArguments", out temp) && temp.Type == JTokenType.String)
                minecraftArguments = temp.ToString();
            if (json.TryGetValue("minimumLauncherVersion", out temp) && temp.Type == JTokenType.Integer)
                minimumLauncherVersion = Convert.ToInt32(temp.ToString());
            if (json.TryGetValue("releaseTime", out temp) && temp.Type == JTokenType.String)
                releaseTime = temp.ToString();
            if (json.TryGetValue("time", out temp) && temp.Type == JTokenType.String)
                time = temp.ToString();
            if (json.TryGetValue("type", out temp) && temp.Type == JTokenType.String)
                type = temp.ToString();

            return new VersionData(assetIndex, assets, downloads, id, inheritsFrom, jar, libraries, logging, mainClass,
                minecraftArguments, minimumLauncherVersion, releaseTime, time, type);
        }
    }
}

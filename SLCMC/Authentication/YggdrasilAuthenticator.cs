using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SLCMC.Authentication
{
    /// <summary>
    /// Yggdrasil验证器
    /// </summary>
    public class YggdrasilAuthenticator : IAuthenticator
    {
        public YggdrasilAuthenticator()
        {
            Url = "https://authserver.mojang.com";
        }

        /// <summary>
        /// Yggdrasil验证服务器地址
        /// </summary>
        public string Url { get; set; }

        public YggdrasilInfo data;

        public Guid AccessToken { get { return data.AccessToken; } }
        public Guid ClientToken { get { return data.ClientToken; } }

        /// <summary>
        /// 使用用户名和密码登录到Yggdrasil服务器
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientToken">客户端标识</param>
        /// <param name="requestUser">是否请求账号数据</param>
        /// <returns></returns>
        public YggdrasilError Authenticate(string username, string password, Guid? clientToken = null, bool? requestUser = null)
        {
            JObject jsonData = new JObject();

            jsonData["agent"] = new JObject();
            jsonData["agent"]["name"] = "Minecraft";
            jsonData["agent"]["version"] = 1;
            jsonData["username"] = username;
            jsonData["password"] = password;
            if (clientToken.HasValue) jsonData["clientToken"] = clientToken.Value.ToString("N");
            if (requestUser.HasValue) jsonData["requestUser"] = requestUser.Value;

            byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonData.ToString());

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + "/authenticate");

                request.Method = "POST";
                request.Timeout = 12000;
                request.ContentType = "application/json";
                request.ContentLength = jsonByteArray.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(jsonByteArray, 0, jsonByteArray.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                if (Convert.ToInt32(response.StatusCode) == 200)
                {
                    data = YggdrasilInfo.Parse(JObject.Parse(reader.ReadToEnd()));
                    return YggdrasilError.Null();
                }
                else
                {
                    return YggdrasilError.RequestError(JObject.Parse(reader.ReadToEnd()));
                }
            }
            catch (Exception e)
            {
                return YggdrasilError.Exception(e);
            }
        }

        public YggdrasilError Refresh(Guid accessToken, Guid clientToken, bool? requestUser = null)
        {
            JObject jsonData = new JObject();

            jsonData["accessToken"] = accessToken.ToString("N");
            jsonData["clientToken"] = clientToken.ToString("N");
            if (requestUser.HasValue) jsonData["requestUser"] = requestUser.Value;

            byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonData.ToString());

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + "/refresh");

                request.Method = "POST";
                request.Timeout = 12000;
                request.ContentType = "application/json";
                request.ContentLength = jsonByteArray.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(jsonByteArray, 0, jsonByteArray.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                if (Convert.ToInt32(response.StatusCode) == 200)
                {
                    data = YggdrasilInfo.Parse(JObject.Parse(reader.ReadToEnd()));
                    return YggdrasilError.Null();
                }
                else
                {
                    return YggdrasilError.RequestError(JObject.Parse(reader.ReadToEnd()));
                }
            }
            catch (Exception e)
            {
                return YggdrasilError.Exception(e);
            }
        }


        //IAuthenticator接口

        /// <summary>
        /// 获取玩家信息
        /// </summary>
        /// <returns>玩家信息</returns>
        public AuthenticationInfo Auth()
        {
            return new AuthenticationInfo(new AuthenticationProfile(data.SelectedProfile.name, data.SelectedProfile.id), 
                                          data.AccessToken, data.User.Properties, "mojang");
        }

        /// <summary>
        /// 验证器名称
        /// </summary>
        public string Type { get { return "yggdrasil"; } }

    }

    public class YggdrasilInfo
    {
        /// <summary>
        /// 解析Yggdrasil返回的json数据
        /// </summary>
        /// <param name="json">Yggdrasil返回json的数据</param>
        /// <returns></returns>
        public static YggdrasilInfo Parse(JObject json)
        {
            YggdrasilInfo yggdrasil = new YggdrasilInfo();

            yggdrasil.AccessToken = Guid.Parse(json["accessToken"].ToString());
            yggdrasil.ClientToken = Guid.Parse(json["clientToken"].ToString());
            if (json["selectedProfile"].Type == JTokenType.Object)
            {
                yggdrasil.SelectedProfile.id = Guid.Parse(json["selectedProfile"]["id"].ToString());
                yggdrasil.SelectedProfile.name = json["selectedProfile"]["name"].ToString();
            }
            if (json["availableProfiles"].Type == JTokenType.Array)
            {
                JArray availableProfiles = JArray.Parse(json["availableProfiles"].ToString());
                foreach (JObject data in availableProfiles)
                {
                    ProfileInfo profile = new ProfileInfo
                    {
                        id = Guid.Parse(data["id"].ToString()),
                        name = data["name"].ToString()
                    };
                    yggdrasil.AvailableProfiles.Add(profile);
                }
            }
            yggdrasil.User.id = Guid.Parse(json["user"]["id"].ToString());
            if (json["user"]["properties"].Type == JTokenType.Array)
            {
                JArray properties = JArray.Parse(json["user"]["properties"].ToString());
                foreach (JObject data in properties)
                {
                    string name = data["name"].ToString();
                    string value = data["value"].ToString();
                    yggdrasil.User.Properties.Add(name, value);
                }
            }
            return yggdrasil;
        }

        /// <summary>
        /// 
        /// </summary>
        public class ProfileInfo
        {
            /// <summary>
            /// 角色的uuid
            /// </summary>
            public Guid id { get; set; }

            /// <summary>
            /// 角色名
            /// </summary>
            public string name { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class UserInfo
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public UserInfo()
            {
                Properties = new Dictionary<string, string>();
            }

            /// <summary>
            /// 账号的uuid
            /// </summary>
            public Guid id { get; set; }

            /// <summary>
            /// 账号属性表
            /// </summary>
            public Dictionary<string, string> Properties { get; set; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public YggdrasilInfo()
        {
            SelectedProfile = new ProfileInfo();
            AvailableProfiles = new List<ProfileInfo>();
            User = new UserInfo();
        }

        /// <summary>
        /// 访问标识
        /// </summary>
        public Guid AccessToken { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public Guid ClientToken { get; set; }

        /// <summary>
        /// 已经选择的角色信息
        /// </summary>
        public ProfileInfo SelectedProfile { get; set; }

        /// <summary>
        /// 可用角色信息表
        /// </summary>
        public List<ProfileInfo> AvailableProfiles { get; set; }

        /// <summary>
        /// 账号信息
        /// </summary>
        public UserInfo User { get; set; }

    }

    public class YggdrasilError
    {

        public static YggdrasilError Null()
        {
            YggdrasilError rError = new YggdrasilError();
            rError.Type = YggdrasilErrorType.Null;

            return rError;
        }

        public static YggdrasilError RequestError(JObject json)
        {
            YggdrasilError rError = new YggdrasilError();
            rError.Type = YggdrasilErrorType.RequestError;
            rError.Error = json["error"].ToString();
            rError.ErrorMessage = json["errorMessage"].ToString();
            rError.Cause = json["cause"].ToString();

            return rError;
        }

        public static YggdrasilError Exception(Exception exception)
        {
            YggdrasilError rError = new YggdrasilError();
            rError.Type = YggdrasilErrorType.Exception;
            rError.Error = exception.GetType().ToString();
            rError.ErrorMessage = exception.Message;
            return rError;
        }

        public YggdrasilErrorType Type { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Cause { get; set; }
    }

    public enum YggdrasilErrorType
    {
        Null = 0,
        Exception = 1,
        RequestError = 2
    }
}

using System;
using System.Collections.Generic;

namespace SLCMC.Authentication
{
    /// <summary>
    /// 验证信息类
    /// </summary>
    public class AuthenticationInfo
    {
        /// <summary>
        /// 初始化验证信息
        /// </summary>
        /// <param name="profile">角色信息</param>
        /// <param name="accessToken">账号访问标识</param>
        /// <param name="properties">账号属性表</param>
        /// <param name="type">角色类型</param>
        public AuthenticationInfo(AuthenticationProfile profile, Guid accessToken, Dictionary<string, string> properties, string type)
        {
            Profile = profile;
            AccessToken = accessToken;
            Properties = properties;
            Type = type;
        }

        /// <summary>
        /// 角色信息
        /// </summary>
        public AuthenticationProfile Profile { get; }

        /// <summary>
        /// 账号访问标识
        /// </summary>
        public Guid AccessToken { get; }

        /// <summary>
        /// 账号属性表
        /// </summary>
        public Dictionary<string, string> Properties { get; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string Type { get; }

    }
}

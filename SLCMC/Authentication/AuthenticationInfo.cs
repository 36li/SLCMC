using System;
using System.Collections.Generic;

namespace SLCMC.Authentication
{
    /// <summary>
    /// 角色信息类
    /// </summary>
    public class AuthenticationInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">角色的uuid</param>
        /// <param name="accessToken">访问标识</param>
        /// <param name="properties">账号属性表</param>
        /// <param name="type">角色类型</param>
        public AuthenticationInfo(string name, Guid id, Guid accessToken, Dictionary<string, string> properties, string type)
        {
            Name = name;
            Id = id;
            AccessToken = accessToken;
            Properties = properties;
            Type = type;
        }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 角色的uuid
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 访问标识
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

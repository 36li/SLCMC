using System;
using System.Collections.Generic;

namespace SLCMC.Authentication
{
    public class AuthenticationInfo
    {
        /// <summary>
        /// 玩家名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 玩家的uuid
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 玩家的Session
        /// </summary>
        public Guid AccessToken { get; }

        /// <summary>
        /// 玩家属性表
        /// </summary>
        public Dictionary<string, string> Properties { get; }

        /// <summary>
        /// 玩家类型
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">玩家名</param>
        /// <param name="id">玩家的uuid</param>
        /// <param name="accessToken">玩家的Session</param>
        /// <param name="properties">玩家属性表</param>
        /// <param name="type">玩家类型</param>
        public AuthenticationInfo(string name, Guid id, Guid accessToken, Dictionary<string, string> properties, string type)
        {
            Name = name;
            Id = id;
            AccessToken = accessToken;
            Properties = properties;
            Type = type;
        }
    }
}

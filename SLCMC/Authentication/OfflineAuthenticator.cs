using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SLCMC.Authentication
{
    /// <summary>
    /// 离线验证器
    /// </summary>
    public class OfflineAuthenticator : IAuthenticator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerName">角色名，如果为null将使用player作为角色名</param>
        /// <param name="playerId">角色的uuid，如果为null将使用角色名生成uuid</param>
        public OfflineAuthenticator(string playerName = null, Guid? playerId = null)
        {
            if (string.IsNullOrWhiteSpace(playerName)) PlayerName = "player";
            else PlayerName = playerName;
            if (playerId.HasValue) PlayerId = playerId.Value;
            else
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                PlayerId = new Guid(md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(PlayerName)));
            }
        }

        /// <summary>
        /// 角色名
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 角色的uuid
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// 账号属性表
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

        //IAuthenticator接口
        #region

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns>角色信息</returns>
        public AuthenticationInfo Auth()
        {
            return new AuthenticationInfo(PlayerName, PlayerId, Guid.NewGuid(), Properties, "legacy");
        }

        /// <summary>
        /// 验证器名称
        /// </summary>
        public string Type { get { return "offline"; } }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SLCMC.Authentication
{
    public class OfflineAuthenticator : IAuthenticator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerName">玩家名</param>
        /// <param name="playerId">玩家的uuid，如果为null将使用玩家名生成uuid</param>
        public OfflineAuthenticator(string playerName = "player", Guid? playerId = null)
        {
            PlayerName = playerName;
            if (playerId.HasValue)
                PlayerId = playerId.Value;
            else
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                PlayerId = new Guid(md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(PlayerName)));
            }      
        }

        /// <summary>
        /// 玩家名
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 玩家的uuid
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// 玩家属性表
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

        //IAuthenticator接口
        #region
        /// <summary>
        /// 获取玩家信息
        /// </summary>
        /// <returns>玩家信息</returns>
        public AuthenticationInfo Auth()
        {
            return new AuthenticationInfo(PlayerName, PlayerId, Guid.NewGuid(), Properties, "Legacy");
        }

        /// <summary>
        /// 验证器名称
        /// </summary>
        public string Type { get { return "offline"; } }
        #endregion

    }
}

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
        /// 角色信息
        /// </summary>
        public AuthenticationProfile profile;

        /// <summary>
        /// 使用角色名和uuid初始化离线验证器
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">角色的uuid，如果为null将自动生成一个</param>
        public OfflineAuthenticator(string name, Guid? id = null)
        {
            try
            {
                SetProfile(name, id);
            }
            catch (AuthenticationException exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 设置角色名和uuid
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">角色的uuid，如果为null将自动生成一个</param>
        public void SetProfile(string name, Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new AuthenticationException("Invalid username");
            if (id.HasValue) profile = new AuthenticationProfile(name, id.Value);
            else
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                profile = new AuthenticationProfile(name,
                                                    new Guid(md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(name))));
            }
        }

        //IAuthenticator接口

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns>角色信息</returns>
        public AuthenticationInfo Auth()
        {
            return new AuthenticationInfo(profile, Guid.NewGuid(), null, "legacy");
        }

        /// <summary>
        /// 验证器名称
        /// </summary>
        public string Type { get { return "offline"; } }

    }
}

using System;

namespace SLCMC.Authentication
{
    /// <summary>
    /// 角色信息类
    /// </summary>
    public class AuthenticationProfile
    {
        /// <summary>
        /// 初始化角色信息
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">角色的uuid</param>
        public AuthenticationProfile(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 角色的uuid
        /// </summary>
        public Guid Id { get; }
    }
}

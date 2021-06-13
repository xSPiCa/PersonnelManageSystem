using System;
using System.Collections.Generic;

namespace PersonnelManageSystem.Utils
{
    /// <summary>
    /// 员工 角色枚举
    /// 用于 Staff.post字段
    /// </summary>
    public class RoleInfo
    {
        //初始化 角色字段对应的字符串
        private static readonly Dictionary<Role, String> RoleStr = new Dictionary<Role, string>
        {
            {Role.Boss, "admin"}, {Role.Developer, "admin"}, {Role.Laborer, "user"}, {Role.Manager, "admin"}
        };
        public static String GetRoleStr(Role r)
        {
            return RoleStr[r];
        }
    }

    public enum Role
    {
        Developer = 10000,
        Boss = 10001,
        Manager = 10002,
        Laborer = 10003,
    }
    
}
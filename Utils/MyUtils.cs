using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;

namespace PersonnelManageSystem.Utils
{
    public static class MyUtils
    {
        /// <summary>
        /// 从认证信息中获取 数据库中保存的用户信息
        /// 如果这个函数崩了 大概率是因为没有认证之后调用此函数导致user.Claims是空
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetHasAuthentication (HttpContext httpContext,PersonnelContext dbContext)
        {
            var sid =int.Parse(((from claim in httpContext.User.Claims
                where claim.Type == "SID"
                select claim).Single().Value));
        
            ReturnResult result = await StaffMapper.GetStaffById(sid, dbContext);
            return result;
        }
    }
}
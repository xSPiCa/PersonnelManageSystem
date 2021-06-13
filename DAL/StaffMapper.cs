using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PersonnelManageSystem.DAL
{
    /// <summary>
    /// 处理Staff数据表相关的数据访问
    /// 注意删除并没有删除表中的数据而是将Enable置为false
    /// 
    /// </summary>
    public static class StaffMapper
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetStaffById(int id,PersonnelContext context)
        {
            try

            {
                 var query = await (from s in context.Staff
                    where s.StaffId == id && s.Enable == true
                    select s).SingleAsync();
                
                return ReturnResult.Success(query);
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(StatusCode.DbException);
            }
        }

        

        /// <summary>
        /// 根据用户名密码查询员工(管理)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetStaffByNamePassword(String username,String password,PersonnelContext context)
        {
            try
            {

                var query = await context.Staff
                    .SingleAsync(staff => staff.Enable == true && staff.Name == username && staff.Password == password);

                return ReturnResult.Success(query);
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(StatusCode.DbException);
            }

        }
        
        
        /// <summary>
        /// 查询一个同一部门下的员工
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetSameDepartmentStaff(Staff staff,PersonnelContext context)
        {
            var queryResult = from s in context.Staff
                where staff.DepartmentId == s.DepartmentId && s.Enable == true
                select s;
            
                List<Staff> staves = await queryResult.ToListAsync();
            
                return new ReturnResult()
                {
                    Data = staves
                };
        }
        
        /// <summary>
        /// 重载的分页版本查询
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="limit">分页长度</param>
        /// <param name="offset">分页偏移</param>
        /// <param name="context">数据库上下文</param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetSameDepartmentStaff(Staff staff,int limit,int offset,PersonnelContext context)
        {

            //include 提供预加载 详情：https://docs.microsoft.com/zh-cn/ef/ef6/querying/related-data?redirectedfrom=MSDN
            // 此处如果不使用预加载 获得的数据 Department字段将会为null
            // 此外请谨慎使用预加载 因为当两个实体互相关联的情况下 序列化将会进入死循环

            try
            {
                var queryResult = context.Staff
                    .Where(s => s.DepartmentId == staff.DepartmentId && s.Enable == true )
                    .Include("Department");

                int count = queryResult.Count();
                List<Staff> staves = await queryResult.OrderBy(staff => staff.StaffId).Skip(limit * offset).Take(limit).ToListAsync();

                return ReturnResult.Success(staves,count);
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(StatusCode.DbException);
            }

        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> UpdateStaff(Staff staff, PersonnelContext context)
        {
            try
            {
                var queryResult = await  context.Staff.SingleAsync(s => staff.StaffId == s.StaffId);

                queryResult.Address = staff.Address;
                queryResult.Age = staff.Age;
                queryResult.Name = staff.Name;
                queryResult.Password = staff.Password;
                queryResult.Post = staff.Post;
                queryResult.Sex = staff.Sex;
                queryResult.DepartmentId = staff.DepartmentId;
                queryResult.Phone = staff.Phone;
                await context.SaveChangesAsync();

                return ReturnResult.Success();
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(StatusCode.DbException);
            }
        }


        /// <summary>
        /// 将员工 enable置为 0 
        /// </summary>
        /// <returns></returns>
        public static async Task<ReturnResult> InValidStaff(int id, PersonnelContext context)
        {
            try
            {
                var queryResult = await context.Staff.SingleAsync(s => s.StaffId == id);

                queryResult.Enable = false;
                
                await context.SaveChangesAsync();
                return ReturnResult.Success();

            }
            catch (Exception e)
            {
                return ReturnResult.Fail(StatusCode.DbException);
            }

        }

    }
}
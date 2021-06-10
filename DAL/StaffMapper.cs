using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PersonnelManageSystem.DAL
{
    /// <summary>
    /// 处理Staff数据表相关的数据访问
    /// </summary>
    public static class StaffMapper
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult GetStaffById(int id,PersonnelContext context)
        {
            try
            {
                var query = (from s in context.Staff
                    where s.StaffId == id && s.Enable == null
                    select s).Single();
                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = query,
                };
            }
            catch (ArgumentNullException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.InValidUserInfo
                };

            }
            catch (InvalidOperationException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.UserDbException
                };
            }

            //其他异常应该抛出 
        }


        /// <summary>
        /// 根据用户名密码查询员工(管理)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult GetStaffByNamePassword(String username,String password,PersonnelContext context)
        {
            try
            {

                var query = context.Staff
                    .Single(staff => staff.Enable == true && staff.Name == username && staff.Password == password);

                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = query
                };
            }
            catch (ArgumentNullException e)
            {

                return new ReturnResult()
                {
                    Code = StatusCode.UserDbException,
                    Data = null
                };
            }
            catch (InvalidOperationException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.InValidUserInfo,
                    Data = null
                };
            }
        }
        
        
        /// <summary>
        /// 查询一个同一部门下的员工
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult GetSameDepartmentStaff(Staff staff,PersonnelContext context)
        {
            var queryResult = from s in context.Staff
                where staff.DepartmentId == s.DepartmentId && s.Enable == true
                select s;
            
            List<Staff> staves = queryResult.ToList();
            
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
        public static ReturnResult GetSameDepartmentStaff(Staff staff,int limit,int offset,PersonnelContext context)
        {

            //include 提供预加载 详情：https://docs.microsoft.com/zh-cn/ef/ef6/querying/related-data?redirectedfrom=MSDN
            // 此处如果不使用预加载 获得的数据 Department字段将会为null
            // 此外请谨慎使用预加载 因为当两个实体互相关联的情况下 序列化将会进入死循环
            var queryResult = context.Staff
                .Where(s => s.DepartmentId == staff.DepartmentId && s.Enable == true )
                .Include("Department");

            int count = queryResult.Count();
            List<Staff> staves = queryResult.OrderBy(staff => staff.StaffId).Skip(limit * offset).Take(limit).ToList();

            return new ReturnResult()
            {
                Data = staves,
                Total =  count
            };
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult UpdateStaff(Staff staff, PersonnelContext context)
        {
            try
            {
                var queryResult = context.Staff.Single(s => staff.StaffId == s.StaffId);

                queryResult.Address = staff.Address;
                queryResult.Age = staff.Age;
                queryResult.Name = staff.Name;
                queryResult.Password = staff.Password;
                queryResult.Post = staff.Post;
                queryResult.Sex = staff.Sex;
                queryResult.DepartmentId = staff.DepartmentId;
                queryResult.Phone = staff.Phone;
                context.SaveChanges();
                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = null
                };
            }
            catch (ArgumentNullException e)
            {

                return new ReturnResult()
                {
                    Code = StatusCode.UserDbException,
                    Data = null
                };
            }
            catch (InvalidOperationException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.InValidUserInfo
                };
            }
        }


        /// <summary>
        /// 将员工 enable置为 0 
        /// </summary>
        /// <returns></returns>
        public static ReturnResult InValidStaff(int id, PersonnelContext context)
        {
            try
            {
                var queryResult = context.Staff.Single(s => s.StaffId == id);

                queryResult.Enable = false;
                
                context.SaveChanges();
                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = null
                };

            }
            catch (ArgumentNullException e)
            {

                return new ReturnResult()
                {
                    Code = StatusCode.UserDbException,
                    Data = null
                };
            }
            catch (InvalidOperationException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.InValidUserInfo
                };
            }
        }

    }
}
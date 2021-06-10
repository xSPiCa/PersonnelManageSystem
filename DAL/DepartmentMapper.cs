using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;
using System.Linq ;
namespace PersonnelManageSystem.DAL
{
    /// <summary>
    /// 处理Department数据表相关的数据访问
    /// </summary>
    public static class DepartmentMapper
    {
        /// <summary>
        /// 获取所以部门信息
        /// </summary>
        /// <returns></returns>
        public static ReturnResult GetAllDepartment(PersonnelContext context)
        {
            try
            {
                var query = from d in context.Department
                    select d;

                List<Department> departments =query.ToList();

                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = departments
                };
            }
            catch (ArgumentException e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.DepartInfoNull,
                    Data = null
                };
            }
        }

        /// <summary>
        /// 获取权限值大于给定权限值 的部门 (ps :权限值越小权限越大)
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult GetValidDepartment(int authority,PersonnelContext context)
        {
            try
            {
                var queryAll = from d in context.Department
                    select d;
                var queryHighAuthority = (from d in queryAll
                    where d.Authority >= authority
                    select d).ToList();
                return new ReturnResult()
                {
                    Code = StatusCode.Success,
                    Data = queryHighAuthority
                };
            }
            catch (Exception e)
            {
                return new ReturnResult()
                {
                    Code = StatusCode.UnKnowError
                };
            }

            
            
        }

    }
}
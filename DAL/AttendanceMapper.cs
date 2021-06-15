using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.VisualBasic;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.DAL
{
    public static class AttendanceMapper
    {
        /// <summary>
        /// 获取职工 历史打卡记录 
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetStaffAttendance(Staff staff,PersonnelContext context)
        {
            try
            {
                var queryList = context.Attendance
                    .Include("Staff")
                    .Where(a => a.StaffId == staff.StaffId)
                    .Select(a => new
                    {
                        AttendanceId = a.AttendanceId,
                        StaffName = a.Staff.Name,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        WorkStatus = AttendanceStatus.GetWorkStatusStr(a.WorkStatus)
                    });
                
                var list =await queryList.OrderByDescending(e =>e.StartTime).ToListAsync();
                return ReturnResult.Success(list,list.Count);
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.InValidUserInfo,e);
            }
        }
        
        /// <summary>
        /// 重载的分页版本查询
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetStaffAttendance(Staff staff,int limit,int offset,PersonnelContext context)
        {
            try
            {
                var queryList = context.Attendance
                    .Include("Staff")
                    .Where(a => a.StaffId == staff.StaffId)
                    .Select(a => new
                    {
                        AttendanceId = a.AttendanceId,
                        StaffName = a.Staff.Name,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        WorkStatus = AttendanceStatus.GetWorkStatusStr(a.WorkStatus)
                    });
                
                var list =await queryList.OrderBy(a => a.StaffName).Skip(limit*offset).Take(limit).ToListAsync();
                
                return ReturnResult.Success(list,list.Count);
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.InValidUserInfo,e);
            }
        }


        /// <summary>
        /// 查询与当前用户处同一部门下的员工 且在职的员工考勤状态
        /// Enable ==true表示此员工在职
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>   
        /// <returns></returns>
        public static async Task<ReturnResult> GetAllAttendanceInfo(Staff staff,PersonnelContext context)
        {
            try
            {
                var queryList = from attendance in  context.Attendance.Include("Staff")
                    where attendance.Staff.Enable == true && attendance.Staff.DepartmentId == staff.DepartmentId
                    select new
                    {
                        AttendanceId = attendance.AttendanceId,
                        StaffName = attendance.Staff.Name,
                        StartTime = attendance.StartTime,
                        EndTime = attendance.EndTime,
                        WorkStatus = AttendanceStatus.GetWorkStatusStr(attendance.WorkStatus)
                    };
                
                var list =await queryList.ToListAsync();
                return ReturnResult.Success(list,list.Count);
            }
            catch (Exception e)
            {
                
                return ReturnResult.Fail(ResultCode.InValidUserInfo,e);
            }
        }


        /// <summary>
        /// 重载分页版本
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="offset"></param>
        /// <param name="context"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> GetAllAttendanceInfo(Staff staff,int limit,int offset,PersonnelContext context)
        {
            try
            {
                var queryList = from attendance in  context.Attendance.Include("Staff")
                    where attendance.Staff.Enable == true && attendance.Staff.DepartmentId == staff.DepartmentId
                    select new
                    {
                        AttendanceId = attendance.AttendanceId,
                        StaffName = attendance.Staff.Name,
                        StartTime = attendance.StartTime,
                        EndTime = attendance.EndTime,
                        WorkStatus = AttendanceStatus.GetWorkStatusStr(attendance.WorkStatus)
                    };
                
                var list =await queryList.OrderBy(a => a.StaffName).Skip(limit*offset).Take(limit).ToListAsync();
                return ReturnResult.Success(list,list.Count);
            }
            catch (Exception e)
            {
                
                return ReturnResult.Fail(ResultCode.InValidUserInfo,e);
            }
        }
        
        
        /// <summary>
        /// 执行上班打卡操作
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="dateTime">上班时间</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static  ReturnResult ExecGoToClock(Staff staff,DateTime dateTime,PersonnelContext context)
        {
            var parameter = new SqlParameter[]
            {
                new SqlParameter("@Time", dateTime),
                new SqlParameter("@StaffId", staff.StaffId)
            };
            try
            {

                var result = context.Attendance.FromSqlRaw("EXECUTE  [dbo].[clock] @Time , @StaffId", parameter)
                    .AsEnumerable();
                return ReturnResult.Success(result.Single());
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.DbException,e);
            }
        }

        /// <summary>
        /// 执行下班打卡操作
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="dateTime">下班时间</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult ExecGoOffClock(Attendance attendance,DateTime dateTime,PersonnelContext context)
        {
            
            var parameter = new SqlParameter[]
            {
                new SqlParameter("@Time", dateTime),
                new SqlParameter("@StaffId", attendance.StaffId)
            };
            try
            {
                var result = context.Attendance.FromSqlRaw("EXECUTE  [dbo].[clock] @Time , @StaffId", parameter)
                    .AsEnumerable();
                return ReturnResult.Success(result.Single());
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.DbException,e);
            }
            
        }

        /// <summary>
        /// 修改考勤状态
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="code"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ReturnResult UpdateWorkStatus(Attendance attendance, AttendanceStatus.WorkCode code,PersonnelContext context)
        {
            return null;
        }

        
        /// <summary>
        /// 删除考勤表中的条目
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> DeleteAttendanceById(List<Attendance> attendance, PersonnelContext context)
        {

            try
            {
                foreach (var item in attendance)
                {
                    var query =await context.Attendance.Where(a => a.AttendanceId == item.AttendanceId).SingleAsync();
                    context.Remove(query);
                }
                await context.SaveChangesAsync();

                return ReturnResult.Success();
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.InValidUserInfo, e);
            }
        }

        /// <summary>
        /// 删除某个员工的所有考勤记录
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ReturnResult> DeleteStaffAllAttendanceById(Staff staff,PersonnelContext context)
        {
            try
            {
                var queryList  = await context.Attendance.Where(a => a.StaffId == staff.StaffId).ToListAsync();

                foreach (var item in queryList)
                {
                    context.Remove(item);
                }

                await context.SaveChangesAsync();

                return ReturnResult.Success();
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.InValidUserInfo,e);
            }
        }
        
        /// <summary>
        /// 获取某个职工某个日期的考勤条目
        /// </summary>
        /// <returns></returns>
        public static async Task<ReturnResult> GetAttendanceByDate(Staff staff,DateTime dateTime,PersonnelContext context)
        {

            
            try
            {
                var queryList=await context.Attendance.Where(a =>
                    a.StaffId == staff.StaffId).ToListAsync();

                var formatQuery = from a in queryList
                    where a.StartTime.ToString("d") == dateTime.ToString("d")
                    select a;
                
                return ReturnResult.Success(formatQuery.Single());
            }
            catch (Exception e)
            {
                return ReturnResult.Fail(ResultCode.NoFund);
            }
        }
    }

}
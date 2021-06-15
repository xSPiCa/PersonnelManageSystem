using System;
using System.Collections.Generic;

namespace PersonnelManageSystem.Utils
{
    public static class AttendanceStatus
    {
            /// <summary>
            /// 状态码和出勤有关
            /// </summary>
        [Flags]
            public enum WorkCode
        {
            //上班打卡
            ClockIn = 0x4,
            //下班打卡
            ClockOut = 0x8,
            
            //迟到
            Late = 0x1,
            
            //早退
            LeaveEarly = 0x2,
            
            //正常上下班
            Normal = ClockIn|ClockOut,
            
            //上班迟到
            WorkLate = ClockIn|Late,
            
            //下班早退
            WorkLeaveEarly = Normal|LeaveEarly,
            
            //迟到早退
            
            WorkLateLeaveEarly = WorkLate|WorkLeaveEarly,
            
            //上班迟到 正常下班
            WorkLateNormal = WorkLate|Normal
            
            
        }
            
        private static readonly Dictionary<WorkCode, String> StatusStr = new Dictionary<WorkCode, string>
        {
            {WorkCode.ClockIn,"正常上班"},{WorkCode.WorkLate, "上班迟到"},
            {WorkCode.Normal, "正常上下班"}, {WorkCode.WorkLeaveEarly, "正常上班下班早退"},
            {WorkCode.WorkLateLeaveEarly, "迟到早退"},{WorkCode.WorkLateNormal,"上班迟到正常下班"}
        };
        
        public static String GetWorkStatusStr(WorkCode status)
        {
            return StatusStr[status];
        }

        /// <summary>
        /// 根据状态码判断是否已经下班打卡了
        /// </summary>
        /// <returns></returns>
        public static Boolean IsGoOffWork(WorkCode status)
        {
            if ((status & WorkCode.ClockOut) == WorkCode.ClockOut)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
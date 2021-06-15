using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Models
{
    [Table("Attendance")]
    public class Attendance
    {

        [Column("AttendanceId")] public int AttendanceId { get; set; }
        [Column("StaffId")] public int StaffId { get; set; }

        [Column("StartTime", TypeName = "smalldatetime")]
        public DateTime StartTime { get; set; }

        [Column("EndTime", TypeName = "smalldatetime")]
        public DateTime? EndTime { get; set; }

        [Range(0, 15)] [Column("WorkStatus")] public AttendanceStatus.WorkCode WorkStatus { get; set; }
        
        
        /// <summary>
        /// inverse navigation property of staff.Attendances
        /// </summary>
        public Staff Staff { get; set; }
        
        
    }
}
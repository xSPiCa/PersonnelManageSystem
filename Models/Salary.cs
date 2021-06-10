using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnelManageSystem.Models
{
    /// <summary>
    /// 工资表
    /// </summary>
    [Table("Salary")]
    public class Salary
    {
        [Key]
        [Column("SalaryId")]
        public int SalaryId { get; set; }



        /// <summary>
        /// 加班次数
        /// </summary>
        [Column("OverTime")]
        public int OverTime { get; set; }  
        
        /// <summary>
        /// 工资组成部分---用于备用
        /// </summary>
        [Column("PartWage1")]
        public int PartWage1 { get; set; }  
        [Column("PartWage2")]
        public int PartWage2 { get; set; }  
        [Column("PartWage3")]
        public int PartWage3 { get; set; }  
        
        /// <summary>
        /// 发布时间 插入时自动生成
        /// </summary>
        [Column("CreateTime", TypeName = "smalldatetime")]
        public DateTime CreateTime { set; get; } 
        
        /// <summary>
        /// 实发工资
        /// </summary>
        [Column("RealWage")]
        public int RealWage { get; set; } 

        
        [Column("StaffId")]
        public int StaffId { get; set; }
        
        /// <summary>
        /// inverse navigation property of Staff.Salaries
        /// </summary>
        public Staff Staff { get; set; }

    }
}
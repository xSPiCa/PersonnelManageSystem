using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PersonnelManageSystem.Utils;
#nullable enable
namespace PersonnelManageSystem.Models
{
    /// <summary>
    /// 模型实体关系建立使用EF框架 相关文档 
    /// https://docs.microsoft.com/en-us/ef/core/
    /// </summary>
    [Table("Staff")]
    public class Staff
    {
        /// <summary>
        /// 
        /// </summary>
        ///
        [Key]
        [Column("StaffId")]
        public int StaffId { get; set; } 

        /// <summary>
        /// 外键 部门Id
        /// </summary>
        [Column("DepartmentId")]
        public int DepartmentId { get; set; }

        
        public Department? Department { get; set; }

        [Column("Name")]
        public string Name { get; set; } 

        [Range(1,100)]
        [Column("Age")]
        public int Age { get; set; }  

        [RegularExpression(@"^[男女]$")]
        [Column("Sex")]
        public string Sex { get; set; }
        
        
        [Column("Post")]
        public Role Post { get; set; }
        
        [MaxLength(20)]
        [Column("Address")]
        public string? Address { get; set; }

        [RegularExpression(@"^1[3456789]\d{9}$",ErrorMessage = "手机格式不正确")]
        [StringLength(11)]
        [Column("Phone")]
        public string? Phone { get; set; }
        
        /// <summary>
        /// 账号是否正在启用
        /// 考虑到离职后数据依然需要保存
        /// </summary>
        [Column("Enable")]
        public bool? Enable { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Column("EntryTime"),DataType("date")]
        public DateTime EntryTime { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        [Column("LeaveTime"), DataType("date")]
        public DateTime? LeaveTime { get; set; }

        [MinLength(8,ErrorMessage = "密码长度过短")] 
        [Column("PassWord")]
        public String Password { get; set; }

        /// <summary>
        /// 实体关系 1-n
        /// 参考文档：https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=data-annotations%2Cdata-annotations-simple-key%2Csimple-key
        ///
        /// collection navigation property
        /// 
        /// 
        /// </summary>
        public ICollection<Salary> Salaries;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Attendance> Attendances;

        public ICollection<OpLog> OpLogs;

    };
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PersonnelManageSystem.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [Column("DepartmentId")]
        public int DepartmentId { get; set; }
        [Column("Name")]
        public string Name { get;  set; }

        [AllowNull]
        [Column("OfficeLocal")]
        public string OfficeLocal { get; set; }
        
        /// <summary>
        /// 部门权限 0-5
        /// 高权限部门管理可以管理低权限部门
        /// 先写着不一定实现
        /// </summary>
        [Range(0,5)]
        [Column("Authority")]
        public int Authority { get; set; }


        public ICollection<Staff> Staves { get; set; }

        
        public DepartManage DepartManage;
    } 
    
}
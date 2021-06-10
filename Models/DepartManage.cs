using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonnelManageSystem.Models
{
    /// <summary>
    /// 管理员表 即 哪个部门的管理员是哪个员工
    /// </summary>
    [Table("DepartManage")]
    public class DepartManage
    {
        [Key]
        [Column(name:"Id")]
        public int Id;

        [Column("DepartmentId")]
        public int DepartmentId;
        

        [Column("StaffId")]
        public int StaffId;


        /// <summary>
        /// 备注
        /// </summary>
        [Column("Comment")]
        public String Commnet;

    }
}
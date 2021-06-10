using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TypeCode = PersonnelManageSystem.Utils.TypeCode;

namespace PersonnelManageSystem.Models
{
    /// <summary>
    /// 记录 系统操作记录
    /// </summary>
    [Table("OpLog")]
    public class OpLog
    {
        
        [Key]
        public int OpId;

        [Column("Type",TypeName = "int")]
        public TypeCode Type;

        [MaxLength(100)]
        [Column("Content")]
        public String Content;

        public int StaffId;
        
        public Staff Staff;
    }
}
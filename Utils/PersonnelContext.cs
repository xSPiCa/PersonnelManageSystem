using Microsoft.EntityFrameworkCore;
using PersonnelManageSystem.Models;

namespace PersonnelManageSystem.Utils
{
    public class PersonnelContext:DbContext
    {
        public PersonnelContext(DbContextOptions<PersonnelContext> options)
        :base(options)
        {
            
        }

        /// <summary>
        /// 框架无法识别一些不符合命名规则的主键或是其他关系 因此需要在此处使用代码表示关系
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DepartManage>().HasKey(c =>new {c.Id});
            modelBuilder.Entity<OpLog>().HasKey(c => new {c.OpId});

            modelBuilder.Entity<Department>()
                .HasKey(d => new {d.DepartmentId});
            //建立 Department -- Staff = > 1-n
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Staves)
                .HasForeignKey(s => new {s.DepartmentId});
        }

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public DbSet<Salary> Salary { get; set; }

        public DbSet<DepartManage> DepartManage { get; set;}

        public DbSet<OpLog> OpLog { get; set; }
    }
}
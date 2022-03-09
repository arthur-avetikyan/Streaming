
using Microsoft.EntityFrameworkCore;
using System.Linq;
using KioskStream.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using KioskStream.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace KioskStream.Data
{

    public partial class KioskStreamDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {

        public KioskStreamDbContext() :
            base()
        {
            OnCreated();
        }

        public KioskStreamDbContext(DbContextOptions<KioskStreamDbContext> options) :
            base(options)
        {
            OnCreated();
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        //public virtual DbSet<Role> Roles{ get; set; }
        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<UserRole> UserRoles{ get; set; }

        public virtual DbSet<Kiosk> Kiosks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //this.RoleMapping(modelBuilder);
            //this.CustomizeRoleMapping(modelBuilder);

            //this.RolePermissionMapping(modelBuilder);
            //this.CustomizeRolePermissionMapping(modelBuilder);

            //this.UserMapping(modelBuilder);
            CustomizeUserMapping(modelBuilder);

            //this.UserRoleMapping(modelBuilder);
            //this.CustomizeUserRoleMapping(modelBuilder);

            //this.RefreshTokenMapping(modelBuilder);
            //this.DepartmentMapping(modelBuilder);
            KioskMapping(modelBuilder);
            PluginMapping(modelBuilder);
            KioskPluginMapping(modelBuilder);
            //this.SalaryMapping(modelBuilder);


            //this.FeedbackAnswerValueMapping(modelBuilder);

            //RelationshipsMapping(modelBuilder);

            CustomizeMapping(ref modelBuilder);
        }

        #region Absence Mapping

        //private void AbsenceMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Absence>().ToTable("Absence");
        //    modelBuilder.Entity<Absence>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        //    modelBuilder.Entity<Absence>().HasKey(x => x.Id);
        //    modelBuilder.Entity<Absence>().Property(x => x.Reason).IsRequired(false);
        //    modelBuilder.Entity<Absence>().Property(x => x.StartDate).IsRequired();
        //    modelBuilder.Entity<Absence>().Property(x => x.EndDate).IsRequired();
        //    modelBuilder.Entity<Absence>().HasOne(d => d.Approver).WithMany(u => u.AbsencesToApprove).HasForeignKey(d => d.ApproverId)
        //        .OnDelete(DeleteBehavior.NoAction);
        //    modelBuilder.Entity<Absence>().HasOne(d => d.Employee).WithMany(u => u.AbsencesAssigned).HasForeignKey(d => d.EmployeeId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //    modelBuilder.Entity<Absence>().HasOne(d => d.Type).WithMany(t => t.AbsencesAssigned).HasForeignKey(d => d.TypeId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity<Absence>().HasOne(d => d.State).WithMany(t => t.Absences).HasForeignKey(d => d.DomainStateId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity<Absence>().Property(x => x.CreatedDatetimeUTC)
        //        .HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
        //    modelBuilder.Entity<Absence>().Property(x => x.UpdatedDatetimeUTC)
        //        .HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        //}

        #endregion

        #region Kiosk Mapping

        private void KioskMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kiosk>().ToTable("Kiosk");
            modelBuilder.Entity<Kiosk>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Kiosk>().HasKey(x => x.Id);
            modelBuilder.Entity<Kiosk>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Kiosk>().Property(x => x.Approved);
            modelBuilder.Entity<Kiosk>().Property(x => x.KioskIdentifier).IsRequired();
            modelBuilder.Entity<Kiosk>().Property(x => x.Location).IsRequired().HasMaxLength(512);
            modelBuilder.Entity<Kiosk>().Property(x => x.TimeZone).IsRequired();
            modelBuilder.Entity<Kiosk>().Property(x => x.CreateDateTimeUtc).IsRequired().HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Kiosk>().HasMany(x => x.KioskPlugins).WithOne(x => x.Kiosk).HasForeignKey(f => f.KioskId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region Plugin Mapping

        private void PluginMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plugin>().ToTable("Plugin");
            modelBuilder.Entity<Plugin>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Plugin>().HasKey(x => x.Id);
            modelBuilder.Entity<Plugin>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Plugin>().Property(x => x.Path).IsRequired();
            modelBuilder.Entity<Plugin>().Property(x => x.CreateDateTimeUtc).IsRequired().HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Plugin>().HasMany(x => x.KioskPlugins).WithOne(x => x.Plugin).HasForeignKey(f => f.PluginId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region Kiosk Plugin Mapping

        private void KioskPluginMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KioskPlugin>().ToTable("KioskPlugin");
            modelBuilder.Entity<KioskPlugin>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<KioskPlugin>().HasKey(x => x.Id);
            modelBuilder.Entity<KioskPlugin>().Property(x => x.PluginId).IsRequired();
            modelBuilder.Entity<KioskPlugin>().Property(x => x.KioskId).IsRequired();
        }

        #endregion
        //#region Department Mapping

        //private void DepartmentMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Department>().ToTable("Department");
        //    modelBuilder.Entity<Department>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        //    modelBuilder.Entity<Department>().HasKey(x => x.Id);
        //    modelBuilder.Entity<Department>().Property(x => x.Name).IsRequired();
        //    modelBuilder.Entity<Department>().HasMany(x => x.Staff).WithOne(x => x.Department).HasForeignKey(f => f.DepartmentId)
        //        .OnDelete(DeleteBehavior.SetNull);
        //    modelBuilder.Entity<Department>().HasOne(d => d.Head).WithOne(u => u.HeadOfDepartment).HasForeignKey<Department>(d => d.HeadId)
        //        .OnDelete(DeleteBehavior.SetNull);
        //}

        //#endregion

        #region RefreshToken Mapping

        private void RefreshTokenMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<RefreshToken>().Property(x => x.Created).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<RefreshToken>().Property(x => x.Expires).IsRequired();
            modelBuilder.Entity<RefreshToken>().Property(x => x.Token).IsRequired();
        }

        #endregion

        //#region Permission Mapping

        //private void PermissionMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Permission>().ToTable(@"Permission", @"dbo");
        //    modelBuilder.Entity<Permission>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        //    modelBuilder.Entity<Permission>().Property(x => x.Name).HasColumnName(@"Name").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
        //    modelBuilder.Entity<Permission>().Property(x => x.Code).HasColumnName(@"Code").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
        //    modelBuilder.Entity<Permission>().HasKey(@"Id");
        //}

        //partial void CustomizePermissionMapping(ModelBuilder modelBuilder);

        //#endregion

        //#region Role Mapping

        //private void RoleMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Role>().ToTable(@"Role", @"dbo");
        //    modelBuilder.Entity<Role>().Property(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        //    modelBuilder.Entity<Role>().Property(x => x.Name).HasColumnName(@"Name").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
        //    modelBuilder.Entity<Role>().Property(x => x.CreatedByUserId).HasColumnName(@"CreatedByUserId").HasColumnType(@"int").ValueGeneratedNever();
        //    modelBuilder.Entity<Role>().Property(x => x.CreatedDatetimeUTC).HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
        //    modelBuilder.Entity<Role>().Property(x => x.UpdatedByUserId).HasColumnName(@"UpdatedByUserId").HasColumnType(@"int").ValueGeneratedNever();
        //    modelBuilder.Entity<Role>().Property(x => x.UpdatedDatetimeUTC).HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        //    modelBuilder.Entity<Role>().HasKey(@"Id");
        //}

        //partial void CustomizeRoleMapping(ModelBuilder modelBuilder);

        //#endregion

        //#region RolePermission Mapping

        //private void RolePermissionMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<RolePermission>().ToTable(@"RolePermission", @"dbo");
        //    modelBuilder.Entity<RolePermission>().Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().ValueGeneratedNever();
        //    modelBuilder.Entity<RolePermission>().Property(x => x.PermissionId).HasColumnName(@"PermissionId").IsRequired().ValueGeneratedNever();
        //    modelBuilder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
        //}

        //partial void CustomizeRolePermissionMapping(ModelBuilder modelBuilder);

        //#endregion

        //#region User Mapping

        //private void UserMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable(@"User", @"dbo");
        //    modelBuilder.Entity<User>().Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
        //    modelBuilder.Entity<User>().Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
        //    modelBuilder.Entity<User>().Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
        //    modelBuilder.Entity<User>().Property(x => x.CreatedDatetimeUTC).HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
        //    modelBuilder.Entity<User>().Property(x => x.UpdatedDatetimeUTC).HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        //    modelBuilder.Entity<User>().Property(x => x.HashingIterationCount).HasColumnName(@"HashingIterationCount").HasColumnType(@"int"); //ValueGeneratedOnAdd
        //    modelBuilder.Entity<User>().Property(x => x.HashingSalt).HasColumnName(@"HashingSalt").HasColumnType(@"binary(64)").ValueGeneratedNever();
        //}

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        //#endregion

        //#region UserRole Mapping

        //private void UserRoleMapping(ModelBuilder modelBuilder)
        //{
        //}

        //partial void CustomizeUserRoleMapping(ModelBuilder modelBuilder);

        //#endregion

        //#region FeedbackAnswerValue Mapping

        //private void FeedbackAnswerValueMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<FeedbackAnswerValue>().HasKey(x => new { x.AnswerValueId, x.FeedbackAnswerId });
        //}

        //#endregion

        //private void RelationshipsMapping(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Permission>().HasMany(x => x.RolePermissions).WithOne(op => op.Permission).IsRequired(true).HasForeignKey(@"PermissionId");

        //    modelBuilder.Entity<Role>().HasOne(x => x.User_CreatedByUserId).WithMany(op => op.Roles_CreatedByUserId).IsRequired(false).HasForeignKey(@"CreatedByUserId");
        //    modelBuilder.Entity<Role>().HasOne(x => x.User_UpdatedByUserId).WithMany(op => op.Roles_UpdatedByUserId).IsRequired(false).HasForeignKey(@"UpdatedByUserId");
        //    modelBuilder.Entity<Role>().HasMany(x => x.RolePermissions).WithOne(op => op.Role).IsRequired(true).HasForeignKey(x => x.RoleId);
        //    modelBuilder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(op => op.Role).IsRequired(true).HasForeignKey(@"RoleId");

        //    modelBuilder.Entity<RolePermission>().HasOne(x => x.Role).WithMany(op => op.RolePermissions).IsRequired(true).HasForeignKey(@"RoleId");
        //    modelBuilder.Entity<RolePermission>().HasOne(x => x.Permission).WithMany(op => op.RolePermissions).IsRequired(true).HasForeignKey(@"PermissionId");

        //    modelBuilder.Entity<User>().HasMany(x => x.Roles_CreatedByUserId).WithOne(op => op.User_CreatedByUserId).IsRequired(false).HasForeignKey(@"CreatedByUserId");
        //    modelBuilder.Entity<User>().HasMany(x => x.Roles_UpdatedByUserId).WithOne(op => op.User_UpdatedByUserId).IsRequired(false).HasForeignKey(@"UpdatedByUserId");
        //    modelBuilder.Entity<User>().HasMany(x => x.UserRoles).WithOne(op => op.User).IsRequired(true).HasForeignKey(@"UserId");

        //    modelBuilder.Entity<UserRole>().HasOne(x => x.User).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"UserId");
        //    modelBuilder.Entity<UserRole>().HasOne(x => x.Role).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"RoleId");

        //    modelBuilder.Entity<User>().HasMany(x => x.RefreshTokens).WithOne(o => o.User).HasForeignKey(f => f.UserId).IsRequired();
        //}

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        }

        partial void OnCreated();
    }
}

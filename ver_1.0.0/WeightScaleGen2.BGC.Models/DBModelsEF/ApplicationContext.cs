using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SyComp> SyComps { get; set; }

    public virtual DbSet<SyDepartment> SyDepartments { get; set; }

    public virtual DbSet<SyEmp> SyEmps { get; set; }

    public virtual DbSet<SyGroupMaster> SyGroupMasters { get; set; }

    public virtual DbSet<SyItemMaster> SyItemMasters { get; set; }

    public virtual DbSet<SyItemMasterRelation> SyItemMasterRelations { get; set; }

    public virtual DbSet<SyLog> SyLogs { get; set; }

    public virtual DbSet<SyMaster> SyMasters { get; set; }

    public virtual DbSet<SyMasterType> SyMasterTypes { get; set; }

    public virtual DbSet<SyMatchPlant> SyMatchPlants { get; set; }

    public virtual DbSet<SyMenu> SyMenus { get; set; }

    public virtual DbSet<SyMenuSection> SyMenuSections { get; set; }

    public virtual DbSet<SyPlant> SyPlants { get; set; }

    public virtual DbSet<SyPrefixDoc> SyPrefixDocs { get; set; }

    public virtual DbSet<SyProvince> SyProvinces { get; set; }

    public virtual DbSet<SyRole> SyRoles { get; set; }

    public virtual DbSet<SyRoleItem> SyRoleItems { get; set; }

    public virtual DbSet<SySender> SySenders { get; set; }

    public virtual DbSet<SySupplier> SySuppliers { get; set; }

    public virtual DbSet<SySystem> SySystems { get; set; }

    public virtual DbSet<SyUser> SyUsers { get; set; }

    public virtual DbSet<TsDocumentPo> TsDocumentPos { get; set; }

    public virtual DbSet<TsHistoryWeightIn> TsHistoryWeightIns { get; set; }

    public virtual DbSet<TsHistoryWeightOut> TsHistoryWeightOuts { get; set; }

    public virtual DbSet<TsIdentNumber> TsIdentNumbers { get; set; }

    public virtual DbSet<TsReturnDatum> TsReturnData { get; set; }

    public virtual DbSet<TsSenderMapping> TsSenderMappings { get; set; }

    public virtual DbSet<TsUomConversion> TsUomConversions { get; set; }

    public virtual DbSet<TsWeightIn> TsWeightIns { get; set; }

    public virtual DbSet<TsWeightOut> TsWeightOuts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER02;Database=WEIGHT_SCALE_GEN2_DEV;User ID=sa;Password=P@ssw0rd;Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SyComp>(entity =>
        {
            entity.HasKey(e => e.CompCode);

            entity.ToTable("sy_comp");

            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.AddrEnLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_en_line1");
            entity.Property(e => e.AddrEnLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_en_line2");
            entity.Property(e => e.AddrThLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_th_line1");
            entity.Property(e => e.AddrThLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_th_line2");
            entity.Property(e => e.ApproveName)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("approve_name");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.DigitSlip)
                .HasMaxLength(2)
                .HasColumnName("digit_slip");
            entity.Property(e => e.EditAfterPrint).HasColumnName("edit_after_print");
            entity.Property(e => e.HeadReport)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("head_report");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.NameEnLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_en_line1");
            entity.Property(e => e.NameEnLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_en_line2");
            entity.Property(e => e.NameThLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_th_line1");
            entity.Property(e => e.NameThLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_th_line2");
            entity.Property(e => e.Phone)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("phone");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("province_code");
        });

        modelBuilder.Entity<SyDepartment>(entity =>
        {
            entity.HasKey(e => new { e.CompCode, e.PlantCode, e.DeptCode });

            entity.ToTable("sy_department");

            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
            entity.Property(e => e.DeptCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("dept_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsAll).HasColumnName("is_all");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.NameEn)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_th");
            entity.Property(e => e.ShortCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("short_code");
        });

        modelBuilder.Entity<SyEmp>(entity =>
        {
            entity.HasKey(e => e.EmpCode);

            entity.ToTable("sy_emp");

            entity.Property(e => e.EmpCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("emp_code");
            entity.Property(e => e.AddrLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_line1");
            entity.Property(e => e.AddrLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_line2");
            entity.Property(e => e.CompCode)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.DeptCode)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("dept_code");
            entity.Property(e => e.DeptTemp)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("dept_temp");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("email");
            entity.Property(e => e.ImgByte).HasColumnName("img_byte");
            entity.Property(e => e.ImgName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("img_name");
            entity.Property(e => e.ImgType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("img_type");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name");
            entity.Property(e => e.PayType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("pay_type");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("phone");
            entity.Property(e => e.PlantCode)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
            entity.Property(e => e.PlantTemp)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_temp");
            entity.Property(e => e.WorkStartDate).HasColumnName("work_start_date");
        });

        modelBuilder.Entity<SyGroupMaster>(entity =>
        {
            entity.HasKey(e => e.GroupCode).HasName("PK_GroupMaster");

            entity.ToTable("sy_group_master");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("group_code");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .HasColumnName("group_name");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
        });

        modelBuilder.Entity<SyItemMaster>(entity =>
        {
            entity.HasKey(e => e.ItemCode);

            entity.ToTable("sy_item_master");

            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("item_code");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ItemGroup)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("item_group");
            entity.Property(e => e.ItemName)
                .HasMaxLength(50)
                .HasColumnName("item_name");
            entity.Property(e => e.ItemShot).HasColumnName("item_shot");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<SyItemMasterRelation>(entity =>
        {
            entity.ToTable("sy_item_master_relation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.Gravity)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("gravity");
            entity.Property(e => e.Humidity)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("humidity");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("item_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SupplierCode).HasColumnName("supplier_code");
        });

        modelBuilder.Entity<SyLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("sy_log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.LogAdditionalInfo)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_additional_Info");
            entity.Property(e => e.LogCallerFilePath)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_caller_file_path");
            entity.Property(e => e.LogCallerMemberName)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_caller_member_name");
            entity.Property(e => e.LogDate)
                .HasColumnType("datetime")
                .HasColumnName("log_date");
            entity.Property(e => e.LogErrorCode).HasColumnName("log_error_code");
            entity.Property(e => e.LogExceptionMessage)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_exception_message");
            entity.Property(e => e.LogInnerException)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_inner_exception");
            entity.Property(e => e.LogIpAddress)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_ip_address");
            entity.Property(e => e.LogLevel)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_level");
            entity.Property(e => e.LogMessage)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_message");
            entity.Property(e => e.LogSourceLineNumber)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_source_line_number");
            entity.Property(e => e.LogStackTrace)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_stack_trace");
            entity.Property(e => e.LogType)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_type");
            entity.Property(e => e.LogUser)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("log_user");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
        });

        modelBuilder.Entity<SyMaster>(entity =>
        {
            entity.HasKey(e => new { e.CompCode, e.PlantCode, e.MasterType, e.MasterCode });

            entity.ToTable("sy_master");

            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
            entity.Property(e => e.MasterType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_type");
            entity.Property(e => e.MasterCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_code");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsAll).HasColumnName("is_all");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.MasterDescEn)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_desc_en");
            entity.Property(e => e.MasterDescTh)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_desc_th");
            entity.Property(e => e.MasterValue1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_value1");
            entity.Property(e => e.MasterValue2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_value2");
            entity.Property(e => e.MasterValue3)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_value3");

            entity.HasOne(d => d.MasterTypeNavigation).WithMany(p => p.SyMasters)
                .HasForeignKey(d => d.MasterType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sy_master_master_type");
        });

        modelBuilder.Entity<SyMasterType>(entity =>
        {
            entity.HasKey(e => e.MasterType);

            entity.ToTable("sy_master_type");

            entity.Property(e => e.MasterType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_type");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.IsAdd).HasColumnName("is_add");
            entity.Property(e => e.IsNotDel).HasColumnName("is_not_del");
            entity.Property(e => e.IsNotEdit).HasColumnName("is_not_edit");
            entity.Property(e => e.MasterTypeDesc)
                .IsRequired()
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("master_type_desc");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
        });

        modelBuilder.Entity<SyMatchPlant>(entity =>
        {
            entity.ToTable("sy_match_plant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.MatchCompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("match_comp_code");
            entity.Property(e => e.MatchPlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("match_plant_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
        });

        modelBuilder.Entity<SyMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK_sy_menu_1");

            entity.ToTable("sy_menu");

            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("display_name");
            entity.Property(e => e.Icon)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("icon");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ListNo).HasColumnName("list_no");
            entity.Property(e => e.MenuDefinition).HasColumnName("menu_definition");
            entity.Property(e => e.MenuLevel).HasColumnName("menu_level");
            entity.Property(e => e.MenuName)
                .IsRequired()
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("menu_name");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.ParentMenuId).HasColumnName("parent_menu_id");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("url");
            entity.Property(e => e.UrlController)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("url_controller");
        });

        modelBuilder.Entity<SyMenuSection>(entity =>
        {
            entity.HasKey(e => e.MenuSectionId);

            entity.ToTable("sy_menu_section");

            entity.Property(e => e.MenuSectionId).HasColumnName("menu_section_id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ListNo).HasColumnName("list_no");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.MenuSectionName)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("menu_section_name");
            entity.Property(e => e.MenuSectionNameDisplay)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("menu_section_name_display");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
        });

        modelBuilder.Entity<SyPlant>(entity =>
        {
            entity.HasKey(e => e.PlantCode);

            entity.ToTable("sy_plant");

            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
            entity.Property(e => e.AddrEnLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_en_line1");
            entity.Property(e => e.AddrEnLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_en_line2");
            entity.Property(e => e.AddrThLine1)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_th_line1");
            entity.Property(e => e.AddrThLine2)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("addr_th_line2");
            entity.Property(e => e.CompCode)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.HeadReport)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("head_report");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.NameEn)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_th");
            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("province_code");
            entity.Property(e => e.ReportType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("report_type");
            entity.Property(e => e.SerialPort)
                .HasMaxLength(5)
                .HasColumnName("serial_port");
            entity.Property(e => e.ShortCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("short_code");
            entity.Property(e => e.ShortPlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("short_plant_code");
        });

        modelBuilder.Entity<SyPrefixDoc>(entity =>
        {
            entity.ToTable("sy_prefix_doc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("comp_code");
            entity.Property(e => e.Digit)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("digit");
            entity.Property(e => e.DocType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("doc_type");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("plant_code");
            entity.Property(e => e.PrefixMonth).HasColumnName("prefix_month");
            entity.Property(e => e.PrefixPattern)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("prefix_pattern");
            entity.Property(e => e.PrefixYear).HasColumnName("prefix_year");
            entity.Property(e => e.SeqCurrent).HasColumnName("seq_current");
            entity.Property(e => e.SeqNext).HasColumnName("seq_next");
        });

        modelBuilder.Entity<SyProvince>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("sy_province");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("code");
            entity.Property(e => e.NameEn)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name_th");
        });

        modelBuilder.Entity<SyRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("sy_role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsSuperRole).HasColumnName("is_super_role");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.RoleDesc)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("role_desc");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<SyRoleItem>(entity =>
        {
            entity.HasKey(e => e.RoleItemId);

            entity.ToTable("sy_role_item");

            entity.Property(e => e.RoleItemId).HasColumnName("role_item_id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsAction).HasColumnName("is_action");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsDisplay).HasColumnName("is_display");
            entity.Property(e => e.MenuSectionId).HasColumnName("menu_section_id");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.MenuSection).WithMany(p => p.SyRoleItems)
                .HasForeignKey(d => d.MenuSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sy_role_item_sy_menu_section");

            entity.HasOne(d => d.Role).WithMany(p => p.SyRoleItems)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sy_role_item_sy_role");
        });

        modelBuilder.Entity<SySender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sy_sender_1");

            entity.ToTable("sy_sender");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.FlagDelete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("flag_delete");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.SenderName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sender_name");
        });

        modelBuilder.Entity<SySupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierCode);

            entity.ToTable("sy_supplier");

            entity.Property(e => e.SupplierCode)
                .ValueGeneratedNever()
                .HasColumnName("supplier_code");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<SySystem>(entity =>
        {
            entity.HasKey(e => new { e.SysType, e.SysCode });

            entity.ToTable("sy_system");

            entity.Property(e => e.SysType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sys_type");
            entity.Property(e => e.SysCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sys_code");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.SysDesc)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sys_desc");
            entity.Property(e => e.SysValue)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sys_value");
        });

        modelBuilder.Entity<SyUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("sy_user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("emp_code");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("password");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.SyUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sy_user_sy_role");
        });

        modelBuilder.Entity<TsDocumentPo>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseNumber, e.NumOfRec }).HasName("PK_document_po");

            entity.ToTable("ts_document_po");

            entity.Property(e => e.PurchaseNumber)
                .HasMaxLength(10)
                .HasColumnName("purchase_number");
            entity.Property(e => e.NumOfRec)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("num_of_rec");
            entity.Property(e => e.Allowance)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("allowance");
            entity.Property(e => e.CompanyCode)
                .HasMaxLength(4)
                .HasColumnName("company_code");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(12)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.CreatedTime)
                .HasPrecision(6)
                .HasColumnName("created_time");
            entity.Property(e => e.DlvComplete)
                .HasMaxLength(1)
                .HasColumnName("dlv_complete");
            entity.Property(e => e.GoodReceived)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("good_received");
            entity.Property(e => e.MaterialCode)
                .HasMaxLength(40)
                .HasColumnName("material_code");
            entity.Property(e => e.MaterialDesc)
                .HasMaxLength(40)
                .HasColumnName("material_desc");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(12)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.ModifiedTime)
                .HasPrecision(6)
                .HasColumnName("modified_time");
            entity.Property(e => e.OrderQty)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("order_qty");
            entity.Property(e => e.PendingQty)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("pending_qty");
            entity.Property(e => e.PendingQtyAll)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("pending_qty_all");
            entity.Property(e => e.Plant)
                .HasMaxLength(4)
                .HasColumnName("plant");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .HasColumnName("status");
            entity.Property(e => e.StorageLoc)
                .HasMaxLength(4)
                .HasColumnName("storage_loc");
            entity.Property(e => e.Uom)
                .HasMaxLength(3)
                .HasColumnName("uom");
            entity.Property(e => e.UomIn)
                .HasMaxLength(3)
                .HasColumnName("uom_in");
            entity.Property(e => e.VenderCode)
                .HasMaxLength(10)
                .HasColumnName("vender_code");
            entity.Property(e => e.VenderName)
                .HasMaxLength(30)
                .HasColumnName("vender_name");
        });

        modelBuilder.Entity<TsHistoryWeightIn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_history_weight_in");

            entity.ToTable("ts_history_weight_in");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CarLicense)
                .HasMaxLength(10)
                .HasColumnName("car_license");
            entity.Property(e => e.CarType)
                .HasMaxLength(50)
                .HasColumnName("car_type");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.DocSend)
                .HasMaxLength(20)
                .HasColumnName("doc_send");
            entity.Property(e => e.DocStart)
                .HasColumnType("datetime")
                .HasColumnName("doc_start");
            entity.Property(e => e.DocStop)
                .HasColumnType("datetime")
                .HasColumnName("doc_stop");
            entity.Property(e => e.DocTypePo)
                .HasMaxLength(10)
                .HasColumnName("doc_type_po");
            entity.Property(e => e.DocumentPo)
                .HasMaxLength(20)
                .HasColumnName("document_po");
            entity.Property(e => e.DocumentRef)
                .HasMaxLength(50)
                .HasColumnName("document_ref");
            entity.Property(e => e.Edi).HasColumnName("edi");
            entity.Property(e => e.EdiSand)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("edi_sand");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("item_code");
            entity.Property(e => e.ItemName)
                .HasMaxLength(60)
                .HasColumnName("item_name");
            entity.Property(e => e.LineNumber)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("line_number");
            entity.Property(e => e.MaintenanceNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maintenance_no");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Reprint).HasColumnName("reprint");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SupplierCode).HasColumnName("supplier_code");
            entity.Property(e => e.UserEdit1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_1");
            entity.Property(e => e.UserEdit2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_2");
            entity.Property(e => e.UserEdit3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_3");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.WeightIn)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_in");
            entity.Property(e => e.WeightInNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
            entity.Property(e => e.WeightInType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("weight_in_type");
        });

        modelBuilder.Entity<TsHistoryWeightOut>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_history_weight_out");

            entity.ToTable("ts_history_weight_out");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("api_bg");
            entity.Property(e => e.ApiSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("api_supplier");
            entity.Property(e => e.BaseUnit)
                .HasMaxLength(3)
                .HasColumnName("base_unit");
            entity.Property(e => e.BeforeWeightOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("before_weight_out");
            entity.Property(e => e.CarLicense)
                .HasMaxLength(10)
                .HasColumnName("car_license");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Edi).HasColumnName("edi");
            entity.Property(e => e.EdiSend)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("edi_send");
            entity.Property(e => e.GrossUom)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("gross_uom");
            entity.Property(e => e.MaintenanceNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maintenance_no");
            entity.Property(e => e.NetUom)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("net_uom");
            entity.Property(e => e.PercentHumidityDiff)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_diff");
            entity.Property(e => e.PercentHumidityOk)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_ok");
            entity.Property(e => e.PercentHumidityOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_out");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.QtyBag)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("qty_bag");
            entity.Property(e => e.QtyPallet)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("qty_pallet");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Reprint).HasColumnName("reprint");
            entity.Property(e => e.SgBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("sg_bg");
            entity.Property(e => e.SgSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("sg_supplier");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TempBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("temp_bg");
            entity.Property(e => e.TempSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("temp_supplier");
            entity.Property(e => e.TotalWeightBag)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("total_weight_bag");
            entity.Property(e => e.TotalWeightPallet)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("total_weight_pallet");
            entity.Property(e => e.UnitReceive)
                .HasMaxLength(3)
                .HasColumnName("unit_receive");
            entity.Property(e => e.UserEdit1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_1");
            entity.Property(e => e.UserEdit2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_2");
            entity.Property(e => e.UserEdit3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_3");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.VolumeBySupplier)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("volume_by_supplier");
            entity.Property(e => e.WeightBag)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_bag");
            entity.Property(e => e.WeightBySupplier)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_by_supplier");
            entity.Property(e => e.WeightInNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
            entity.Property(e => e.WeightOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_out");
            entity.Property(e => e.WeightOutNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_out_no");
            entity.Property(e => e.WeightOutType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("weight_out_type");
            entity.Property(e => e.WeightPallet)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_pallet");
            entity.Property(e => e.WeightReceive)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_receive");
        });

        modelBuilder.Entity<TsIdentNumber>(entity =>
        {
            entity.HasKey(e => new { e.Company, e.Year, e.Month, e.Type }).HasName("PK_Ident_Number");

            entity.ToTable("ts_ident_number");

            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Year)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("year");
            entity.Property(e => e.Month)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("month");
            entity.Property(e => e.Type)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.IdenNumber).HasColumnName("iden_number");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
        });

        modelBuilder.Entity<TsReturnDatum>(entity =>
        {
            entity.HasKey(e => new { e.WeightOutNo, e.Sequence }).HasName("PK_sy_return_data");

            entity.ToTable("ts_return_data");

            entity.Property(e => e.WeightOutNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_out_no");
            entity.Property(e => e.Sequence)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("sequence");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.DocDate).HasColumnName("doc_date");
            entity.Property(e => e.DocSend)
                .HasMaxLength(20)
                .HasColumnName("doc_send");
            entity.Property(e => e.DocStart)
                .HasColumnType("datetime")
                .HasColumnName("doc_start");
            entity.Property(e => e.DocStop)
                .HasColumnType("datetime")
                .HasColumnName("doc_stop");
            entity.Property(e => e.DocumentYear)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("document_year");
            entity.Property(e => e.GoodMovement)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("good_movement");
            entity.Property(e => e.GrType)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("gr_type");
            entity.Property(e => e.ItemText)
                .HasMaxLength(50)
                .HasColumnName("item_text");
            entity.Property(e => e.Material)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("material");
            entity.Property(e => e.MaterialDocument)
                .HasMaxLength(10)
                .HasColumnName("material_document");
            entity.Property(e => e.Message)
                .HasMaxLength(220)
                .HasColumnName("message");
            entity.Property(e => e.MessageType)
                .HasMaxLength(1)
                .HasColumnName("message_type");
            entity.Property(e => e.Plant)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("plant");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.PoLineNumber)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("po_line_number");
            entity.Property(e => e.PoNumber)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("po_number");
            entity.Property(e => e.PostDate).HasColumnName("post_date");
            entity.Property(e => e.RefDoc)
                .HasMaxLength(16)
                .HasColumnName("ref_doc");
            entity.Property(e => e.SendData)
                .HasMaxLength(1)
                .HasColumnName("send_data");
            entity.Property(e => e.Sloc)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("sloc");
            entity.Property(e => e.StockType)
                .IsRequired()
                .HasMaxLength(1)
                .HasColumnName("stock_type");
            entity.Property(e => e.TruckNo)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("truck_no");
            entity.Property(e => e.WeightIn)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("weight_in");
            entity.Property(e => e.WeightInNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
            entity.Property(e => e.WeightOut)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("weight_out");
            entity.Property(e => e.WeightRec)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("weight_rec");
            entity.Property(e => e.WeightReject)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("weight_reject");
            entity.Property(e => e.WeightUnit)
                .HasMaxLength(3)
                .HasColumnName("weight_unit");
            entity.Property(e => e.WeightVendor)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("weight_vendor");
        });

        modelBuilder.Entity<TsSenderMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sy_sender_mapping");

            entity.ToTable("ts_sender_mapping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.WeightInNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
        });

        modelBuilder.Entity<TsUomConversion>(entity =>
        {
            entity.HasKey(e => new { e.MaterialCode, e.AlterUom, e.BaseUom }).HasName("PK_UOM_Conversion");

            entity.ToTable("ts_uom_conversion");

            entity.Property(e => e.MaterialCode)
                .HasMaxLength(40)
                .HasColumnName("material_code");
            entity.Property(e => e.AlterUom)
                .HasMaxLength(3)
                .HasColumnName("alter_uom");
            entity.Property(e => e.BaseUom)
                .HasMaxLength(3)
                .HasColumnName("base_uom");
            entity.Property(e => e.AlterUomIn)
                .HasMaxLength(3)
                .HasColumnName("alter_uom_in");
            entity.Property(e => e.BaseUomIn)
                .HasMaxLength(3)
                .HasColumnName("base_uom_in");
            entity.Property(e => e.CompCode)
                .HasMaxLength(50)
                .HasColumnName("comp_code");
            entity.Property(e => e.ConvWeightD)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("conv_weight_d");
            entity.Property(e => e.ConvWeightN)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("conv_weight_n");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(12)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
            entity.Property(e => e.CreatedTime)
                .HasPrecision(6)
                .HasColumnName("created_time");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("gross_weight");
            entity.Property(e => e.NetWeight)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("net_weight");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(12)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UpdatedTime)
                .HasPrecision(6)
                .HasColumnName("updated_time");
            entity.Property(e => e.WeightUnit)
                .HasMaxLength(3)
                .HasColumnName("weight_unit");
        });

        modelBuilder.Entity<TsWeightIn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_weight_In");

            entity.ToTable("ts_weight_in");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CarLicense)
                .HasMaxLength(10)
                .HasColumnName("car_license");
            entity.Property(e => e.CarType)
                .HasMaxLength(50)
                .HasColumnName("car_type");
            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.DocSend)
                .HasMaxLength(20)
                .HasColumnName("doc_send");
            entity.Property(e => e.DocStart)
                .HasColumnType("datetime")
                .HasColumnName("doc_start");
            entity.Property(e => e.DocStop)
                .HasColumnType("datetime")
                .HasColumnName("doc_stop");
            entity.Property(e => e.DocTypePo)
                .HasMaxLength(10)
                .HasColumnName("doc_type_po");
            entity.Property(e => e.DocumentPo)
                .HasMaxLength(20)
                .HasColumnName("document_po");
            entity.Property(e => e.DocumentRef)
                .HasMaxLength(50)
                .HasColumnName("document_ref");
            entity.Property(e => e.Edi).HasColumnName("edi");
            entity.Property(e => e.EdiSand).HasColumnName("edi_sand");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("item_code");
            entity.Property(e => e.ItemName)
                .HasMaxLength(60)
                .HasColumnName("item_name");
            entity.Property(e => e.LineNumber)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("line_number");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Reprint).HasColumnName("reprint");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SupplierCode).HasColumnName("supplier_code");
            entity.Property(e => e.UserEdit1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_1");
            entity.Property(e => e.UserEdit2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_2");
            entity.Property(e => e.UserEdit3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_3");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.WeightIn)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_in");
            entity.Property(e => e.WeightInNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
            entity.Property(e => e.WeightInType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("weight_in_type");
        });

        modelBuilder.Entity<TsWeightOut>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_weight_out");

            entity.ToTable("ts_weight_out");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("api_bg");
            entity.Property(e => e.ApiSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("api_supplier");
            entity.Property(e => e.BaseUnit)
                .HasMaxLength(3)
                .HasColumnName("base_unit");
            entity.Property(e => e.BeforeWeightOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("before_weight_out");
            entity.Property(e => e.CarLicense)
                .HasMaxLength(10)
                .HasColumnName("car_license");
            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.GrossUom)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("gross_uom");
            entity.Property(e => e.NetUom)
                .HasColumnType("decimal(13, 3)")
                .HasColumnName("net_uom");
            entity.Property(e => e.PercentHumidityDiff)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_diff");
            entity.Property(e => e.PercentHumidityOk)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_ok");
            entity.Property(e => e.PercentHumidityOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("percent_humidity_out");
            entity.Property(e => e.PlantCode)
                .HasMaxLength(50)
                .HasColumnName("plant_code");
            entity.Property(e => e.QtyBag)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("qty_bag");
            entity.Property(e => e.QtyPallet)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("qty_pallet");
            entity.Property(e => e.Remark1)
                .HasMaxLength(50)
                .HasColumnName("remark_1");
            entity.Property(e => e.Remark2)
                .HasMaxLength(50)
                .HasColumnName("remark_2");
            entity.Property(e => e.Reprint).HasColumnName("reprint");
            entity.Property(e => e.SgBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("sg_bg");
            entity.Property(e => e.SgSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("sg_supplier");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TempBg)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("temp_bg");
            entity.Property(e => e.TempSupplier)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("temp_supplier");
            entity.Property(e => e.TotalWeightBag)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("total_weight_bag");
            entity.Property(e => e.TotalWeightPallet)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("total_weight_pallet");
            entity.Property(e => e.UnitReceive)
                .HasMaxLength(3)
                .HasColumnName("unit_receive");
            entity.Property(e => e.UserEdit1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_1");
            entity.Property(e => e.UserEdit2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_2");
            entity.Property(e => e.UserEdit3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_edit_3");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.VolumeBySupplier)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("volume_by_supplier");
            entity.Property(e => e.WeightBag)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_bag");
            entity.Property(e => e.WeightBySupplier)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_by_supplier");
            entity.Property(e => e.WeightInNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_in_no");
            entity.Property(e => e.WeightOut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_out");
            entity.Property(e => e.WeightOutNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("weight_out_no");
            entity.Property(e => e.WeightOutType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("weight_out_type");
            entity.Property(e => e.WeightPallet)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_pallet");
            entity.Property(e => e.WeightReceive)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("weight_receive");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

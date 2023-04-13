using Microsoft.EntityFrameworkCore;

namespace ZhuZhuYunAPI.Models
{
    public partial class PanoUserContext : DbContext
    {
        public PanoUserContext()
        {
        }

        public PanoUserContext(DbContextOptions<PanoUserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PanoUser> PanoUser { get; set; } = null!;
        public virtual DbSet<PanoTempUser> PanoTempUser { get; set; } = null!;
        public virtual DbSet<BlackRecord> BlackRecord { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfo { get; set; } = null!;
        public virtual DbSet<LoginData> LoginData { get; set; } = null!;
        public virtual DbSet<PanoDownLoadRecord> PanoDownLoadRecord { get; set; } = null!;
        public virtual DbSet<PanoLoginRecord> PanoLoginRecord { get; set; } = null!;
        public virtual DbSet<PanoRegisterRecord> PanoRegisterRecord { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            //public int Id { get; set; }
            //public string UserName { get; set; } = null!;
            //public string PassWord { get; set; } = null!;
            //public string DepartmentName { get; set; } = null!;


            modelBuilder.Entity<PanoUser>(entity =>
            {
                entity.ToTable("PanoUser_Table");
                //entity.Property(e => e.Swith)
                //    .HasMaxLength(200)
                //    .HasColumnName("swith");
            });        
            modelBuilder.Entity<PanoTempUser>(entity =>
            {
                entity.ToTable("PanoTempUser_Table");
                //entity.Property(e => e.Swith)
                //    .HasMaxLength(200)
                //    .HasColumnName("swith");
            });
            modelBuilder.Entity<LoginData>(entity =>
            {
                entity.ToTable("LoginData_Table");
                //entity.Property(e => e.Swith)
                //    .HasMaxLength(200)
                //    .HasColumnName("swith");
            });
            modelBuilder.Entity<PanoDownLoadRecord>(entity =>
            {
                entity.ToTable("PanoDownLoadRecord_Table");
                //entity.Property(e => e.Swith)
                //    .HasMaxLength(200)
                //    .HasColumnName("swith");
            });
            modelBuilder.Entity<PanoLoginRecord>(entity =>
            {
                entity.ToTable("PanoLoginRecord_Table");
                //entity.Property(e => e.Swith)
                //    .HasMaxLength(200)
                //    .HasColumnName("swith");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }


}

using Microsoft.EntityFrameworkCore;

namespace TiendaUniformesAPI.Models;

public partial class TiendaUniformesContext : DbContext
{
    public TiendaUniformesContext() { }

    public TiendaUniformesContext(DbContextOptions<TiendaUniformesContext> options) : base(options) { }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Garment> Garments { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=TiendaUniformes;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdC).HasName("PK__Customer__DC501A2DBA29A538");

            entity.ToTable("Customer");

            entity.Property(e => e.IdC).HasColumnName("idC");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Garment>(entity =>
        {
            entity.HasKey(e => e.IdG).HasName("PK__Garment__DC501A2961AC031E");

            entity.ToTable("Garment");

            entity.Property(e => e.IdG).HasColumnName("idG");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.Desctiption)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("desctiption");
            entity.Property(e => e.IdS).HasColumnName("idS");
            entity.Property(e => e.IdSc).HasColumnName("idSc");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__Inventor__DC501A2792AE0468");

            entity.ToTable("Inventory");

            entity.Property(e => e.IdI).HasColumnName("idI");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.IdG).HasColumnName("idG");
            entity.Property(e => e.IdSc).HasColumnName("idSc");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Quantitaty).HasColumnName("quantitaty");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdO).HasName("PK__Order__DC501A21C47B9029");

            entity.ToTable("Order");

            entity.Property(e => e.IdO).HasColumnName("idO");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.DateOrder).HasColumnName("dateOrder");
            entity.Property(e => e.DeadLine).HasColumnName("deadLine");
            entity.Property(e => e.IdC).HasColumnName("idC");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("totalPrice");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.IdOd).HasName("PK__OrderDet__9DB850CDAE8357E4");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.IdOd).HasColumnName("idOd");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.IdG).HasColumnName("idG");
            entity.Property(e => e.IdO).HasColumnName("idO");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Quantitaty).HasColumnName("quantitaty");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.IdSc).HasName("PK__School__9DB83077A3F3A8E7");

            entity.ToTable("School");

            entity.Property(e => e.IdSc).HasColumnName("idSc");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.IdS).HasName("PK__Size__DC501A1DC337A1B8");

            entity.ToTable("Size");

            entity.Property(e => e.IdS).HasColumnName("idS");
            entity.Property(e => e.CreateDate).HasColumnName("createDate");
            entity.Property(e => e.CreateUser).HasColumnName("createUser");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifyDate).HasColumnName("modifyDate");
            entity.Property(e => e.ModifyUser).HasColumnName("modifyUser");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Size1).HasColumnName("size");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdU).HasName("PK__User__DC501A1B7B7F2445");

            entity.ToTable("Account");

            entity.Property(e => e.IdU).HasColumnName("idU");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Pass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

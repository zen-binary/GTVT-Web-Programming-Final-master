using Microsoft.EntityFrameworkCore;
using Project_Final.Models;
using Project_Final.Models.Entity;
using Project_Final.Utils;
using System.Security.Claims;
using System.Text.Json;

namespace Project_Final.Context;

public partial class DBContext : DbContext
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        Converters = { new ProductJsonConverter() },
        WriteIndented = true
    };

    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost,21433;Initial Catalog=utc_shop;Persist Security Info=True;User ID=sa;Password=Passw0rd;TrustServerCertificate=True");

    public override int SaveChanges()
    {
        var entries = ChangeTracker
        .Entries()
        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        var httpContextAccessor = HttpContextHelper.HttpContextAccessor;
        var createdBy = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "system";
        var createdAt = DateTime.Now;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is User user)
                {
                    user.CreatedBy = createdBy;
                    user.CreatedAt = createdAt;
                    user.LastModifiedBy = createdBy;
                    user.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Category category)
                {
                    category.CreatedBy = createdBy;
                    category.CreatedAt = createdAt;
                    category.LastModifiedBy = createdBy;
                    category.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Product product)
                {
                    product.CreatedBy = createdBy;
                    product.CreatedAt = createdAt;
                    product.LastModifiedBy = createdBy;
                    product.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Order order)
                {
                    order.CreatedBy = createdBy;
                    order.CreatedAt = createdAt;
                    order.LastModifiedBy = createdBy;
                    order.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is OrderDetail orderDetail)
                {
                    orderDetail.CreatedBy = createdBy;
                    orderDetail.CreatedAt = createdAt;
                    orderDetail.LastModifiedBy = createdBy;
                    orderDetail.LastModifiedAt = createdAt;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is User user)
                {
                    user.LastModifiedBy = createdBy;
                    user.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Category category)
                {
                    category.LastModifiedBy = createdBy;
                    category.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Product product)
                {
                    product.LastModifiedBy = createdBy;
                    product.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is Order order)
                {
                    order.LastModifiedBy = createdBy;
                    order.LastModifiedAt = createdAt;
                }
                else if (entry.Entity is OrderDetail orderDetail)
                {
                    orderDetail.LastModifiedBy = createdBy;
                    orderDetail.LastModifiedAt = createdAt;
                }
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Key config
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Details)
            .WithOne(od => od.Order)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
        //Product property
        modelBuilder.Entity<Product>()
           .Property(p => p.Status)
           .HasConversion(
            v => v.ToString(),
            v => (Status)Enum.Parse(typeof(Status), v));
       
        modelBuilder.Entity<Category>()
           .Property(p => p.Status)
           .HasConversion(
            v => v.ToString(),
            v => (Status)Enum.Parse(typeof(Status), v));

        //Order property
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion(
            v => v.ToString(),
            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v))
            .HasColumnType("varchar(20)");

        // User property
        modelBuilder.Entity<User>()
            .Property(u => u.Roles)
            .HasConversion(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = true }),
            v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            );

        // CreatedAt
        modelBuilder.Entity<User>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Category>()
           .Property(b => b.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Product>()
           .Property(b => b.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Order>()
           .Property(b => b.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<OrderDetail>()
           .Property(b => b.CreatedAt)
           .HasDefaultValueSql("GETUTCDATE()");


        // UpdatedAt
        modelBuilder.Entity<User>()
            .Property(b => b.LastModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Category>()
            .Property(b => b.LastModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Product>()
            .Property(b => b.LastModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Order>()
            .Property(b => b.LastModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<OrderDetail>()
            .Property(b => b.LastModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}

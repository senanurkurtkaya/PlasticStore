using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentMedia> ContentMedias { get; set; }
        public DbSet<CustomerRequest> CustomerRequests { get; set; }
        public DbSet<ProductMetaTags> ProductMetaTags { get; set; }
        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; }
        public DbSet<FaqCategory> FaqCategories { get; set; }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            //ONR++ 29012025/16:09
            modelBuilder.Entity<Content>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<ContentMedia>()
                .HasKey(cm => cm.Id);

            modelBuilder.Entity<ContentMedia>()
                .HasOne(cm => cm.Content)
                .WithMany(c => c.Medias)
                .HasForeignKey(cm => cm.ContentId)
                .OnDelete(DeleteBehavior.Cascade);         

            modelBuilder.Entity<Content>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Content>()
                .Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Content>()
                .HasMany(c => c.Medias)
                .WithOne(m => m.Content)
                .HasForeignKey(m => m.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
    .HasMany(u => u.CustomerRequests)
    .WithOne(r => r.Customer)
    .HasForeignKey(r => r.UserId)
    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CustomerRequest>()
   .HasOne(c => c.Category)
   .WithMany(c => c.CustomerRequests)
   .HasForeignKey(c => c.CategoryId)
   .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomerRequest>()
        .HasOne(r => r.Customer)
        .WithMany(u => u.CustomerRequests)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<CustomerRequest>()
                .HasOne(r => r.AssignedUser)
                .WithMany(u => u.AssignedRequests)
                .HasForeignKey(r => r.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            //--ONR 29012025/16:09
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Tabloların isimlendirilmesi
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");

            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            var adminRoleId = "admin-role-id";
            var userRoleId = "user-role-id";
            var contentManagerId = "content-manager-id";
            var customerServiceId = "customer-service-id";

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = userRoleId, Name = "User", NormalizedName = "USER" },
                new Role { Id = contentManagerId, Name = "İçerik Yöneticisi", NormalizedName = "İÇERIK YÖNETICISI " },
                new Role { Id = customerServiceId, Name = "Müşteri Hizmetleri", NormalizedName = "MÜŞTERI HIZMETLERI " }
            );

            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "System",
                LastName = "Administrator",
                IsAdmin = true,
                RoleName = "Admin",
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User",
                NormalizedUserName = "USER",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "Regular",
                LastName = "User",
                IsAdmin = false,
                RoleName = "User"
            };
            user.PasswordHash = hasher.HashPassword(user, "User123!");



            modelBuilder.Entity<User>().HasData(admin, user);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string> { UserId = admin.Id, RoleId = adminRoleId},
               new IdentityUserRole<string> { UserId = user.Id, RoleId = userRoleId }
            );
        }
    }
}

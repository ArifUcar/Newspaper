using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Base;
using UdemyCarBook.Domain.Entities;
using System.Reflection;

namespace UdemyCarBook.Persistance.Context
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {
        }

        public DbSet<BaseHistory> Histories { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Authors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("char(36)")
                    .HasCharSet("ascii")
                    .HasCollation("ascii_general_ci");
            });
            modelBuilder.Entity<BaseHistory>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("char(36)")
                    .HasCharSet("ascii")
                    .HasCollation("ascii_general_ci");

                entity.Property(e => e.EntityId)
                    .HasColumnType("char(36)")
                    .HasCharSet("ascii")
                    .HasCollation("ascii_general_ci");
            });
            modelBuilder.Entity<News>()
                .HasMany(n => n.Tags)
                .WithMany(t => t.News)
                .UsingEntity(j => j.ToTable("NewsTag"));

            modelBuilder.Entity<News>()
                .HasOne(n => n.CreatedByUser)
                .WithMany(u => u.CreatedNews)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<News>()
                .HasOne(n => n.UpdatedByUser)
                .WithMany(u => u.UpdatedNews)
                .HasForeignKey(n => n.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<News>()
                .HasOne(n => n.LastModifiedByUser)
                .WithMany(u => u.LastModifiedNews)
                .HasForeignKey(n => n.LastModifiedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasOne(r => r.LastModifiedByUser)
                    .WithMany(u => u.LastModifiedRoles)
                    .HasForeignKey(r => r.LastModifiedByUserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.UpdatedByUser)
                    .WithMany(u => u.UpdatedRoles)
                    .HasForeignKey(r => r.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.CreatedByUser)
                    .WithMany(u => u.CreatedRoles)
                    .HasForeignKey(r => r.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(entity =>
            {
                // Self-referencing relationships
                entity.HasOne(u => u.CreatedByUser)
                    .WithMany(u => u.CreatedByUsers)
                    .HasForeignKey("CreatedById")
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(u => u.UpdatedByUser)
                    .WithMany(u => u.UpdatedByUsers)
                    .HasForeignKey("UpdatedByUserId")
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(u => u.LastModifiedByUser)
                    .WithMany(u => u.LastModifiedByUsers)
                    .HasForeignKey("LastModifiedByUserId")
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                // Many-to-Many relationship with Role
                entity.HasMany(u => u.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(j => j.ToTable("UserRoles"));

                // One-to-Many relationships
                entity.HasMany(u => u.CreatedNews)
                    .WithOne(n => n.CreatedByUser)
                    .HasForeignKey(n => n.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.UpdatedNews)
                    .WithOne(n => n.UpdatedByUser)
                    .HasForeignKey(n => n.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.CreatedRoles)
                    .WithOne(r => r.CreatedByUser)
                    .HasForeignKey(r => r.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.UpdatedRoles)
                    .WithOne(r => r.UpdatedByUser)
                    .HasForeignKey(r => r.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.LastModifiedRoles)
                    .WithOne(r => r.LastModifiedByUser)
                    .HasForeignKey(r => r.LastModifiedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Ignore virtual collections
                entity.Ignore(u => u.CreatedRecords);
                entity.Ignore(u => u.UpdatedRecords);
            });

           /* modelBuilder.Entity<User>()
                .HasMany(a => a.News)
                .WithOne(n => n.Author)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(a => a.CreatedByUser)
                .WithMany(u => u.CreatedAuthors)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(a => a.UpdatedByUser)
                .WithMany(u => u.UpdatedAuthors)
                .HasForeignKey(a => a.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);*/

            modelBuilder.Entity<User>()
                .HasOne(a => a.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(a => a.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.News)
                .WithOne(n => n.Category)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedCategories)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.UpdatedByUser)
                .WithMany(u => u.UpdatedCategories)
                .HasForeignKey(c => c.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.News)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedComments)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.LastModifiedByUser)
                .WithMany(u => u.UpdatedComments)
                .HasForeignKey(c => c.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

          /*  modelBuilder.Entity<SocialMedia>()
                .HasOne(s => s.User)
                .WithMany(a => a.SocialMediaAccounts)
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);*/

            modelBuilder.Entity<SocialMedia>()
                .HasOne(s => s.CreatedByUser)
                .WithMany(u => u.CreatedSocialMedias)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SocialMedia>()
                .HasOne(s => s.UpdatedByUser)
                .WithMany(u => u.UpdatedSocialMedias)
                .HasForeignKey(s => s.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SocialMedia>()
                .HasOne(s => s.LastModifiedByUser)
                .WithMany(u => u.LastModifiedSocialMedias)
                .HasForeignKey(s => s.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tag>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTags)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tag>()
                .HasOne(t => t.UpdatedByUser)
                .WithMany(u => u.UpdatedTags)
                .HasForeignKey(t => t.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedContacts)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.LastModifiedByUser)
                .WithMany(u => u.UpdatedContacts)
                .HasForeignKey(c => c.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Newsletter>()
                .HasOne(n => n.CreatedByUser)
                .WithMany(u => u.CreatedNewsletters)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Newsletter>()
                .HasOne(n => n.UpdatedByUser)
                .WithMany(u => u.UpdatedNewsletters)
                .HasForeignKey(n => n.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.CreatedByUser)
                .WithMany(u => u.CreatedRoles)
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.UpdatedByUser)
                .WithMany(u => u.UpdatedRoles)
                .HasForeignKey(r => r.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.LastModifiedByUser)
                .WithMany(u => u.LastModifiedRoles)
                .HasForeignKey(r => r.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions)
                .UsingEntity(j => j.ToTable("RolePermissions"));

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            // User-Role ilişkisi
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    j => j.HasOne(ur => ur.Role)
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId),
                    j => j.HasOne(ur => ur.User)
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId)
                );
        }
    }
}

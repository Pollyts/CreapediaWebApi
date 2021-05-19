using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class d2v9eis2ivh7hhContext : DbContext
    {
        public d2v9eis2ivh7hhContext()
        {
        }

        public d2v9eis2ivh7hhContext(DbContextOptions<d2v9eis2ivh7hhContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Trust Server Certificate=True;SSL Mode=Require;Host=ec2-54-220-53-223.eu-west-1.compute.amazonaws.com;Database=d2v9eis2ivh7hh;Username=nszmoagmjrbjkq;Password=bdbc52b63a7f68cc172fac4e9f3a373f0a7d78f837b4471e2ed47c4b2167c23c");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("elements");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Chatacteristicscount).HasColumnName("chatacteristicscount");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Lastupdate).HasColumnName("lastupdate");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.Parentfolderid).HasColumnName("parentfolderid");

                entity.HasOne(d => d.Parentfolder)
                    .WithMany(p => p.Elements)
                    .HasForeignKey(d => d.Parentfolderid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("elements_parentfolderid_fkey");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("folders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Elementscount).HasColumnName("elementscount");

                entity.Property(e => e.Folderscount).HasColumnName("folderscount");

                entity.Property(e => e.Lastupdate).HasColumnName("lastupdate");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.Parentfolderid).HasColumnName("parentfolderid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Parentfolder)
                    .WithMany(p => p.InverseParentfolder)
                    .HasForeignKey(d => d.Parentfolderid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("folders_parentfolderid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("folders_userid_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mail");

                entity.Property(e => e.Mailconfirm).HasColumnName("mailconfirm");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

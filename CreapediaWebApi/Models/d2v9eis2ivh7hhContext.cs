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

        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<Characteristictemplate> Characteristictemplates { get; set; }
        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Templateelement> Templateelements { get; set; }
        public virtual DbSet<Templatefolder> Templatefolders { get; set; }
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

            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.ToTable("characteristic");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CharacteristictemplateId).HasColumnName("characteristictemplate_id");

                entity.Property(e => e.ElementId).HasColumnName("element_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Value)
                    .HasMaxLength(300)
                    .HasColumnName("value");

                entity.HasOne(d => d.Characteristictemplate)
                    .WithMany(p => p.Characteristics)
                    .HasForeignKey(d => d.CharacteristictemplateId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("characteristic_characteristictemplate_id_fkey");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.Characteristics)
                    .HasForeignKey(d => d.ElementId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("characteristic_element_id_fkey");
            });

            modelBuilder.Entity<Characteristictemplate>(entity =>
            {
                entity.ToTable("characteristictemplate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TemplateelementId).HasColumnName("templateelement_id");

                entity.Property(e => e.Value)
                    .HasMaxLength(300)
                    .HasColumnName("value");

                entity.HasOne(d => d.Templateelement)
                    .WithMany(p => p.Characteristictemplates)
                    .HasForeignKey(d => d.TemplateelementId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("characteristictemplate_templateelement_id_fkey");
            });

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("element");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FolderId).HasColumnName("folder_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TemplateelementId).HasColumnName("templateelement_id");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Elements)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("element_folder_id_fkey");

                entity.HasOne(d => d.Templateelement)
                    .WithMany(p => p.Elements)
                    .HasForeignKey(d => d.TemplateelementId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("element_templateelement_id_fkey");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("folder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.TemplatefolderId).HasColumnName("templatefolder_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("folder_project_id_fkey");

                entity.HasOne(d => d.Templatefolder)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.TemplatefolderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("folder_templatefolder_id_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("project_user_id_fkey");
            });

            modelBuilder.Entity<Templateelement>(entity =>
            {
                entity.ToTable("templateelement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TemplatefolderId).HasColumnName("templatefolder_id");

                entity.HasOne(d => d.Templatefolder)
                    .WithMany(p => p.Templateelements)
                    .HasForeignKey(d => d.TemplatefolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templateelement_templatefolder_id_fkey");
            });

            modelBuilder.Entity<Templatefolder>(entity =>
            {
                entity.ToTable("templatefolder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.ParentfolderId).HasColumnName("parentfolder_id");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Parentfolder)
                    .WithMany(p => p.InverseParentfolder)
                    .HasForeignKey(d => d.ParentfolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatefolder_parentfolder_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Templatefolders)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatefolder_userid_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .HasColumnName("mail");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

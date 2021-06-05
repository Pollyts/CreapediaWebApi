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
        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Elementlink> Elementlinks { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<Relation> Relations { get; set; }
        public virtual DbSet<Templatecharacteristic> Templatecharacteristics { get; set; }
        public virtual DbSet<Templateelement> Templateelements { get; set; }
        public virtual DbSet<Templatefolder> Templatefolders { get; set; }
        public virtual DbSet<Templatelink> Templatelinks { get; set; }
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
                entity.ToTable("characteristics");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Elementid).HasColumnName("elementid");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Value)
                    .HasMaxLength(300)
                    .HasColumnName("value");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.Characteristics)
                    .HasForeignKey(d => d.Elementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("characteristics_elementid_fkey");
            });

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("elements");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ifpubic).HasColumnName("ifpubic");

                entity.Property(e => e.Image).HasColumnName("image");

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

            modelBuilder.Entity<Elementlink>(entity =>
            {
                entity.ToTable("elementlinks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Childelementid).HasColumnName("childelementid");

                entity.Property(e => e.Parenttelementid).HasColumnName("parenttelementid");

                entity.HasOne(d => d.Childelement)
                    .WithMany(p => p.Elementlinks)
                    .HasForeignKey(d => d.Childelementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("elementlinks_childelementid_fkey");

                entity.HasOne(d => d.Parenttelement)
                    .WithMany(p => p.Elementlinks)
                    .HasForeignKey(d => d.Parenttelementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("elementlinks_parenttelementid_fkey");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("folders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ifpubic).HasColumnName("ifpubic");

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

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.ToTable("relations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Firstelementid).HasColumnName("firstelementid");

                entity.Property(e => e.Rel1to2)
                    .HasMaxLength(50)
                    .HasColumnName("rel1to2");

                entity.Property(e => e.Rel2to1)
                    .HasMaxLength(50)
                    .HasColumnName("rel2to1");

                entity.Property(e => e.Secondelementid).HasColumnName("secondelementid");

                entity.HasOne(d => d.Firstelement)
                    .WithMany(p => p.RelationFirstelements)
                    .HasForeignKey(d => d.Firstelementid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("relations_firstelementid_fkey");

                entity.HasOne(d => d.Secondelement)
                    .WithMany(p => p.RelationSecondelements)
                    .HasForeignKey(d => d.Secondelementid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("relations_secondelementid_fkey");
            });

            modelBuilder.Entity<Templatecharacteristic>(entity =>
            {
                entity.ToTable("templatecharacteristics");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Telementid).HasColumnName("telementid");

                entity.Property(e => e.Value)
                    .HasMaxLength(300)
                    .HasColumnName("value");

                entity.HasOne(d => d.Telement)
                    .WithMany(p => p.Templatecharacteristics)
                    .HasForeignKey(d => d.Telementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatecharacteristics_telementid_fkey");
            });

            modelBuilder.Entity<Templateelement>(entity =>
            {
                entity.ToTable("templateelements");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ifpubic).HasColumnName("ifpubic");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Parentfolderid).HasColumnName("parentfolderid");

                entity.HasOne(d => d.Parentfolder)
                    .WithMany(p => p.Templateelements)
                    .HasForeignKey(d => d.Parentfolderid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templateelements_parentfolderid_fkey");
            });

            modelBuilder.Entity<Templatefolder>(entity =>
            {
                entity.ToTable("templatefolders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ifpubic).HasColumnName("ifpubic");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Parentfolderid).HasColumnName("parentfolderid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Parentfolder)
                    .WithMany(p => p.InverseParentfolder)
                    .HasForeignKey(d => d.Parentfolderid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatefolders_parentfolderid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Templatefolders)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatefolders_userid_fkey");
            });

            modelBuilder.Entity<Templatelink>(entity =>
            {
                entity.ToTable("templatelinks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Childelementid).HasColumnName("childelementid");

                entity.Property(e => e.Parenttelementid).HasColumnName("parenttelementid");

                entity.HasOne(d => d.Childelement)
                    .WithMany(p => p.TemplatelinkChildelements)
                    .HasForeignKey(d => d.Childelementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatelinks_childelementid_fkey");

                entity.HasOne(d => d.Parenttelement)
                    .WithMany(p => p.TemplatelinkParenttelements)
                    .HasForeignKey(d => d.Parenttelementid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("templatelinks_parenttelementid_fkey");
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

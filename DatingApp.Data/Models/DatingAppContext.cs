using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatingApp.Data.Models
{
    public partial class DatingAppContext : DbContext
    {
        public DatingAppContext()
        {
        }

        public DatingAppContext(DbContextOptions<DatingAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<User> User { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=tcp:shailensqlserver123.database.windows.net,1433;Initial Catalog=DatingApp;Persist Security Info=False;User ID=shailen;Password=DatingApp@");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Likes>(entity =>
            {
                entity.HasKey(e => new { e.LikerId, e.LikeeId });

                entity.HasOne(d => d.Likee)
                    .WithMany(p => p.LikesLikee)
                    .HasForeignKey(d => d.LikeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_User1");

                entity.HasOne(d => d.Liker)
                    .WithMany(p => p.LikesLiker)
                    .HasForeignKey(d => d.LikerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_User");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.Property(e => e.DateRead).HasColumnType("datetime");

                entity.Property(e => e.MessageSent).HasColumnType("datetime");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.MessagesRecipient)
                    .HasForeignKey(d => d.RecipientId)
                    .HasConstraintName("FK_Messages_User1");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessagesSender)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Messages_User");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PublicId)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Photo_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.Interests)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Introduction)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.KnownAs)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastActive).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasMaxLength(500);

                entity.Property(e => e.PasswordSalt).HasMaxLength(500);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

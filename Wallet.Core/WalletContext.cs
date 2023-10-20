using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Drawing;
using System.IO;
using Wallet.Core.Entities;

namespace Wallet.Core
{
    public class WalletContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserInfoEntity> UserInfos { get; set; }

        public DbSet<TransactionEntity> Transactions { get; set; }

        public WalletContext(DbContextOptions<WalletContext> options)
            : base(options)
        {
        }

        public class IconValueConverter : ValueConverter<Icon, byte[]>
        {
            public IconValueConverter() : base(
                icon => IconToByteArray(icon),
                byteArray => ByteArrayToIcon(byteArray))
            { }

            private static byte[] IconToByteArray(Icon icon)
            {
                if (icon == null)
                    return null;

                using (MemoryStream stream = new MemoryStream())
                {
                    icon.Save(stream);
                    return stream.ToArray();
                }
            }

            private static Icon ByteArrayToIcon(byte[] byteArray)
            {
                if (byteArray == null || byteArray.Length == 0)
                    return null;

                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    //BinaryFormatter bf = new BinaryFormatter();
                    //return (Icon)bf.Deserialize(ms);

                    return new Icon(ms);
                }

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HashCode)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<UserInfoEntity>(entity =>
            {
                entity.Property(e => e.StoryPoints)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoPaymentDue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<TransactionEntity>(entity =>
            {
                entity.Property(e => e.TransactionName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorizedUser)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasConversion(new IconValueConverter());

                entity.HasKey(e => e.Id);
            }); ;
        }
    }
}

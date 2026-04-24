using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class AddressConfiguration: IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("address");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(u => u.City).HasColumnName("city").IsRequired().HasMaxLength(30);
            builder.Property(u => u.Street).HasColumnName("street").IsRequired().HasMaxLength(30);
            builder.Property(u => u.Number).HasColumnName("number");
            builder.Property(u => u.ZipCode).HasColumnName("zipcode").IsRequired().HasMaxLength(8);
            builder.Property(u => u.Geolocation_lat).HasColumnName("geolocation_lat").HasMaxLength(15);
            builder.Property(u => u.Geolocation_long).HasColumnName("geolocation_long").HasMaxLength(15);
            builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone");

            builder.HasOne(a => a.User)
               .WithOne(u => u.Address)
               .HasForeignKey<Address>(a => a.UserId);

        }
    }
}

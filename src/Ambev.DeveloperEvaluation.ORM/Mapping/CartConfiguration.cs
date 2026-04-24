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
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(u => u.Date).HasColumnName("date").IsRequired();
            builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone");

            builder.HasOne(p => p.User)
           .WithMany(b => b.Cart)
           .HasForeignKey(p => p.UserId);
        }
    }
}

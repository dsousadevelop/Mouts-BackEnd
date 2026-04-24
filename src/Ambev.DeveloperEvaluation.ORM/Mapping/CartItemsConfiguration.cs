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
    public class CartItemsConfiguration: IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cartitems");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.CartId).HasColumnName("cart_id").IsRequired();
            builder.Property(u => u.ProductId).HasColumnName("product_id").IsRequired();
            builder.Property(u => u.Quantity).HasColumnName("quantity").IsRequired().HasPrecision(10,2);
            builder.Property(u => u.Discount).HasColumnName("discount").HasPrecision(10, 2);
            builder.Property(u => u.SubTotal).HasColumnName("subtotal").IsRequired().HasPrecision(10, 2);
            builder.Property(u => u.Total).HasColumnName("total").IsRequired().HasPrecision(10, 2);
            builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone");

            builder.HasOne(p => p.Cart)
           .WithMany(b => b.CartItems)
           .HasForeignKey(p => p.CartId);

            builder.HasOne(p => p.Product)
           .WithMany(b => b.CartItems)
           .HasForeignKey(p => p.ProductId);
        }
    }
}

using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.Title).HasColumnName("title").HasMaxLength(60).IsRequired();
            builder.Property(u => u.Price).HasColumnName("price").HasPrecision(10, 2).IsRequired();
            builder.Property(u => u.Description).HasColumnName("description").HasMaxLength(100).IsRequired();
            builder.Property(u => u.CategoryId).HasColumnName("category_id").IsRequired();
            builder.Property(u => u.Image).HasColumnName("image");
            builder.Property(u => u.Rating_Rate).HasColumnName("rating_rate");
            builder.Property(u => u.Rating_Count).HasColumnName("rating_count");
            builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone"); ;
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone"); ;

            builder.HasOne(p => p.Category)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.CategoryId);
}
    }
}

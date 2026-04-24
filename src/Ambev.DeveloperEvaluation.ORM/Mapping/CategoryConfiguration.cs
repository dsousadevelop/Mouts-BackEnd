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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.Description).HasColumnName("description").HasMaxLength(40).IsRequired();
            builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone");
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone");
        }
    }
}

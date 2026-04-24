using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        // builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
        builder.Property(u => u.FirstName).HasColumnName("firstname").IsRequired().HasMaxLength(40);
        builder.Property(u => u.LastName).HasColumnName("lastname").IsRequired().HasMaxLength(40);
        builder.Property(u => u.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
        builder.Property(u => u.Password).HasColumnName("password").IsRequired().HasMaxLength(40);
        builder.Property(u => u.CreatedAt).HasColumnName("createdat").IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone");
        builder.Property(u => u.Status).HasColumnName("status")
            .HasConversion<int>();
        builder.Property(u => u.Role).HasColumnName("roles")
            .HasConversion<int>();

    }
}

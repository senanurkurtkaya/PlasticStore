using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntitiyConfigs
{
    internal class ProductMetaTagsEntityConfig : IEntityTypeConfiguration<ProductMetaTags>
    {
        public void Configure(EntityTypeBuilder<ProductMetaTags> builder)
        {
            builder.HasKey(p => p.Id); // Primary Key

            builder.Property(p => p.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.OpenGraphType)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(p => p.OpenGraphUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.TwitterTitle)
                .HasMaxLength(200)
                .IsRequired(false);


        builder.HasOne(p => p.Product) // Product -> MetaTags
               .WithOne(m => m.MetaTags) // MetaTags -> Product
               .HasForeignKey<ProductMetaTags>(p => p.ProductId) // Foreign Key
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}

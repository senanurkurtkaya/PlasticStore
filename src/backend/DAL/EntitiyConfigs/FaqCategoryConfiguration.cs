using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntitiyConfigs
{
    public class FaqCategoryConfiguration : IEntityTypeConfiguration<FaqCategory>
    {
        public void Configure(EntityTypeBuilder<FaqCategory> builder)
        {
            builder.ToTable("FaqCategories"); 

            builder.HasKey(fc => fc.Id); 

            builder.Property(fc => fc.Name)
                   .IsRequired()
                   .HasMaxLength(100);           
           
        }
    }
}

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
    public class FrequentlyAskedQuestionConfiguration : IEntityTypeConfiguration<FrequentlyAskedQuestion>
    {
        public void Configure(EntityTypeBuilder<FrequentlyAskedQuestion> builder)
        {
            builder.ToTable("FrequentlyAskedQuestions");

            builder.HasKey(faq => faq.Id); 

            builder.Property(faq => faq.Question)
                   .IsRequired()
                   .HasMaxLength(500); 

            builder.Property(faq => faq.Answer)
                   .IsRequired()
                   .HasMaxLength(1000); 

      

            builder.HasOne(faq => faq.FaqCategory)
                   .WithMany(fc => fc.Questions)
                   .HasForeignKey(faq => faq.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
    
}

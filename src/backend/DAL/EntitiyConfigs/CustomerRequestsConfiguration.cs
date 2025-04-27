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
    public class CustomerRequestsConfiguration : IEntityTypeConfiguration<CustomerRequest>
    {
        public void Configure(EntityTypeBuilder<CustomerRequest> builder)
        {
            builder.ToTable("CustomerRequests");

            // **Primary Key**
            builder.HasKey(cr => cr.Id);

            // **Zorunlu Alanlar (Not Null)**
            builder.Property(cr => cr.Title)
                .HasMaxLength(250);

            builder.Property(cr => cr.Description)
                .HasMaxLength(1000);

            builder.Property(cr => cr.UserId)
                .IsRequired();

            builder.Property(cr => cr.CustomerName)
                .HasMaxLength(100);

            builder.Property(cr => cr.CustomerSurname)
                .HasMaxLength(100);

            builder.Property(cr => cr.CustomerEmail)
                .HasMaxLength(150);

            builder.Property(cr => cr.RequestDetails)
                .HasMaxLength(2000);

            // **CategoryId Foreign Key (Opsiyonel - NULL olabilir)**
            builder.HasOne(cr => cr.Category)
                .WithMany()
                .HasForeignKey(cr => cr.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // Kategori silinirse, NULL yap

            // **AssignedUser Foreign Key (Opsiyonel)**
            builder.HasOne(cr => cr.AssignedUser)
                .WithMany()
                .HasForeignKey(cr => cr.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse, bu kayıt silinmez.

            // **Customer Foreign Key (Zorunlu)**
            builder.HasOne(cr => cr.Customer)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse, bu kayıt da silinir.

            // **Teklif Alanları (Opsiyonel)**
            builder.Property(cr => cr.IsProposalRequest)
                .HasDefaultValue(false);

            builder.Property(cr => cr.EstimatedPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(cr => cr.ProposalDetails)
                .HasMaxLength(2000);
        }
    }
}

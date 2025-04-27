using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CustomerRequest:BaseClass
    {
        //public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }   
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerEmail { get; set; }
        public string? RequestDetails { get; set; }
        public string? AssignedTo { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Oluşturuldu;
        public bool? IsProposalRequest { get; set; } // Teklif mi?
        public decimal? EstimatedPrice { get; set; } // Teklif edilen fiyat
        public string? ProposalDetails { get; set; } // Teklif içeriği
        public User? AssignedUser { get; set; }
        public User Customer { get; set; }

    }
}

using PlastikMVC.Enums;

namespace PlastikMVC.Models
{
    public class CustomerRequest:BaseEntity
    {

            public string UserId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerSurname { get; set; }
            public string CustomerUsername { get; set; }
            public string CustomerEmail { get; set; }
            public string RequestDetails { get; set; }
            public string? AssignedTo { get; set; }
            public RequestStatus Status { get; set; } = RequestStatus.Oluşturuldu;
            public bool? IsProposalRequest { get; set; }  // Teklif talebi mi?
            public decimal? EstimatedPrice { get; set; }  // Güncellenen teklif fiyatı
            public string? ProposalDetails { get; set; }
            public User? AssignedUser { get; set; }
            public User Customer { get; set; }        
    }
}

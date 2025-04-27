using PlastikMVC.Enums;

namespace PlastikMVC.AllDtos.CustomerRequestDtos
{
    public class CustomerRequestUpdateDto  : BaseDto
    {
        public string? RequestDetails { get; set; }
        public string? AssignedTo { get; set; }
        public RequestStatus? Status { get; set; }
        public bool? IsProposalRequest { get; set; }  // Teklif talebi mi?
        public decimal? EstimatedPrice { get; set; }  // Güncellenen teklif fiyatı
        public string? ProposalDetails { get; set; }   //  teklif Detayı
    }
}

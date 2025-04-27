namespace PlastikMVC.AllDtos.CustomerRequestDtos
{
    public class PurposalRequestViewModel
    {
        public string? UserId { get; set; }
        public string? AssignedTo { get; set; }
        public bool? IsProposalRequest { get; set; } // Teklif mi?
        public decimal? EstimatedPrice { get; set; } // Teklif edilen fiyat
        public string? ProposalDetails { get; set; } // Teklif içeriği
        public string? CustomerUsername { get; set; }
        public string? CustomerEmail { get; set; }
    }
}

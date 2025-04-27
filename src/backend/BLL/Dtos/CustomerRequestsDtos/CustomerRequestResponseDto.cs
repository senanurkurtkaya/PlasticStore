using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CustomerRequestsDtos
{
    public class CustomerRequestResponseDto :BaseDto
    {
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerEmail { get; set; }
        public string RequestDetails { get; set; }
        public string? AssignedTo { get; set; }
        public string? AssignedUserName { get; set; }
        public bool? IsProposalRequest { get; set; } // Teklif mi?
        public decimal? EstimatedPrice { get; set; } // Teklif edilen fiyat
        public string? ProposalDetails { get; set; } // Teklif içeriği
        public RequestStatus Status { get; set; }
    }
}

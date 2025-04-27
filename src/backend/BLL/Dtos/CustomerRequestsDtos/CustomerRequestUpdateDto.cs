using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CustomerRequestsDtos
{
    public class CustomerRequestUpdateDto:BaseDto
    {
        public string? RequestDetails { get; set; }
        public string? AssignedTo { get; set; }
        public RequestStatus? Status { get; set; }
        public bool? IsProposalRequest { get; set; } // Teklif mi?
        public decimal? EstimatedPrice { get; set; } // Teklif edilen fiyat
        public string? ProposalDetails { get; set; } // Teklif içeriği
    }
}

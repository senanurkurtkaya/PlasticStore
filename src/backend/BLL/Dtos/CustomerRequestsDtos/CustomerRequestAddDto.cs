using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Dtos.CustomerRequestsDtos
{
    public class CustomerRequestAddDto:BaseDto
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public string? RequestDetails { get; set; }
        public string? AssignedTo { get; set; }
        public bool? IsProposalRequest { get; set; } // Teklif mi?
        public decimal? EstimatedPrice { get; set; } // Teklif edilen fiyat
        public string? ProposalDetails { get; set; } // Teklif içeriği
        public int CategoryId { get; set; }
        public string? CustomerUsername { get; set; }
        public string? CustomerEmail { get; set; }

    }
}

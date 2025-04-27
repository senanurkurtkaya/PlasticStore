using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CustomerRequestsDtos
{
    public class CustomerRequestDto:BaseDto
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
        public DateTime CreatedDate { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }        
        public string? CategoryName { get; set; }
        public BLL.Dtos.CategoryDto.CategoryDto? Category { get; set; }
    }
}

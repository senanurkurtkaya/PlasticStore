using BLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CustomerDto
{
    public class UpdateCustomerRequestDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        [EnumDataType(typeof(CustomerRequestStatus), ErrorMessage = "Geçersiz durum. Sadece 'Beklemede', 'İşlemde' veya 'Tamamlandı' olabilir.")]
        public string Status { get; set; }
    }

}

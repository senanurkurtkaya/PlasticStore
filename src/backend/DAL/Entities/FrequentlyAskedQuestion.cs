using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class FrequentlyAskedQuestion :BaseClass
    {
     
        public string Question { get; set; }

        public string Answer { get; set; }

        public int CategoryId { get; set; }
        public virtual FaqCategory FaqCategory { get; set; }
    }
}

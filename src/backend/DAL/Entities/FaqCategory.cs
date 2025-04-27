using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class FaqCategory :BaseClass
    {

        public string Name { get; set; }

        public virtual ICollection<FrequentlyAskedQuestion> Questions { get; set; }
    }
}

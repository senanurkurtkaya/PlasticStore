using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductMetaTags :BaseClass
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string OpenGraphType { get; set; }

        [MaxLength(4000)]
        public string OpenGraphUrl { get; set; }

        public string OpenGraphTitle  { get; set; }

        public string OpenGraphDescription { get; set; }

        public string OpenGraphImage { get; set; }



        public string TwitterCard { get; set; }
        public string TwitterUrl { get; set; }

        public string TwitterTitle { get; set; }

        public string TwitterDescription { get; set; }

        public string TwitterImage { get; set; }


        public  int ProductId { get; set; }

     
        public Product Product { get; set; }

      


    }
}

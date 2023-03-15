using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.DBModels
{
    public class Category
    {
        [Key]
        public int Category_ID { get; set; }
        public string Description { get; set; }
        public DateTime Load_DateTime { get; set; }

    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace CoreWebApiWithEFCore.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}

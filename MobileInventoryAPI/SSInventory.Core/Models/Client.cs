using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Client
    {
        public int ClientId { get; set; }
        public int? TeamdesignCustNo { get; set; }
        public int? SsidbClientId { get; set; }
        public string ClientName { get; set; }
        public bool HasInventory { get; set; }
    }
}

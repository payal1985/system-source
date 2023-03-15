using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ClientInventoryDisplaycolumn { get; set; }
        public string Path { get; set; }
        public bool InventoryApp { get; set; }
        public string Permission { get; set; }


    }
}

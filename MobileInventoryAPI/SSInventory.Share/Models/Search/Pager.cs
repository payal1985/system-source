using System.ComponentModel;

namespace SSInventory.Share.Models.Search
{
    public class Pager
    {
        [DefaultValue(1)]
        public int CurrentPage { get; set; } = 1;
        [DefaultValue(10)]
        public int ItemsPerPage { get; set; } = 10;
    }
}

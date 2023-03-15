using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class UpdateLocationInputModel
    {
        public int ClientId { get; set; }
        public LocationModel Locations { get; set; }
        public List<int> InventoryItemId { get; set; }
    }

    public class LocationModel
    {
        public TupleModel Building { get; set; }
        public TupleModel Floor { get; set; }
        public GpsLocation Gps { get; set; }
        public string RoomNumber { get; set; }
    }

    public class TupleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GpsLocation
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}

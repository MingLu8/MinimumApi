namespace MinimumApi.Entities
{
    public class RoutePoint
    {
        public string RouteId { get; set; }
        public string DriverId { get; set; }
        public DateTime Time { get; set; }
        public long SequenceNumber { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }
}

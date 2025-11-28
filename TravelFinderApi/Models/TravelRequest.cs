namespace TravelFinderApi.Models
{
    public class TravelRequest
    {
        public string CurLatitude { get; set; }
        public string CurLongitude { get; set; }
        public string DestinationDistrict { get; set; }
        public DateTime TravelDate { get; set; }
    }
}

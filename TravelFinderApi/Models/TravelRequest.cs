using System.ComponentModel.DataAnnotations;

namespace TravelFinderApi.Models
{
    public class TravelRequest
    {
        [Required]
        public string CurLatitude { get; set; }
        [Required]
        public string CurLongitude { get; set; }
        [Required]
        public string DestinationDistrict { get; set; }
        public DateTime? TravelDate { get; set; }
    }
}

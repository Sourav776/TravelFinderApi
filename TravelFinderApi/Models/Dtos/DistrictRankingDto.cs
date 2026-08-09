namespace TravelFinderApi.Models.Dtos
{
    public record DistrictRankingDto
    (
        string DistrictName,
        string BanglaName,
        string Lat,
        string Long,
        double AverageTemp2PM,
        double AverageAirPM25
    );
}

namespace TravelFinderApi.Models.Dtos
{
    public record DistrictRankingDto
    (
        string DistrictName,
        string BanglaName,
        string Lat,
        double AverageTemp2PM,
        double AverageAirPM25
    );
}

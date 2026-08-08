namespace TravelFinderApi.Models.Dtos
{
    public record DistrictRankingDto
    (
        string DistrictName,
        string BanglaName,
        double AverageTemp2PM,
        double AverageAirPM25
    );
}

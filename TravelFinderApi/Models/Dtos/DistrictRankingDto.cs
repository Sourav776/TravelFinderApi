namespace TravelFinderApi.Models.Dtos
{
    public record DistrictRankingDto
    (
        string DistrictName,
        double AverageTemp2PM,
        double AverageAirPM25
    );
}

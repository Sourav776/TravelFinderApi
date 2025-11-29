🚀 Setup & Run Instructions

📦 Prerequisites

-   .NET 8 SDK (download from [https://dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
-   Visual Studio 2022 or VS Code with C# extension (optional)

📥 Clone the Repository

    git clone https://github.com/Sourav776/TravelFinderApi.git
    cd TravelFinderApi\TravelFinderApi.Soln

📚 Install Dependencies

    dotnet restore

▶️ Run the API

    cd..
    cd TravelFinderApi
    dotnet run

-   Server starts on https://localhost:7226 or http://localhost:5188.
-   Initial startup fetches/caches data (~2–5 seconds; see console
    logs).
-   Access Swagger: [https://localhost:7001/swagger](https://localhost:7226/index.html).

🧪 Test Endpoints

✅ 1. GET /api/Ranking/GetTop10

Returns Coolest and Cleanest 10 Districts.

Example Response:

    [
    {
    "DistrictName": "Kushtia",
    "AverageTemp2PM": 24.97,
    "AverageAirPM25": 88.4
    },
    {
    "DistrictName": "Jhalokati",
    "AverageTemp2PM": 25,
    "AverageAirPM25": 75.11
    }
    ]

2. ✅ POST /api/Travel/Recommend

Request Body:

    {
      "CurLatitude": "23.48",
      "CurLongitude": "89.41",
      "DestinationDistrict": "Cox's Bazar",
      "TravelDate": "2025-12-01"
    }

Example Response:

    {
      "Status": "Recommended",
      "Reason": "Your destination is 1.2°C cooler and has significantly better air quality. Enjoy your trip!"
    }

📚 Libraries & Versions

-   .NET 8.0
-   Swashbuckle.AspNetCore 6.6.2
-   Microsoft.Extensions.Caching.Memory 8 (Default shipment with .NET 8.0)

📝 Notes

-   No database needed (in-memory cache).

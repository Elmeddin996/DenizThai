using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Denizthai.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly string _propertyId = "501346344";
        private readonly IWebHostEnvironment _env;

        public AnalyticsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<IActionResult> AnalyticsData()
        {
            var keyFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "denizthai-analytics-b23d53ab3b43.json");

            var credential = GoogleCredential.FromFile(keyFilePath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var client = new BetaAnalyticsDataClientBuilder
            {
                Credential = credential
            }.Build();

            // --------- Realtime Data ---------
            var realtimeRequest = new RunRealtimeReportRequest
            {
                Property = $"properties/{_propertyId}",
                Metrics = { new Metric { Name = "activeUsers" } },
                Dimensions = {
                    new Dimension { Name = "country" },
                    new Dimension { Name = "city" },
                    new Dimension { Name = "deviceCategory" }
                }
            };

            var realtimeResponse = await client.RunRealtimeReportAsync(realtimeRequest);

            var realtimeData = new List<object>();

            if (realtimeResponse.Rows != null && realtimeResponse.Rows.Count > 0)
            {
                realtimeData = realtimeResponse.Rows.Select(r => new
                {
                    Country = r.DimensionValues[0].Value,
                    City = r.DimensionValues[1].Value,
                    Device = r.DimensionValues[2].Value,
                    ActiveUsers = r.MetricValues[0].Value
                }).ToList<object>();
            }


            // --------- Historical Data ---------
            var historicalClient = client; // eyni client-dən istifadə edə bilərik

            // Funksiya: start və end date arasında user sayını çəkmək
            async Task<string> GetUserCount(string startDate, string endDate)
            {
                var request = new RunReportRequest
                {
                    Property = $"properties/{_propertyId}",
                    Metrics = { new Metric { Name = "activeUsers" } },
                    DateRanges = { new DateRange { StartDate = startDate, EndDate = endDate } }
                };

                var response = await historicalClient.RunReportAsync(request);
                if (response.Rows != null && response.Rows.Count > 0)
                    return response.Rows[0].MetricValues[0].Value;
                return "0";
            }

            var day1Users = await GetUserCount("1daysAgo", "today");
            var last7DaysUsers = await GetUserCount("7daysAgo", "today");
            var last30DaysUsers = await GetUserCount("30daysAgo", "today");

            // --------- JSON formatında qaytarmaq ---------
            return Json(new
            {
                Realtime = realtimeData,
                Day1Users = day1Users,
                Last7DaysUsers = last7DaysUsers,
                Last30DaysUsers = last30DaysUsers
            });
        }
    }
}

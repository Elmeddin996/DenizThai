using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Denizthai.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly string _propertyId = "GA4_PROPERTY_ID"; // GA4 Property ID (Admin → Property Settings)
        private readonly string _keyFilePath = @"C:\Path\to\your\service-account.json";

        public async Task<IActionResult> RealtimeUsers()
        {
            var credential = GoogleCredential.FromFile(_keyFilePath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var client = new BetaAnalyticsDataClientBuilder
            {
                Credential = credential
            }.Build();

            var request = new RunRealtimeReportRequest
            {
                Property = $"properties/{_propertyId}",
                Metrics = { new Metric { Name = "activeUsers" } }
            };

            var response = await client.RunRealtimeReportAsync(request);

            var activeUsers = response.Rows.Count > 0 ? response.Rows[0].MetricValues[0].Value : "0";
            return Json(new { activeUsers });
        }
    }
}

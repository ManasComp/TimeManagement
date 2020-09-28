using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodOrderApp.Helpers
{
    public static class AnalyticsHelper
    {
        public static async Task TrackEventAsync(string eventName, Dictionary<string, string> properties = null)
        {
            if (await Microsoft.AppCenter.Analytics.Analytics.IsEnabledAsync())
                Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName,properties);
        }
    }
}
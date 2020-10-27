using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeManagement.Helpers
{
    public class AnalyticsHelper
    {
        public async Task TrackEventAsync(string eventName, Dictionary<string, string> properties = null)
        {
            if (await Microsoft.AppCenter.Analytics.Analytics.IsEnabledAsync())
                Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName,properties);
        }
    }
}
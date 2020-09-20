﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace FoodOrderApp.Helpers
{
    public static class CrashesHelper
    {
        public static async Task TrackErrorAsync(Exception ex, Dictionary<string, string> properties = null)
        {
            if( await  Crashes.IsEnabledAsync())
                Crashes.TrackError(ex, properties);
        }
    }
}
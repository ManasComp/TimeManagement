﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace FoodOrderApp.Converters
{
    public class TextToBoolConverter:IValueConverter//trigger value - changes IsEnable of buttons on login page
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
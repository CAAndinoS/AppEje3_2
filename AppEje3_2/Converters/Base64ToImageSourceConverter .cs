﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AppEje3_2.Converters
{
    public class Base64ToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String)
            {
                byte[] imageBytes = System.Convert.FromBase64String(base64String);
                return ImageSource.FromStream(() => new System.IO.MemoryStream(imageBytes));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

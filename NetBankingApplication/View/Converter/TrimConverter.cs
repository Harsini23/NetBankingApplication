using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace NetBankingApplication.View.Converter
{
    public class TrimConverter : DependencyObject, IValueConverter
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TrimConverter), new PropertyMetadata(""));


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isTrim = System.Convert.ToBoolean(value);
            return isTrim ? Text : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string imagePath)
            {
                BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                if (imagePath != null || string.IsNullOrWhiteSpace(imagePath))
                {
                    bitmapImage = new BitmapImage(new Uri(imagePath));
                }
                return bitmapImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

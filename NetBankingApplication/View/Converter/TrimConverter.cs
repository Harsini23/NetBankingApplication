using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

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
}

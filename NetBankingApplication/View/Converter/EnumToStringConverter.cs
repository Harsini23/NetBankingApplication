using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Text.RegularExpressions;

namespace NetBankingApplication.View.Converter
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !value.GetType().IsEnum)
                return "";

            var stringVal= value.ToString();
            return typeof(CurrencyValues).GetField(stringVal, BindingFlags.Public | BindingFlags.Static).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AccountTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";
            string outputString="";
         
           outputString = Regex.Replace(value.ToString(), "(?<=[a-z])([A-Z])", " $1");

            return outputString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

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
    
    public class AccountTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";
                // Check if the input value is an enum value
                if (value is Enum enumValue)
                {
                    // Get the value of the static variable with the same name
                    string staticVariableValue = typeof(EnumToStringConversion).GetField(enumValue.ToString())?.GetValue(null) as string;
                    // Return the value of the static variable, or the enum name if no matching variable was found
                    return staticVariableValue ?? enumValue.ToString();
                }
                return null;
            }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                string convertVal= Regex.Replace(value as string, @"\s+", "");

                if (string.IsNullOrWhiteSpace(convertVal))
                    return null;

                // Check if the input string matches a static variable value
                var matchingField = typeof(EnumToStringConversion).GetFields()
                    .FirstOrDefault(field => (string)field.GetValue(null) == convertVal);
                if (matchingField != null)
                    return Enum.Parse(targetType, matchingField.Name);

                // Otherwise, try to parse the input string directly as an enum name
                return Enum.Parse(targetType, convertVal);
            }
            return null;
        }
    }
}

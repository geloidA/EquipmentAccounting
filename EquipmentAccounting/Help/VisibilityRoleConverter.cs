using EquipmentAccounting.DataBase;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EquipmentAccounting.Help
{
    public class VisibilityRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Users user)
            {
                return user.Roles.Name == "User" ? Visibility.Collapsed : Visibility.Visible;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

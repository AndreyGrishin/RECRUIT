using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using RecruIT.Model.DBModel;

namespace RecruIT.ViewModel
{
    class PostConverterToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = value as Posts;
            return val.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

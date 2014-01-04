using System;
using System.Windows;
using System.Windows.Data;

namespace TaskMaster.Model.Converters
{
    public class PlayingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PlayingState? playing = (PlayingState?)value;
            if (playing != null && playing == PlayingState.Playing)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

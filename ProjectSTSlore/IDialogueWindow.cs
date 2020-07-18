using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProjectSTSlore
{
    interface IDialogueWindow
    {
        void Submit_Click(object sender, RoutedEventArgs e);
        void Cancel_Click(object sender, EventArgs e);
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e);
    }
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var bi = new BitmapImage();
            MemoryStream ms = new MemoryStream(value as byte[]);
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

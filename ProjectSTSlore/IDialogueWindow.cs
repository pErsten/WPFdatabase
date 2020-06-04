using System;
using System.Windows;
using System.Windows.Input;

namespace ProjectSTSlore
{
    interface IDialogueWindow
    {
        void Submit_Click(object sender, RoutedEventArgs e);
        void Cancel_Click(object sender, EventArgs e);
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e);
    }
}

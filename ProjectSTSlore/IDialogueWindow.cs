using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ProjectSTSlore
{
    interface IDialogueWindow
    {
        void Submit_Click(object sender, RoutedEventArgs e);
        void Cancel_Click(object sender, EventArgs e);
    }
}

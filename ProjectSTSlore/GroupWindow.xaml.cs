using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectSTSlore
{
    /// <summary>
    /// Логика взаимодействия для GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window, INotifyPropertyChanged
    {
        public Group group;
        public GroupWindow(Group group)
        {
            InitializeComponent();

            this.group = new Group(group.groupNumber);
            GroupWindow_GroupNumber.Text = group.groupNumber.ToString();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            group.groupNumber = Convert.ToInt32(GroupWindow_GroupNumber.Text);
            this.DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            group = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

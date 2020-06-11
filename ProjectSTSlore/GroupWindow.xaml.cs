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
    public partial class GroupWindow : Window, IDialogueWindow
    {
        public Group group;
        public GroupWindow(Group group)
        {
            InitializeComponent();

            this.group = new Group(group.groupNumber);
            //hmm, yes, floor made out of floor
            GroupWindow_GroupNumber.Text = (group.groupNumber != null) ? group.groupNumber.ToString() : null;
            this.Closed += Cancel_Click;
        }

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            int groupNumber;
            try
            {
                groupNumber = Convert.ToInt32(GroupWindow_GroupNumber.Text);
            }
            catch
            {
                Entity.errorMessage("use correct symbols!");
                return;
            }

            if (group.groupNumber == default)//if it was add command
            {

            }
            else//if it was edit command
            {
                if (groupNumber == group.groupNumber)
                {
                    this.Closed -= Cancel_Click;
                    this.DialogResult = true;
                    return;
                }
            }
            group.groupNumber = groupNumber;
            if (!(MainProgram.groups as DBGroups).Check(group))
            {
                group.groupNumber = default;
                return;
            }
            this.Closed -= Cancel_Click;
            this.DialogResult = true;
        }

        public void Cancel_Click(object sender, EventArgs e)
        {
            group = null;
        }

        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

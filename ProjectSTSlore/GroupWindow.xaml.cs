using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;

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
            if (group != null)
                this.group = new Group(group.groupNumber, group.image, 0);
            else
                this.group = new Group(null, null, 0);
            GroupWindow_GroupNumber.Text = this.group.groupNumber.ToString();
            Closed += Cancel_Click;
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
            if (!MainProgram.groups.Check(group))
            {
                group.groupNumber = null;
                return;
            }
            this.Closed -= Cancel_Click;
            this.DialogResult = true;
        }

        public void OpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (!String.IsNullOrEmpty(openFileDialog.FileName))
                {
                    try
                    {
                        new FileInfo(openFileDialog.FileName).CopyTo($"{MainProgram.homeDirectory}\\images", false);
                    }
                    catch
                    {
                        Entity.errorMessage("Error: file with such name is already exist, rename this");
                    }
                }
            }
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

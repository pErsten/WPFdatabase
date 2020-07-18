using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
//using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

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

            if (this.group.image != null)
                GroupWindow_SelectedImage.Source = ByteArrayToImage(this.group.image);
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

            if (groupNumber != group.groupNumber)
            {
                group.groupNumber = groupNumber;
                if (!MainProgram.groups.Check(group))
                {
                    group = null;
                    return;
                }
            }
            this.Closed -= Cancel_Click;
            this.DialogResult = true;
        }

        static public byte[] ImageToByteArray(ref OpenFileDialog openFileDialog, byte[] image)
        {
            using (var filestream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
            {
                image = new byte[filestream.Length];
                filestream.Read(image, 0, Convert.ToInt32(filestream.Length));

                return image;
            }
        }

        static public ImageSource ByteArrayToImage(byte[] image)
        {
            var bi = new BitmapImage();
            MemoryStream ms = new MemoryStream(image);
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public void OpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog //setting requirements for search and selecting the file
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png" 
            };

            if (openFileDialog.ShowDialog() != true || String.IsNullOrEmpty(openFileDialog.FileName)) return; //return if search was canceled or file doesn't have a name 

            group.image = ImageToByteArray(ref openFileDialog, group.image);
            GroupWindow_SelectedImage.Source = ByteArrayToImage(group.image);
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

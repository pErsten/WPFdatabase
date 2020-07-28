using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjectSTSlore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainProgram mainProgram = new MainProgram();
        public MainWindow()
        {
            InitializeComponent();
            GroupsListView.ItemsSource = MainProgram.groups.Get();
            StudentsListView.ItemsSource = MainProgram.students.Get();
            TeachersListView.ItemsSource = MainProgram.teachers.Get();
            SubjectsListView.ItemsSource = MainProgram.subjects.Get();
            DataContext = mainProgram;
        }
    }
}

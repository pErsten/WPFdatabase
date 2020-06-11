using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectSTSlore
{
    public class MainProgram : INotifyPropertyChanged
    {
        /*
         * a shit ton of collections
         */
        public static ObservableCollection<Student> students { set; get; } = new DBStudents();//this looks terrifying, but it's the only way the program works
        public static ObservableCollection<Teacher> teachers { set; get; } = new DBTeachers();
        public static ObservableCollection<Subject> subjects { set; get; } = new DBSubjects();
        public static ObservableCollection<Group> groups { set; get; } = new DBGroups();
        public static ObservableCollection<Group_TeacherSubject> group_teacherSubjects { set; get; } = new DBGroup_TeacherSubjects();
        public static ObservableCollection<Teacher_Subject> teacher_subjects { set; get; } = new DBTeacher_Subjects();
        public static ObservableCollection<Marks> marks { set; get; } = new DBMarks();
        public static ObservableCollection<Person> persons { set; get; } = new DBPersons();

        /*
         * Dialogue windows
         */
        //every dialogue window must set it's owner as MainWindow everytime it receves a new value
        private GroupWindow _groupWindow;
        public GroupWindow groupWindow
        {
            get { return _groupWindow; }
            set { _groupWindow = value; _groupWindow.Owner = Application.Current.MainWindow; }
        }

        /*
         * default entities, for the sake of having a default user
         */
        //ending parameter in every default entity must be zero
        public static Group defaultGroup = new Group(default, 0);
        public static Person defaultPerson = new Person(default, default, default, default, 0);
        public static Teacher defaultTeacher = new Teacher(defaultPerson, 0);
        public static Student defaultStudent = new Student(defaultPerson, defaultGroup, 0);

        /*
         * here are the class fields
         */
        private Group selectedGroup;
        public Group SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                selectedGroup = value;
                ChangeProperty();
            }
        }

        /*
         * commands
         */
        private CommandClass addGroup;
        private CommandClass editGroup;
        private CommandClass deleteGroup;
        //these commands send a copy of original value to dialogue when entity is added or edited
        //the dialogue then manipulate with the copy of entity
        //and after sends it back
        //command checks if it's null(when user cancelled this operation) and if it is - remains status quo
        //vice versa - the value is either added or changed in the original list of entities
        public CommandClass AddGroup
        {
            get
            {
                return addGroup ??
                (addGroup = new CommandClass(obj =>
                {
                    groupWindow = new GroupWindow(defaultGroup);//sending default value to class, where it creates copy of it
                    groupWindow.ShowDialog();//and if it's all okay with new group(no exceptions)
                    if (groupWindow.group != null)
                        (groups as DBGroups).AddWithoutCheck(groupWindow.group);//it adds to the group collection(GroupWindow checks if it's a valid value, hence check is now useless)
                }));
            }
        }
        public CommandClass EditGroup
        {
            get
            {
                return editGroup ??
                (editGroup = new CommandClass(obj =>
                {
                    Group group = new Group((obj as Group).groupNumber, 0);
                    groupWindow = new GroupWindow(group);
                    groupWindow.ShowDialog();
                    if (groupWindow.group != null)
                        (obj as Group).groupNumber = groupWindow.group.groupNumber;
                },(obj) => obj != null));
            }
        }
        public CommandClass DeleteGroup
        {
            get
            {
                return deleteGroup ??
                (deleteGroup = new CommandClass(obj => 
                {
                    (groups as DBGroups).Remove(obj as Group);
                },(obj) => obj != null));
            }
        }

        /*
         * other stuff
         */
        public static void Message(string message)
        {
            //throw new Exception(message);
            MessageBox.Show(message);
        }
        public MainProgram()
        {
            StarterPack();
        }
        public void StarterPack()
        {
            (groups as DBGroups).Add(new Group(391));
            (groups as DBGroups).Add(new Group(392));
            (groups as DBGroups).Add(new Group(371));
            (groups as DBGroups).Add(new Group(372));
            (groups as DBGroups).Add(new Group(351));
            (groups as DBGroups).Add(new Group(491));

            //(groups as DBGroups).RemoveById(1);

            //(persons as DBPersons).Add(new Person("Vlad", "Pahnenko", "Alexandrovich", default));

            (students as DBStudents).Add(new Student("Alex", "Maudza", "Romanovich", (groups as DBGroups)[0]));
            (students as DBStudents).Add(new Student("Misha", "Kadochnikov", "Andreevich", (groups as DBGroups)[0]));
            (students as DBStudents).Add(new Student("David", "Zhidkov", "Sergeevich", (groups as DBGroups)[0]));
            (students as DBStudents).Add(new Student("Vlad", "Pahnenko", "Alexandrovich", (groups as DBGroups)[0]));
            (students as DBStudents).Add(new Student("Artem", "Letych", "Anatolievich", (groups as DBGroups)[0]));
            (students as DBStudents).Add(new Student("Vlad", "Skrishevskiy", "Valdemarovich", (groups as DBGroups)[0], "in the house"));
            (students as DBStudents).Add(new Student("Acakiy", "Laptev", "Acakievich", (groups as DBGroups)[1]));
            (students as DBStudents).Add(new Student("Seraphim", "Tapochkin", "Mihailovich", (groups as DBGroups)[1]));
            (students as DBStudents).Add(new Student("Nikodim", "Polochkin", "Alexandrovich", (groups as DBGroups)[2]));
            (students as DBStudents).Add(new Student("Ricardo", "Milos", "Artiomovich", (groups as DBGroups)[2]));
            (students as DBStudents).Add(new Student("Marpha", "Stulieva", "Davidova", (groups as DBGroups)[4]));
            (students as DBStudents).Add(new Student("David", "Nauoutboukov", "Nicodimovich", (groups as DBGroups)[4]));
            (students as DBStudents).Add(new Student("Vlad", "Artemenko", "Oleksandrovich", (groups as DBGroups)[5]));
            
            (teachers as DBTeachers).Add(new Teacher("o", "hfdg", "hgd"));
            (teachers as DBTeachers).Add(new Teacher("a", "hfdg", "hgd"));
            (teachers as DBTeachers).Add(new Teacher("e", "hhtrfdg", "hgd"));
            (teachers as DBTeachers).Add(new Teacher("i", "hfdg", "hghfdd"));
            (teachers as DBTeachers).Add(new Teacher("u", "hfdg", "hgdrg"));
            
            (subjects as DBSubjects).Add(new Subject("maths"));
            (subjects as DBSubjects).Add(new Subject("programming"));
            (subjects as DBSubjects).Add(new Subject("literature"));
            (subjects as DBSubjects).Add(new Subject("english"));
            (subjects as DBSubjects).Add(new Subject("ukrainian"));
            
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[0], (teachers as DBTeachers)[0]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[0], (teachers as DBTeachers)[1]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[1], (teachers as DBTeachers)[2]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[3], (teachers as DBTeachers)[1]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[4], (teachers as DBTeachers)[1]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[0], (teachers as DBTeachers)[2]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[2], (teachers as DBTeachers)[3]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[3], (teachers as DBTeachers)[4]));
            (teacher_subjects as DBTeacher_Subjects).Add(new Teacher_Subject((subjects as DBSubjects)[4], (teachers as DBTeachers)[4]));
            
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[0], (teacher_subjects as DBTeacher_Subjects)[0], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[0], (teacher_subjects as DBTeacher_Subjects)[6], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[0], (teacher_subjects as DBTeacher_Subjects)[2], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[0], (teacher_subjects as DBTeacher_Subjects)[3], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[0], (teacher_subjects as DBTeacher_Subjects)[4], 56));

            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[1], (teacher_subjects as DBTeacher_Subjects)[1], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[1], (teacher_subjects as DBTeacher_Subjects)[6], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[1], (teacher_subjects as DBTeacher_Subjects)[3], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[1], (teacher_subjects as DBTeacher_Subjects)[2], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject((groups as DBGroups)[1], (teacher_subjects as DBTeacher_Subjects)[4], 32));

            /*Console.WriteLine((students as DBStudents).Count);

            foreach (var elem in teacher_subjects)
                Console.WriteLine(elem);
            foreach (var elem in teachers)
                Console.WriteLine(elem);
            foreach (var elem in students)
                Console.WriteLine(elem);
            foreach (var elem in groups)
                Console.WriteLine(elem);
            foreach (var elem in subjects)
                Console.WriteLine(elem);
            foreach (var elem in group_teacherSubjects)
                Console.WriteLine(elem);
            var markes = from t in marks
                         orderby t.student.id, t.subjectForMarks.id
                         select t;
            foreach (var elem in markes)
                Console.WriteLine(elem);
            foreach (var elem in persons)
                Console.WriteLine(elem);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            //(subjects as DBSubjects).Remove((subjects as DBSubjects)[1]);
            //(students as DBStudents).Remove((students as DBStudents)[2]);

            foreach (var elem in teacher_subjects)
                Console.WriteLine(elem);
            foreach (var elem in teachers)
                Console.WriteLine(elem);
            foreach (var elem in students)
                Console.WriteLine(elem);
            foreach (var elem in groups)
                Console.WriteLine(elem);
            foreach (var elem in subjects)
                Console.WriteLine(elem);
            foreach (var elem in group_teacherSubjects)
                Console.WriteLine(elem);
            foreach (var elem in markes)
                Console.WriteLine(elem);
            foreach (var elem in persons)
                Console.WriteLine(elem);

            var setudents = from t in students
                            group t by t.@group into g
                            select new { grab = g.Key.groupNumber, cunt = g.Count(), sotudents = from s in g select s };
            foreach (var gur in setudents)
            {
                Console.WriteLine($"{gur.grab}: {gur.cunt}");
                foreach (var stod in gur.sotudents)
                {
                    Console.WriteLine($"\t{stod.person.name} {stod.person.surname} {stod.person.patronymic}");
                }
            }

            var techers = from t in teacher_subjects
                          group t by t.teacher into g
                          select new { tocher = g.Key, cunt = g.Count(), subjoct = g };
            foreach (var gor in techers)
            {
                Console.WriteLine($"{gor.tocher.person.name} {gor.tocher.person.surname} {gor.tocher.person.patronymic}: {gor.cunt} subject{(gor.cunt == 1 ? "" : "s")}");
                foreach (var sebject in gor.subjoct)
                {
                    Console.WriteLine($"\t{sebject.subject.subjectName}");
                }
            }*/
        }

        /*
         * a default event for this program to work
         */
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    public class CommandClass : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandClass(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
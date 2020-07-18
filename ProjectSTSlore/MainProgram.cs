using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Reflection;
using System.Data;

namespace ProjectSTSlore
{
    public class MainProgram : INotifyPropertyChanged
    {
        /*
         * a shit ton of collections
         */
        public static DBGroups groups { set; get; }
        public static DBStudents students { set; get; }//this looks terrifying, but it's the only way the program works
        public static DBPersons persons { set; get; }
        public static ObservableCollection<Teacher> teachers { set; get; } = new DBTeachers();
        public static ObservableCollection<Subject> subjects { set; get; } = new DBSubjects();
        public static ObservableCollection<Group_TeacherSubject> group_teacherSubjects { set; get; } = new DBGroup_TeacherSubjects();
        public static ObservableCollection<Teacher_Subject> teacher_subjects { set; get; } = new DBTeacher_Subjects();
        public static ObservableCollection<Marks> marks { set; get; } = new DBMarks();

        public static string homeDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\HumanResourcesDB";
        public static HumanResourcesDBContext DB { get; set; }

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
        //public static Group defaultGroup = new Group(default, "", 0);
        //public static Person defaultPerson = new Person(default, default, default, default, 0);
        //public static Teacher defaultTeacher = new Teacher(defaultPerson, 0);
        //public static Student defaultStudent = new Student(defaultPerson, defaultGroup, 0);

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
                    groupWindow = new GroupWindow(null);//sending default value to class, where it creates copy of it
                    groupWindow.ShowDialog();//and if it's all okay with new group(no exceptions)
                    if (groupWindow.group != null)
                        //(groups as DBGroups).AddWithoutCheck(groupWindow.group);//it adds to the group collection(GroupWindow checks if it's a valid value, hence check is now useless)
                        groups.AddWithoutCheck(groupWindow.group);//it adds to the group collection(GroupWindow checks if it's a valid value, hence check is now useless)
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
                    Group group = new Group((obj as Group).groupNumber, (obj as Group).image, 0);
                    groupWindow = new GroupWindow(group);
                    groupWindow.ShowDialog();
                    if (groupWindow.group != null)
                    {
                        (obj as Group).groupNumber = groupWindow.group.groupNumber;
                        (obj as Group).image = groupWindow.group.image;
                        DB.Groups.Update(obj as Group);
                        DB.SaveChanges();
                    }
                }, (obj) => obj != null));
            }
        }
        public CommandClass DeleteGroup
        {
            get
            {
                return deleteGroup ??
                (deleteGroup = new CommandClass(obj =>
                {
                    groups.Remove(obj as Group);
                }, (obj) => obj != null));
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
            DirectoryCreator();

            File.AppendAllText($"{homeDirectory}\\database\\log.txt", "\n\n\nNew start of application!\n");
            File.AppendAllText($"{homeDirectory}\\database\\log.txt", $"{Directory.GetCurrentDirectory()}\n");
            File.AppendAllText($"{homeDirectory}\\database\\log.txt", $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\HumanResourcesDB.db\n");

            var options = new DbContextOptionsBuilder<HumanResourcesDBContext>()
                .UseSqlite($"Data Source={ Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\HumanResourcesDB.db;")
                .Options;

            DB = new HumanResourcesDBContext(options);
            DB.Groups.Load();
            DB.Students.Load();
            DB.Persons.Load();
            persons = new DBPersons(DB);
            groups = new DBGroups(DB);
            students = new DBStudents(DB);
            if (DB.Groups.Count() == 0)
                StarterPack();
        }
        private void DirectoryCreator()
        {
            //File.Copy($"{ Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\HumanResourcesDB.db", $"{homeDirectory}\\database\\HumanResourcesDB.db", true);
            if (!File.Exists($"{ homeDirectory}\\database\\HumanResourcesDB.db"))
            {
                Console.WriteLine($"This directory doesn't exist yet - {homeDirectory}");
                Directory.CreateDirectory(homeDirectory);
                Directory.CreateDirectory($"{homeDirectory}\\database");
                File.Copy($"{ Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\HumanResourcesDB.db", $"{homeDirectory}\\database\\HumanResourcesDB.db", true);
                Directory.CreateDirectory($"{homeDirectory}\\images");
                Directory.CreateDirectory($"{homeDirectory}\\temp");

                File.AppendAllText($"{homeDirectory}\\database\\log.txt", $"Now it's created - {homeDirectory}\n");
            }

            File.AppendAllText($"{homeDirectory}\\database\\log.txt", $"This is a home directory - {homeDirectory}\n");
        }
        private void StarterPack()
        {
            groups.Add(new Group{ groupNumber = 391/*, image = $"{homeDirectory}\\images\\391.jpg"*/ });
            groups.Add(new Group{ groupNumber = 392});
            groups.Add(new Group{ groupNumber = 371});
            groups.Add(new Group{ groupNumber = 372});
            groups.Add(new Group{ groupNumber = 351});
            groups.Add(new Group{ groupNumber = 491});

            foreach(var elem in groups.Get())
            {
                Console.WriteLine(elem);
            }

            //(groups as DBGroups).RemoveById(1);
            if (DB.Persons.Count() == 0)
            {
                persons.Add(new Person { name = "Alex", surname = "Maudza", patronymic = "Romanovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Misha", surname = "Kadochnikov", patronymic = "Andreevich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "David", surname = "Zhidkov", patronymic = "Sergeevich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Vlad", surname = "Pahnenko", patronymic = "Alexandrovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Artem", surname = "Letych", patronymic = "Anatolievich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Vlad", surname = "Skrishevskiy", patronymic = "Valdemarovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Acakiy", surname = "Laptev", patronymic = "Acakievich", address = "in the house", personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Seraphim", surname = "Tapochkin", patronymic = "Mihailovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Nikodim", surname = "Polochkin", patronymic = "Alexandrovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Ricardo", surname = "Milos", patronymic = "Artiomovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Marpha", surname = "Stulieva", patronymic = "Davidova", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "David", surname = "Nauoutboukov", patronymic = "Nicodimovich", address = default, personRole = PersonRole.NONE });
                persons.Add(new Person { name = "Vlad", surname = "Artemenko", patronymic = "Oleksandrovich", address = default, personRole = PersonRole.NONE });
            }/**/

            students.Add(new Student { person = persons[0], group = groups[0] });
            students.Add(new Student { person = persons[1], group = groups[0] });
            students.Add(new Student { person = persons[2], group = groups[0] });
            students.Add(new Student { person = persons[3], group = groups[0] });
            students.Add(new Student { person = persons[4], group = groups[0] });
            students.Add(new Student { person = persons[5], group = groups[1] });
            students.Add(new Student { person = persons[6], group = groups[1] });
            students.Add(new Student { person = persons[7], group = groups[2] });
            students.Add(new Student { person = persons[8], group = groups[2] });
            students.Add(new Student { person = persons[9], group = groups[2] });
            students.Add(new Student { person = persons[10], group = groups[4] });
            students.Add(new Student { person = persons[11], group = groups[4] });
            students.Add(new Student { person = persons[12], group = groups[5] });/**/

            /*(teachers as DBTeachers).Add(new Teacher("o", "hfdg", "hgd"));
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

            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[0], (teacher_subjects as DBTeacher_Subjects)[0], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[0], (teacher_subjects as DBTeacher_Subjects)[6], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[0], (teacher_subjects as DBTeacher_Subjects)[2], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[0], (teacher_subjects as DBTeacher_Subjects)[3], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[0], (teacher_subjects as DBTeacher_Subjects)[4], 56));

            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[1], (teacher_subjects as DBTeacher_Subjects)[1], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[1], (teacher_subjects as DBTeacher_Subjects)[6], 56));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[1], (teacher_subjects as DBTeacher_Subjects)[3], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[1], (teacher_subjects as DBTeacher_Subjects)[2], 32));
            (group_teacherSubjects as DBGroup_TeacherSubjects).Add(new Group_TeacherSubject(groups[1], (teacher_subjects as DBTeacher_Subjects)[4], 32));*/

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
        public void ChangeProperty([CallerMemberName] string prop = "")
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
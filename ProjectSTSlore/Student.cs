using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public class Student : Entity, INotifyPropertyChanged
    {
        private static uint ID = 0;

        private Group _group;
        private Person _person;
        public Group group
        {
            get { return _group; }
            set
            {
                _group = value;
                ChangeProperty();
            }
        }
        public Person person
        {
            get { return _person; }
            set
            {
                _person = value;
                ChangeProperty();
            }
        }

        public Student(string name, string surname, string patronymic, Group group, string address = default)
        {
            (MainProgram.persons as DBPersons).Add(new Person(name, surname, patronymic, address));
            id = ++ID;
            this.group = group;
            person = (MainProgram.persons as DBPersons).Last();
        }

        public Student(Person person, Group group)
        {
            id = ++ID;
            this.group = group;
            this.person = person;
        }

        public override string ToString()
        {
            return $"Student: name - {person.name}, surname - {person.surname}, patronymic - {person.patronymic}, address - {person.address ?? "none"}, group - {group.groupNumber}, group id - {group.id}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class DBStudents : IDB<Student>
    {
        public DBStudents() : base() { }

        public override void Add(Student newStudent)
        {
            if (newStudent.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add used in database person as student");
                return;
            }
            newStudent.person.personRole = PersonRole.STUDENT;
            base.Add(newStudent);
            var groups = from t in (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)
                         where t.@group == newStudent.@group
                         select t;
            foreach (var group in groups)
            {
                (MainProgram.marks as DBMarks).Add(new Marks(newStudent, group));
            }
        }
        protected override void DeepRemove(Student entity)//удаление студента со всеми его оценками
        {
            entity.person.personRole = PersonRole.NONE;
            for (int i = 0; i < (MainProgram.marks as IDB<Marks>).Count();)
            {
                if ((MainProgram.marks as DBMarks)[i, false].student.id == entity.id)
                {
                    (MainProgram.marks as DBMarks).SoftRemove(i);
                    continue;
                }
                i++;
            }
        }
    }
}

using System.ComponentModel;
using System.Linq;

namespace ProjectSTSlore
{
    public class Student : Entity
    {
        private static int ID = 0;

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

        public Student(string name, string surname, string patronymic, Group group, string address = default, byte id = 1)
        {
            if (id == 0)
            {
                /*Entity.errorMessage("this way is temporary forbidden, use another constructor");
                return;*/
                person = new Person(name, surname, patronymic, address, 0);
                this.group = group;
            }
            else
            {
                MainProgram.persons.Add(new Person { name = name, surname = surname, patronymic = patronymic, address = address });
                this.id = ++ID;
                this.group = group;
                person = MainProgram.persons.Get().Last();
            }
        }

        public Student(Person person, Group group, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.group = group;
            this.person = person;
        }
        public Student() { }

        public override string ToString()
        {
            return $"Student: name - {person.name}, surname - {person.surname}, patronymic - {person.patronymic}, address - {person.address ?? "none"}, group - {group.groupNumber}, group id - {group.id}";
        }
    }

    public class DBStudents : SetDB<Student>
    {
        public DBStudents(HumanResourcesDBContext HRDBContext) : base(HRDBContext, HRDBContext.Students) { }

        public override void AddWithoutCheck(Student newStudent)
        {
            newStudent.person.personRole = PersonRole.STUDENT;
            HRDBContext.Students.Add(newStudent);
            HRDBContext.SaveChanges();
            var groups = from t in MainProgram.group_teacherSubjects.Get()
                         where t.@group == newStudent.@group
                         select t;
            foreach (var group in groups)
            {
                MainProgram.marks.Add(new Marks(newStudent, group));
            }
        }

        public override bool Check(Student newStudent)
        {
            if (newStudent.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add used in database person as student");
                return false;
            }
            return true;
        }
    }
    /*public class DBStudents : IDB<Student>
    {
        public DBStudents() : base() { }

        public override void AddWithoutCheck(Student newStudent)
        {
            newStudent.person.personRole = PersonRole.STUDENT;
            base.AddWithoutCheck(newStudent);
            var groups = from t in (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)
                         where t.@group == newStudent.@group
                         select t;
            foreach (var group in groups)
            {
                (MainProgram.marks as DBMarks).Add(new Marks(newStudent, group));
            }
        }

        public override bool Check(Student newStudent)
        {
            if (newStudent.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add used in database person as student");
                return false;
            }
            return true;
        }

        protected override void DeepRemove(Student entity)//удаление студента со всеми его оценками
        {
            entity.person.personRole = PersonRole.NONE;
            for (int i = 0; i < (MainProgram.marks as IDB<Marks>).Count();)
            {
                if ((MainProgram.marks as DBMarks)[i].student.id == entity.id)
                {
                    (MainProgram.marks as DBMarks).SoftRemove(i);
                    continue;
                }
                i++;
            }
        }
    }*/
}

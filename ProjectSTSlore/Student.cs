using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public class Student : Entity
    {
        private static uint ID = 0;

        public Group group { private set; get; }
        public Person person { private set; get; }

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
    }
    public class DBStudents : IDB<Student>
    {
        public DBStudents() : base() { }

        public override void Add(Student newStudent)
        {
            if (newStudent.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add used person in database as student");
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

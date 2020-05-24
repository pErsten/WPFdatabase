using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public class Teacher : Entity
    {
        private static uint ID = 0;
        public Person person { set; get; }

        public Teacher(string name, string surname, string patronymic, string address = default, bool id = false)
        {
            if (id)
            {
                this.id = 0;
                person = new Person(name, surname, patronymic, address);
            }
            else
            {
                this.id = ++ID;
                (MainProgram.persons as DBPersons).Add(new Person(name, surname, patronymic, address));
                person = (MainProgram.persons as DBPersons).Last();
            }
        }
        public Teacher(Person person, bool id = false)
        {
            this.person = person;
            if (id)
                this.id = 0;
            else
                this.id = ++ID;

        }
        public override string ToString()
        {
            return $"Teacher: name - {person.name}, surname - {person.surname}, patronymic - {person.patronymic}, id - {id}";
        }
    }
    public class DBTeachers : IDB<Teacher>
    {
        public DBTeachers() : base() { }

        public override void Add(Teacher newTeacher)
        {
            if (newTeacher.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add existing teacher");
                return;
            }
            newTeacher.person.personRole = PersonRole.TEACHER;
            base.Add(newTeacher);
        }
        protected override void DeepRemove(Teacher entity)//удаление учителя и замена на учителя по-умолчанию в связанных таблицах
        {
            entity.person.personRole = PersonRole.NONE;
            entity.person = MainProgram.defaultPerson;//не помню для чего это нужно, убери и проверь это как нибудь
            for (int i = 0; i < (MainProgram.teacher_subjects as DBTeacher_Subjects).Count();)
            {
                if ((MainProgram.teacher_subjects as DBTeacher_Subjects)[i, false].teacher.id == entity.id)
                {
                    (MainProgram.teacher_subjects as DBTeacher_Subjects)[i, false].teacher = MainProgram.defaultTeacher;
                    continue;
                }
                i++;
            }
        }
    }
}

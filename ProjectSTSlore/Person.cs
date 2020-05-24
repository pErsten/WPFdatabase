using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public enum PersonRole
    {
        NONE,
        STUDENT,
        TEACHER
    }

    public class Person : Entity
    {
        private static uint ID = 0;
        public string name;
        public string surname;
        public string patronymic;
        public string address;
        public PersonRole personRole = PersonRole.NONE;

        public Person(string name, string surname, string patronymic, string address)
        {
            this.id = ID++;
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.address = address;
        }
        public override string ToString()
        {
            return $"Person: name - {name}, surname - {surname}, patronymic - {patronymic}, address - {address ?? "none"}, role - {personRole}";
        }
    }
    public class DBPersons : IDB<Person>
    {
        public DBPersons() : base() { }

        public override void Add(Person newPerson)
        {
            foreach (Person listedPerson in Items)
                if (listedPerson.surname == newPerson.surname && listedPerson.name == newPerson.name && listedPerson.patronymic == newPerson.patronymic)
                {
                    Entity.errorMessage("Error: trying to add person with the same name, surname and patronymic");
                    return;
                }
            base.Add(newPerson);
        }
        protected override void DeepRemove(Person entity)//удаление человека вместе с классом содержащий его роль (студент или учитель)
        {
            if (entity.personRole == PersonRole.NONE)
                return;
            if (entity.personRole == PersonRole.STUDENT)
                for (int i = 0; i < (MainProgram.students as DBStudents).Count(); i++)
                {
                    if (MainProgram.students[i].person.id == entity.id)
                    {
                        (MainProgram.students as DBStudents).RemoveByIndex(i);
                        return;
                    }
                }
            if (entity.personRole == PersonRole.TEACHER)
                for (int i = 0; i < (MainProgram.teachers as DBTeachers).Count(); i++)
                {
                    if (MainProgram.teachers[i].person.id == entity.id)
                    {
                        (MainProgram.teachers as DBTeachers).RemoveByIndex(i);
                        return;
                    }
                }
        }
    }
}

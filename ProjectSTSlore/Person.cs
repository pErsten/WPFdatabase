using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public enum PersonRole
    {
        NONE,
        STUDENT,
        TEACHER
    }

    public class Person : Entity, INotifyPropertyChanged
    {
        private static uint ID = 0;
        private string _name;
        private string _surname;
        private string _patronymic;
        private string _address;
        private PersonRole _personRole = PersonRole.NONE;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                ChangeProperty();
            }
        }
        public string surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                ChangeProperty();
            }
        }
        public string patronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                ChangeProperty();
            }
        }
        public string address
        {
            get { return _address; }
            set
            {
                _address = value;
                ChangeProperty();
            }
        }
        public PersonRole personRole
        {
            get { return _personRole; }
            set
            {
                _personRole = value;
                ChangeProperty();
            }
        }

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
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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

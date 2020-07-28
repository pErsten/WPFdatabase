using System.ComponentModel;
using System.Linq;

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
        private static int ID = 0;
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

        public Person(string name, string surname, string patronymic, string address, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.address = address;
        }
        public Person() { }

        public override string ToString()
        {
            return $"Person: name - {name}, surname - {surname}, patronymic - {patronymic}, address - {address ?? "none"}, role - {personRole}";
        }
    }

    public class DBPersons : SetDB<Person>
    {
        public DBPersons(HumanResourcesDBContext HRDBContext) : base(HRDBContext) { }

        public override void AddWithoutCheck(Person item)
        {
            HRDBContext.Persons.Add(item);
            HRDBContext.SaveChanges();
        }

        public override Person this[int index]
        {
            get
            {
                if (index >= 0 && index < HRDBContext.Persons.Count())
                    return HRDBContext.Persons.ToList()[index];
                else
                    return null;
            }
            set
            {
                if (index >= 0 && index < HRDBContext.Persons.Count())
                {
                    HRDBContext.Persons.ToList()[index] = value;
                    HRDBContext.SaveChanges();
                }
            }
        }

        public override void SoftRemove(int index)
        {
            HRDBContext.Persons.ToList().RemoveAt(index);
            HRDBContext.SaveChanges();
        }

        public override bool Check(Person newPerson)
        {
            foreach (Person listedPerson in HRDBContext.Persons.ToList())
                if (listedPerson.surname == newPerson.surname && listedPerson.name == newPerson.name && listedPerson.patronymic == newPerson.patronymic)
                {
                    Entity.errorMessage("Error: trying to add person with the same name, surname and patronymic");
                    return false;
                }
            return true;
        }

        /*protected override void DeepRemove(Person entity)//удаление человека вместе с классом содержащий его роль (студент или учитель)
        {
            if (entity.personRole == PersonRole.NONE)
                return;
            if (entity.personRole == PersonRole.STUDENT)
                for (int i = 0; i < MainProgram.students.Get().Count(); i++)
                {
                    if ((MainProgram.students as DBStudents)[i].person.id == entity.id)
                    {
                        (MainProgram.students as DBStudents).Remove((MainProgram.students as DBStudents)[i]);
                        return;
                    }
                }
            if (entity.personRole == PersonRole.TEACHER)
                for (int i = 0; i < (MainProgram.teachers as DBTeachers).Count(); i++)
                {
                    if ((MainProgram.teachers as DBTeachers)[i].person.id == entity.id)
                    {
                        (MainProgram.teachers as DBTeachers).Remove((MainProgram.teachers as DBTeachers)[i]);
                        return;
                    }
                }
        }*/

        public override void Remove(Person item)
        {
            HRDBContext.Persons.Remove(item);
            HRDBContext.SaveChanges();
        }

        public override BindingList<Person> Get()
        {
            return HRDBContext.Persons.Local.ToBindingList();
        }
    }
}

using System;
using System.ComponentModel;
using System.Linq;

namespace ProjectSTSlore
{
    public class Teacher : Entity
    {
        private static int ID = 0;

        private Person _person;
        public Person person
        {
            get { return _person; }
            set
            {
                _person = value;
                ChangeProperty();
            }
        }

        public Teacher(string name, string surname, string patronymic, string address = default, byte id = 1)
        {
            if (id == 0)
            {
                this.id = 0;
                person = new Person(name, surname, patronymic, address, 0);
            }
            else
            {
                this.id = ++ID;
                MainProgram.persons.Add(new Person { name = name, surname = surname, patronymic = patronymic, address = address });
                person = MainProgram.persons.Get().Last();
            }
        }
        public Teacher(Person Person, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.person = Person;

        }
        public Teacher() { }

        public override string ToString()
        {
            return $"Teacher: name - {person.name}, surname - {person.surname}, patronymic - {person.patronymic}, id - {id}";
        }
    }

    public class DBTeachers : SetDB<Teacher>
    {
        public DBTeachers(HumanResourcesDBContext HRDBContext) : base(HRDBContext) { }

        public override Teacher this[int index]
        {
            get
            {
                if (index >= 0 && index < HRDBContext.Teachers.Count())
                    return HRDBContext.Teachers.ToList()[index];
                else
                    return null;
            }
            set
            {
                if (index >= 0 && index < HRDBContext.Teachers.Count())
                {
                    HRDBContext.Teachers.ToList()[index] = value;
                    HRDBContext.SaveChanges();
                }
            }
        }

        public override void AddWithoutCheck(Teacher newTeacher)
        {
            HRDBContext.Teachers.Add(newTeacher);
            HRDBContext.SaveChanges();
        }

        public override bool Check(Teacher newTeacher)
        {
            if (newTeacher.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add existing teacher");
                return false;
            }
            return true;
        }

        public override void Remove(Teacher item)
        {
            HRDBContext.Teachers.Remove(item);
            HRDBContext.SaveChanges();
        }

        public override void SoftRemove(int index)
        {
            HRDBContext.Teachers.ToList().RemoveAt(index);
            HRDBContext.SaveChanges();
        }

        /*protected override void DeepRemove(Student entity)//удаление студента со всеми его оценками
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
        }*/

        public override BindingList<Teacher> Get()
        {
            return HRDBContext.Teachers.Local.ToBindingList();
        }
    }

    /*public class DBTeachers : IDB<Teacher>
    {
        public DBTeachers() : base() { }

        public override void AddWithoutCheck(Teacher newTeacher)
        {
            newTeacher.person.personRole = PersonRole.TEACHER;
            base.AddWithoutCheck(newTeacher);
        }

        public override bool Check(Teacher newTeacher)
        {
            if (newTeacher.person.personRole != PersonRole.NONE)
            {
                Entity.errorMessage("Error: trying to add existing teacher");
                return false;
            }
            return true;
        }

        protected override void DeepRemove(Teacher entity)//удаление учителя и замена на учителя по-умолчанию в связанных таблицах
        {
            entity.person.personRole = PersonRole.NONE;
            //entity.person = null;//не помню для чего это нужно, убери и проверь это как нибудь
            for (int i = 0; i < (MainProgram.teacher_subjects as DBTeacher_Subjects).Count();)
            {
                if ((MainProgram.teacher_subjects as DBTeacher_Subjects)[i].teacher.id == entity.id)
                {
                    (MainProgram.teacher_subjects as DBTeacher_Subjects)[i].teacher = null;
                    continue;
                }
                i++;
            }
        }
    }*/
}

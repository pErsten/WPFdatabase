using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ProjectSTSlore
{
    public class Marks : Entity
    {
        public static int ID = 0;
        public int markPosition;

        private List<byte> _marksList;
        private Student _student;
        private Group_TeacherSubject _subjectForMarks;
        [NotMapped]
        public List<byte> marksList
        {
            get { return _marksList; }
            set
            {
                _marksList = value;
                ChangeProperty();
            }
        }
        public Student student
        {
            get { return _student; }
            set
            {
                _student = value;
                ChangeProperty();
            }
        }
        public Group_TeacherSubject subjectForMarks
        {
            get { return _subjectForMarks; }
            set
            {
                _subjectForMarks = value;
                ChangeProperty();
            }
        }

        public Marks(Student student, Group_TeacherSubject subjectForMarks, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            markPosition = 0;
            this.student = student;
            this.subjectForMarks = subjectForMarks;
            marksList = new List<byte>(subjectForMarks.hours ?? 0);//пока кол-во оценок прямо зависит от часов отведенного обучению предмета
        }
        public Marks() { }

        public void AddMark(byte mark)//добавляем оценку в конец списка
        {
            if (markPosition < subjectForMarks.hours)
                marksList[markPosition++] = mark;
            else
                errorMessage("Error: already have maximum amount of marks");
        }
        public void AddMark(byte mark, int position)//добавляем оценку на определенную позицию
        {
            if (position >= markPosition && position < subjectForMarks.hours)//если после позиции нет оценок - автозаполнение будет происходить после этого места
            {
                marksList[position] = mark;
                markPosition = ++position;
            }
            else if (position < markPosition && position >= 0)//если после оценки есть - позиция автозаполнения не меняется
                marksList[position] = mark;
            else
                errorMessage("Error: wrong position of mark");

        }
        public override string ToString()
        {
            return $"Marks: student name and surname - {student.person.name} {student.person.surname}, student id - {student.id}, chain id - {subjectForMarks.id}, subject id - {subjectForMarks.teacherSubject.subject.id}, subject name - {subjectForMarks.teacherSubject.subject.subjectName}";
        }
    }

    public class DBMarks : SetDB<Marks>
    {
        public DBMarks(HumanResourcesDBContext HRDBContext) : base(HRDBContext) { }

        public override Marks this[int index]
        {
            get
            {
                if (index >= 0 && index < HRDBContext.Marks.Count())
                    return HRDBContext.Marks.ToList()[index];
                else
                    return null;
            }
            set
            {
                if (index >= 0 && index < HRDBContext.Marks.Count())
                {
                    HRDBContext.Marks.ToList()[index] = value;
                    HRDBContext.SaveChanges();
                }
            }
        }

        public override void AddWithoutCheck(Marks newMarks)
        {
            HRDBContext.Marks.Add(newMarks);
            HRDBContext.SaveChanges();
        }
        public override bool Check(Marks newMarks)
        {
            return true;
        }

        public override BindingList<Marks> Get()
        {
            return HRDBContext.Marks.Local.ToBindingList();
        }

        public override void Remove(Marks item)
        {
            HRDBContext.Marks.Remove(item);
            HRDBContext.SaveChanges();
        }

        public override void SoftRemove(int index)
        {
            HRDBContext.Marks.ToList().RemoveAt(index);
            HRDBContext.SaveChanges();
        }

        /*protected override void DeepRemove(Marks entity)
        {
            Entity.errorMessage("you shouldn't be there");
        }*/
    }
    /*public class DBMarks : IDB<Marks>
    {
        public DBMarks() : base() { }

        public override void AddWithoutCheck(Marks newMarks)
        {
            base.AddWithoutCheck(newMarks);
        }
        public override bool Check(Marks newMarks)
        {
            return true;
        }
        protected override void DeepRemove(Marks entity)
        {
            Entity.errorMessage("you shouldn't be there");
        }
    }*/
}

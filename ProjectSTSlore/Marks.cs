using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSTSlore
{
    public class Marks : Entity
    {
        public static uint ID = 0;
        public int markPosition;

        public List<byte> marksList { private set; get; }
        public Student student { private set; get; }
        public Group_TeacherSubject subjectForMarks { private set; get; }

        public Marks(Student student, Group_TeacherSubject subjectForMarks)
        {
            id = ++ID;
            markPosition = 0;
            this.student = student;
            this.subjectForMarks = subjectForMarks;
            marksList = new List<byte>(subjectForMarks.hours);//пока кол-во оценок прямо зависит от часов отведенного обучению предмета
        }
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
    public class DBMarks : IDB<Marks>
    {
        public DBMarks() : base() { }

        public override void Add(Marks newMarks)
        {
            base.Add(newMarks);
        }
        protected override void DeepRemove(Marks entity)
        {
            throw new Exception("you shouldn't be there");
        }
    }
}

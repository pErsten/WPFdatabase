using System.Linq;

namespace ProjectSTSlore
{
    public class Teacher_Subject : Entity
    {
        private static int ID = 0;

        private Subject _subject;
        private Teacher _teacher;
        public Subject subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                ChangeProperty();
            }
        }
        public Teacher teacher
        {
            get { return _teacher; }
            set
            {
                _teacher = value;
                ChangeProperty();
            }
        }

        public Teacher_Subject(Subject subject, Teacher teacher, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.subject = subject;
            this.teacher = teacher;
        }
        public Teacher_Subject() { }

        public override string ToString()
        {
            return $"Teacher-Subject chain: id - {id}, id of subject - {subject.id}, id of teacher - {teacher.id}";
        }
    }

    public class DBTeacher_Subjects : IDB<Teacher_Subject>
    {
        public DBTeacher_Subjects() : base() { }

        public override void AddWithoutCheck(Teacher_Subject newTeacher_Subject)
        {
            base.AddWithoutCheck(newTeacher_Subject);
        }

        public override bool Check(Teacher_Subject newTeacher_Subject)
        {
            foreach (Teacher_Subject listedTeacher_Subject in Items)
                if (listedTeacher_Subject.subject.id == newTeacher_Subject.subject.id && listedTeacher_Subject.teacher.id == newTeacher_Subject.teacher.id)
                {
                    Entity.errorMessage("Error: trying to add exisiting chain of teachers and subjects");
                    return false;
                }
            return true;
        }

        protected override void DeepRemove(Teacher_Subject entity)//удаление связи предмета с учителем и замена на учителя по-умолчанию в связанной таблице
        {
            for (int i = 0; i < (MainProgram.teacher_subjects as DBTeacher_Subjects).Count(); i++)
            {
                if ((MainProgram.teacher_subjects as DBTeacher_Subjects)[i].id == entity.id)
                {
                    (MainProgram.teacher_subjects as DBTeacher_Subjects)[i].teacher = null;
                    break;
                }
            }
        }
    }
}

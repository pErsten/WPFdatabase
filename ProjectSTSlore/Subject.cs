using System.ComponentModel;
using System.Linq;

namespace ProjectSTSlore
{
    public class Subject : Entity
    {
        private static int ID = 0;

        private string _subjectName;
        public string subjectName
        {
            get { return _subjectName; }
            set
            {
                _subjectName = value;
                ChangeProperty();
            }
        }

        public Subject(string subjectName, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.subjectName = subjectName;
        }
        public Subject() { }

        public override string ToString()
        {
            return $"Subject: name - {subjectName}, id - {id}";
        }
    }

    public class DBSubjects : SetDB<Subject>
    {
        public DBSubjects(HumanResourcesDBContext HRDBContext) : base(HRDBContext, HRDBContext.Subjects) { }

        public override bool Check(Subject newSubject)
        {
            foreach (Subject listedSubject in HRDBContext.Subjects.ToList())
                if (listedSubject.subjectName == newSubject.subjectName)
                {
                    Entity.errorMessage("Error: trying to add exisiting subject");
                    return false;
                }
            return true;
        }
    }
    /*public class DBSubjects : IDB<Subject>
    {
        public DBSubjects() : base() { }

        public override void AddWithoutCheck(Subject newSubject)
        {
            base.AddWithoutCheck(newSubject);
        }

        public override bool Check(Subject newSubject)
        {
            foreach (Subject listedSubject in Items)
                if (listedSubject.subjectName == newSubject.subjectName)
                {
                    Entity.errorMessage("Error: trying to add exisiting subject");
                    return false;
                }
            return true;
        }

        protected override void DeepRemove(Subject entity)//удаление предмета, и всего того, что связано с этим предметом
        {
            for (int j = 0; j < (MainProgram.teacher_subjects as DBTeacher_Subjects).Count();)
            {
                if ((MainProgram.teacher_subjects as DBTeacher_Subjects)[j].subject.id == entity.id)
                {
                    for (int k = 0; k < (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).Count();)
                    {
                        if ((MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)[k].teacherSubject.id == (MainProgram.teacher_subjects as DBTeacher_Subjects)[j].id)
                        {
                            (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).Remove((MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)[k]);
                            continue;
                        }
                        k++;
                    }
                    (MainProgram.teacher_subjects as DBTeacher_Subjects).SoftRemove(j);
                    continue;
                }
                j++;
            }
        }
    }*/
}

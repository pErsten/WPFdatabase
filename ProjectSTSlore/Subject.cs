using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public class Subject : Entity, INotifyPropertyChanged
    {
        private static uint ID = 0;

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

        public Subject(string subjectName)
        {
            id = ++ID;
            this.subjectName = subjectName;
        }
        public override string ToString()
        {
            return $"Subject: name - {subjectName}, id - {id}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class DBSubjects : IDB<Subject>
    {
        public DBSubjects() : base() { }

        public override void Add(Subject newSubject)
        {
            foreach (Subject listedSubject in Items)
                if (listedSubject.subjectName == newSubject.subjectName)
                {
                    Entity.errorMessage("Error: trying to add exisiting subject");
                    return;
                }
            base.Add(newSubject);
        }
        protected override void DeepRemove(Subject entity)//удаление предмета, и всего того, что связано с этим предметом
        {
            for (int j = 0; j < (MainProgram.teacher_subjects as DBTeacher_Subjects).Count();)
            {
                if ((MainProgram.teacher_subjects as DBTeacher_Subjects)[j, false].subject.id == entity.id)
                {
                    for (int k = 0; k < (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).Count();)
                    {
                        if ((MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)[k, false].teacherSubject.id == (MainProgram.teacher_subjects as DBTeacher_Subjects)[j, false].id)
                        {
                            (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).RemoveByIndex(k);
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
    }
}

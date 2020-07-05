using System.Linq;

namespace ProjectSTSlore
{
    public class Group_TeacherSubject : Entity
    {
        private static int ID = 0;

        private Teacher_Subject _teacherSubject;
        private Group _group;
        private byte? _hours;
        public Teacher_Subject teacherSubject
        {
            get { return _teacherSubject; }
            set
            {
                _teacherSubject = value;
                ChangeProperty();
            }
        }
        public Group group
        {
            get { return _group; }
            set
            {
                _group = value;
                ChangeProperty();
            }
        }
        public byte? hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                ChangeProperty();
            }
        }

        public Group_TeacherSubject(Group group, Teacher_Subject teacherSubject, byte? hours, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.group = group;
            this.teacherSubject = teacherSubject;
            this.hours = hours;
        }
        public Group_TeacherSubject() { }
        public override string ToString()
        {
            return $"Group_TeacherSubject chain: id - {id}, id of teacher-subject chain - {teacherSubject.id}, id of teacher - {teacherSubject.teacher.id}, id of subject - {teacherSubject.subject.id}, id of group - {group.id}, quantity of study hours for this group - {hours}";
        }
    }

    public class DBGroup_TeacherSubjects : IDB<Group_TeacherSubject>
    {
        public DBGroup_TeacherSubjects() : base() { }

        public override void AddWithoutCheck(Group_TeacherSubject newGroup_TeacherSubject)
        {
            base.AddWithoutCheck(newGroup_TeacherSubject);
            var students = from t in MainProgram.students.Get()
                           where t.@group == newGroup_TeacherSubject.@group
                           select t;
            foreach (var student in students)
            {
                (MainProgram.marks as DBMarks).Add(new Marks(student, newGroup_TeacherSubject));
            }
        }

        public override bool Check(Group_TeacherSubject newGroup_TeacherSubject)
        {
            foreach (Group_TeacherSubject listedGroup_TeacherSubject in Items)
                if (listedGroup_TeacherSubject.group.id == newGroup_TeacherSubject.group.id && listedGroup_TeacherSubject.teacherSubject.subject.id == newGroup_TeacherSubject.teacherSubject.subject.id)
                {
                    Entity.errorMessage("Error: trying to add two same subjects to the same group");
                    return false;
                }
            return true;
        }

        protected override void DeepRemove(Group_TeacherSubject entity)//удаление предмета в группе, вместе со всеми оценками
        {
            for (int i = 0; i < (MainProgram.marks as DBMarks).Count();)
            {
                if ((MainProgram.marks as DBMarks)[i].subjectForMarks.id == entity.id)
                {
                    (MainProgram.marks as DBMarks).SoftRemove(i);
                    continue;
                }
                i++;
            }
        }
    }
}

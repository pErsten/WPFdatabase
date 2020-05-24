using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public class Group_TeacherSubject : Entity
    {
        private static uint ID = 0;

        public Teacher_Subject teacherSubject { private set; get; }
        public Group group { private set; get; }
        public byte hours { private set; get; }
        public Group_TeacherSubject(Group group, Teacher_Subject teacherSubject, byte hours)
        {
            id = ++ID;
            this.group = group;
            this.teacherSubject = teacherSubject;
            this.hours = hours;
        }
        public override string ToString()
        {
            return $"Group_TeacherSubject chain: id - {id}, id of teacher-subject chain - {teacherSubject.id}, id of teacher - {teacherSubject.teacher.id}, id of subject - {teacherSubject.subject.id}, id of group - {group.id}, quantity of study hours for this group - {hours}";
        }
    }
    public class DBGroup_TeacherSubjects : IDB<Group_TeacherSubject>
    {
        public DBGroup_TeacherSubjects() : base() { }

        public override void Add(Group_TeacherSubject newGroup_TeacherSubject)
        {
            foreach (Group_TeacherSubject listedGroup_TeacherSubject in Items)
                if (listedGroup_TeacherSubject.group.id == newGroup_TeacherSubject.group.id && listedGroup_TeacherSubject.teacherSubject.subject.id == newGroup_TeacherSubject.teacherSubject.subject.id)
                {
                    Entity.errorMessage("Error: trying to add two same subjects to the same group");
                    return;
                }
            base.Add(newGroup_TeacherSubject);
            var students = from t in (MainProgram.students as DBStudents)
                           where t.@group == newGroup_TeacherSubject.@group
                           select t;
            foreach (var student in students)
            {
                (MainProgram.marks as DBMarks).Add(new Marks(student, newGroup_TeacherSubject));
            }
        }
        protected override void DeepRemove(Group_TeacherSubject entity)//удаление предмета в группе, вместе со всеми оценками
        {
            for (int i = 0; i < (MainProgram.marks as DBMarks).Count();)
            {
                if ((MainProgram.marks as DBMarks)[i, false].subjectForMarks.id == entity.id)
                {
                    (MainProgram.marks as DBMarks).SoftRemove(i);
                    continue;
                }
                i++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public class Group : Entity
    {
        private static uint ID = 0;

        public int groupNumber { get; private set; }

        public Group(int groupNumber)
        {
            id = ++ID;
            this.groupNumber = groupNumber;
        }
        public override string ToString()
        {
            return $"Group: group - {groupNumber}, id - {id}";
        }
    }
    public class DBGroups : IDB<Group>
    {
        public DBGroups() : base() { }

        public override void Add(Group newGroup)
        {
            foreach (Group listedGroup in Items)
                if (listedGroup.groupNumber == newGroup.groupNumber)
                {
                    Entity.errorMessage("Error: trying to add already existing group");
                    return;
                }
            base.Add(newGroup);
        }
        protected override void DeepRemove(Group entity)
        {
            for (int i = 0; i < (MainProgram.students as DBStudents).Count();)
            {
                if ((MainProgram.students as DBStudents)[i, false].group.id == entity.id)
                {
                    (MainProgram.students as DBStudents).RemoveByIndex(i);//удаление студентов с оценками
                    continue;
                }
                i++;
            }
            for (int i = 0; i < (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).Count();)
            {
                if ((MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)[i, false].group.id == entity.id)
                {
                    (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).SoftRemove(i);//удаление только связей, так как все связанные оценки уже удалены прошлым циклом
                    continue;
                }
                i++;
            }
        }
    }
}

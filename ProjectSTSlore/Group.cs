using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public class Group : Entity
    {
        private static uint ID = 0;

        private int _groupNumber;
        public int groupNumber
        {
            get { return _groupNumber; }
            set
            {
                _groupNumber = value;
                ChangeProperty();
            }
        }

        public Group(int groupNumber, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
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

        public override void AddWithoutCheck(Group newGroup)
        {
            base.AddWithoutCheck(newGroup);
        }
        public override bool Check(Group newGroup)
        {
            foreach (Group listedGroup in Items)
                if (listedGroup.groupNumber == newGroup.groupNumber)
                {
                    Entity.errorMessage("Error: trying to add already existing group");
                    return false;
                }
            return true;
        }
        protected override void DeepRemove(Group entity)
        {
            for (int i = 0; i < (MainProgram.students as DBStudents).Count();)
            {
                if ((MainProgram.students as DBStudents)[i].group.id == entity.id)
                {
                    (MainProgram.students as DBStudents).Remove((MainProgram.students as DBStudents)[i]);//удаление студентов с оценками
                    continue;
                }
                i++;
            }
            for (int i = 0; i < (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).Count();)
            {
                if ((MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects)[i].group.id == entity.id)
                {
                    (MainProgram.group_teacherSubjects as DBGroup_TeacherSubjects).SoftRemove(i);//удаление только связей, так как все связанные оценки уже удалены прошлым циклом
                    continue;
                }
                i++;
            }
        }
    }
}

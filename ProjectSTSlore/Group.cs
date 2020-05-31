using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public class Group : Entity, INotifyPropertyChanged
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

        public Group(int groupNumber)
        {
            id = ++ID;
            this.groupNumber = groupNumber;
        }
        public override string ToString()
        {
            return $"Group: group - {groupNumber}, id - {id}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class DBGroups : IDB<Group>
    {
        public DBGroups() : base() { }

        public override void Add(Group newGroup)
        {
            if (!Check(newGroup)) return;
            base.Add(newGroup);
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

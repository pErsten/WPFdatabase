using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Documents;

namespace ProjectSTSlore
{
    public class Group : Entity
    {
        private static int ID = 0;

        private int? _groupNumber;
        private byte[] _image;
        public int? groupNumber
        {
            get { return _groupNumber; }
            set
            {
                _groupNumber = value;
                ChangeProperty();
            }
        }
        [Column(TypeName = "blob")]
        public byte[] image
        {
            get { return _image; }
            set
            {
                _image = value;
                ChangeProperty();
            }
        }

        public Group(int? groupNumber, byte[] image = null, byte id = 1)
        {
            if (id == 0)
                this.id = 0;
            else
                this.id = ++ID;
            this.groupNumber = groupNumber;
            this.image = image;
        }
        public Group() { }

        public override string ToString()
        {
            return $"Group: group - {groupNumber}, id - {id}";
        }
    }

    public class DBGroups : SetDB<Group>
    {
        public DBGroups(HumanResourcesDBContext HRDBContext) : base(HRDBContext)
        {
            Console.WriteLine("groups established");
        }

        public override void AddWithoutCheck(Group item)
        {
            HRDBContext.Groups.Add(item);
            HRDBContext.SaveChanges();
        }

        public override Group this[int index]
        {
            get
            {
                if (index >= 0 && index < HRDBContext.Groups.Count())
                    return HRDBContext.Groups.ToList()[index];
                else
                    return null;
            }
            set
            {
                if (index >= 0 && index < HRDBContext.Groups.Count())
                {
                    HRDBContext.Groups.ToList()[index] = value;
                    HRDBContext.SaveChanges();
                }
            }
        }

        public override void SoftRemove(int index)
        {
            HRDBContext.Groups.ToList().RemoveAt(index);
            HRDBContext.SaveChanges();
        }

        public override bool Check(Group newGroup)
        {
            foreach (Group listedGroup in HRDBContext.Groups.ToList())
                if (listedGroup.groupNumber == newGroup.groupNumber)
                {
                    Entity.errorMessage("Error: trying to add already existing group");
                    return false;
                }
            return true;
        }
        protected override void DeepRemove(Group entity)
        {
            for (int i = 0; i < MainProgram.students.Get().Count();)
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

        public override void Remove(Group item)
        {
            HRDBContext.Groups.Remove(item);
            HRDBContext.SaveChanges();
        }

        public override BindingList<Group> Get()
        {
            return HRDBContext.Groups.Local.ToBindingList();
        }
    }
    /*  public DBGroups() : base() { }

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
    }*/
}

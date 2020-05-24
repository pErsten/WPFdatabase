using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ProjectSTSlore
{
    public abstract class Entity
    {
        public delegate void StringDelegate(string message);
        public static StringDelegate errorMessage = MainProgram.Message;

        public uint id { get; protected set; }
        public abstract override string ToString();
    }
    public abstract class IDB<T> : ObservableCollection<T> where T : Entity
    {

        //protected List<T> objects = null;
        protected IDB()
        {
            //this.objects = new List<T>();
        }
        public virtual new void Add(T item)
        {
            base.Add(item);
        }
        private int BinarySearchById(int id)
        {
            if (id > Items.Last().id)
                return -1;
            int startPoint = 0;
            int endPoint = Items.Count();
            while (startPoint <= endPoint)
            {
                if (Items[(endPoint + startPoint) / 2].id == id)
                    return (endPoint + startPoint) / 2;
                else if (Items[(endPoint + startPoint) / 2].id > id)
                {
                    endPoint = (endPoint + startPoint) / 2;
                    endPoint--;
                    continue;
                }
                else
                {
                    startPoint = (endPoint + startPoint) / 2;
                    startPoint++;
                    continue;
                }
            }
            return -1;
        }
        public T this[int index, bool idNumber = true]
        {
            get
            {
                if (idNumber)
                {
                    index = BinarySearchById(index);
                    if (index != -1)
                        return Items[index];
                    else
                    {
                        Entity.errorMessage($"Error: there aren't any Items in {typeof(T)} with such id");
                        return null;
                    }
                }
                else if (index >= 0 && index < Items.Count)
                    return Items[index];
                else
                    return null;
            }
            set
            {
                if (idNumber)
                {
                    index = BinarySearchById(index);
                    if (index != -1)
                        Items[index] = value;
                }
                else if (index >= 0 && index < Items.Count)
                    Items[index] = value;
            }
        }
        public void SoftRemove(int index)
        {
            Items.RemoveAt(index);
        }
        protected abstract void DeepRemove(T entity);
        public void RemoveById(int id)
        {
            if (id > 0 && id <= Items.Last().id)
            {
                int index = BinarySearchById(id);
                if (index != -1)
                {
                    DeepRemove(Items[index]);
                    Items.RemoveAt(index);
                    return;
                }
            }
            Entity.errorMessage("Error: trying to delete wrong object");
        }
        public void RemoveByIndex(int index)
        {
            if (index >= 0 && index < Items.Count())
            {
                DeepRemove(Items[index]);
                Items.RemoveAt(index);
            }
            else
                Entity.errorMessage("Error: trying to delete wrong object" + Items.Count());
        }
    }
}

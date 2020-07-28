﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectSTSlore
{
    public abstract class Entity : INotifyPropertyChanged, IDisposable
    {
        public delegate void StringDelegate(string message);
        public static StringDelegate errorMessage = MainProgram.Message;

        [Key]
        public int id { get; set; }

        public abstract override string ToString();
        public void Dispose()
        {
            Console.WriteLine("item disposed");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeProperty([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public abstract class SetDB<T> //TODO: make this as the realisation from IEnumerator for binding purposes
    {
        public HumanResourcesDBContext HRDBContext;

        protected SetDB(HumanResourcesDBContext HRDBContext)
        {
            this.HRDBContext = HRDBContext;
        }
        public void Add(T item)
        {
            if (!Check(item)) return;
            AddWithoutCheck(item);
        }

        public abstract void AddWithoutCheck(T item);
        /* Realisation of this method should look like this:
         * 
           {
              HRDBContext.[Name_of_DbSet].Add(item);
              HRDBContext.SaveChanges();
           }
         *
         */

        public abstract bool Check(T newItem);

        public abstract T this[int index] { get;set; }
        /* Realisation of this method should look like this:
         * 
           {
               get
               {
                   if (index >= 0 && index < HRDBContext.[Name_of_DbSet].Count())
                       return HRDBContext.[Name_of_DbSet].ToList()[index];
                   else
                       return null;
               }
               set
               {
                   if (index >= 0 && index < HRDBContext.[Name_of_DbSet].Count())
                   {
                       HRDBContext.[Name_of_DbSet].ToList()[index] = value;
                       HRDBContext.SaveChanges();
                   }
               }
           }
         *
         */

        public abstract void SoftRemove(int index);
        /* Realisation of this method should look like this:
         * 
           {
               HRDBContext.[Name_of_DbSet].ToList().RemoveAt(index);
               HRDBContext.SaveChanges();
           }
         *
         */

        //protected abstract void DeepRemove(T entity);

        public abstract void Remove(T item);
        /* Realisation of this method should look like this:
         *
             {
                 HRDBContext.[Name_of_DbSet].Remove(item);
                 HRDBContext.SaveChanges();
             }
         * 
         */

        public abstract BindingList<T> Get();
        /* Realisation of this method should look like this:
         *
             {
                 return HRDBContext.[Name_of_DbSet].Local.ToBindingList();
             }
         * 
         */
    }
    public abstract class IDB<T> : ObservableCollection<T> where T : Entity
    {
        protected IDB() { }
        public new void Add(T item)
        {
            if (!Check(item)) return;
            AddWithoutCheck(item);
        }
        public virtual void AddWithoutCheck(T item)
        {
            base.Add(item);
        }

        public abstract bool Check(T newItem);

        public new T this[int index]
        {
            get
            {
                /*bool idNumber = false;
                if (idNumber)
                {
                    Console.WriteLine("Error: addressing by id");
                    index = BinarySearchById(index);
                    if (index != -1)
                        return Items[index];
                    else
                    {
                        Entity.errorMessage($"Error: there aren't any Items in {typeof(T)} with such id");
                        return null;
                    }
                }
                else*/
                if (index >= 0 && index < Items.Count)
                    return Items[index];
                else
                    return null;
            }
            set
            {
                /*bool idNumber = false;
                if (idNumber)
                {
                    index = BinarySearchById(index);
                    if (index != -1)
                        Items[index] = value;
                }
                else */
                if (index >= 0 && index < Items.Count)
                    Items[index] = value;
            }
        }

        public virtual void SoftRemove(int index)
        {
            Items.RemoveAt(index);
        }

        protected abstract void DeepRemove(T entity);

        public new virtual void Remove(T item)
        {
            try
            {
                DeepRemove(item);
            }
            catch
            {
                Entity.errorMessage("Error: trying to delete wrong object" + Items.Count());
            }
        }
        /*public new void Remove(T item)
        {
            try
            {
                DeepRemove(item);
                base.Remove(item);
            }
            catch
            {
                Entity.errorMessage("Error: trying to delete wrong object" + Items.Count());
            }
        }*/
        /*public void RemoveByIndex(int index)
        {
            if (index >= 0 && index < Items.Count())
            {
                DeepRemove(Items[index]);
                Items.RemoveAt(index);
            }
            else
                Entity.errorMessage("Error: trying to delete wrong object" + Items.Count());
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
        }*/
    }
}

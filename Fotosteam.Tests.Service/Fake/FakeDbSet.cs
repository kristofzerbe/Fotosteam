using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Tests.Service.Fake
{
    public class FakeDbSet<T> : DbSet<T>, IDbSet<T> where T : class
    {
        private readonly HashSet<T> Data;

        public FakeDbSet()
        {
            Data = new HashSet<T>();
        }

        public virtual new T Find(params object[] keyValues)
        {

            var keyProperty = typeof(T).GetProperty(
         "Id",
         BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            var result = this.SingleOrDefault(obj =>
                keyProperty.GetValue(obj).ToString() == keyValues.First().ToString());
            return result;
        }

        private static int _lastId;
        public override T Add(T item)
        {
            var baseItem = (item as PocoBase);
            if (baseItem != null && baseItem.Id == 0)
            {
                if (_lastId == 0)
                {
                    foreach (var element in Data)
                    {
                        var id = (element as PocoBase).Id;
                        if (_lastId < id)
                            _lastId = id;
                    }
                }
                _lastId++;
                baseItem.Id = _lastId;
            }
            Data.Add(item);
            return item;
        }

        private static object Lock = new object();
        public override T Remove(T item)
        {
            lock (Lock)
            {
                if (item is PocoBase)
                {
                    var id = (item as PocoBase).Id;
                    foreach (var element in Data)
                    {
                        if ((element as PocoBase).Id == id)
                        {
                            Data.Remove(element);
                            return element;
                        }
                    }

                }
                if (item is ExifData)
                {
                    var id = (item as ExifData).PhotoId;
                    foreach (var element in Data)
                    {
                        if ((element as ExifData).PhotoId == id)
                        {
                            Data.Remove(element);
                            return element;
                        }
                    }

                }
                Data.Remove(item);
                return item;
            }
        }

        public override T Attach(T item)
        {
            lock (Lock)
            {
                Data.Add(item);
                return item;
            }
        }

        public void Detach(T item)
        {
            lock (Lock)
            {
                Data.Remove(item);
            }
        }

        Type IQueryable.ElementType
        {
            get
            {
                lock (Lock)
                {
                    return Data.AsQueryable().ElementType;
                }
            }
        }

        Expression IQueryable.Expression
        {
            get
            {
                lock (Lock)
                {
                    return Data.AsQueryable().Expression;
                }
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                lock (Lock)
                {
                    return Data.AsQueryable().Provider;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Lock)
            {
                return Data.GetEnumerator();
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (Lock)
            {
                return Data.GetEnumerator();
            }
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override ObservableCollection<T> Local
        {
            get
            {
                lock (Lock)
                {
                    return new ObservableCollection<T>(Data);
                }
            }
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
    }
}
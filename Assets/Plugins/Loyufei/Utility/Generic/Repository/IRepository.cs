using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IRepository
    {
        public int Capacity { get; }

        public IReposit Search(Func<IReposit, bool> condition);

        public IReposit SearchAt(int index);
        
        public IReposit SearchAt(object identify);
        
        public IEnumerable<IReposit> SearchAll();

        public IEnumerable<IReposit> SearchAll(Func<IReposit, bool> condition);

        public void Sort(Comparison<IReposit> comparison);
    }

    public interface IRepository<TData> : IRepository 
    {
        public IReposit<TData> Search(Func<IReposit<TData>, bool> condition);
        
        public new IReposit<TData> SearchAt(int index);

        public new IReposit<TData> SearchAt(object identify);

        public new IEnumerable<IReposit<TData>> SearchAll();

        public IEnumerable<IReposit<TData>> SearchAll(Func<IReposit<TData>, bool> condition);

        public void Sort(Comparison<IReposit<TData>> comparison);

        IReposit IRepository.Search(Func<IReposit, bool> condition) 
            => Search((reposit) => condition.Invoke(reposit));
        IReposit IRepository.SearchAt(int index)
            => SearchAt(index);
        IReposit IRepository.SearchAt(object identify)
            => SearchAt(identify);
        IEnumerable<IReposit> IRepository.SearchAll()
            => SearchAll();
        IEnumerable<IReposit> IRepository.SearchAll(Func<IReposit, bool> condition)
            => SearchAll((reposit) => condition.Invoke(reposit)).OfType<IReposit>();
        void IRepository.Sort(Comparison<IReposit> comparison)
            => Sort((r1, r2) => comparison.Invoke(r1, r2));
    }

    public interface IFlexibleRepository : IRepository 
    {
        public bool Limited { get; }

        public IEnumerable<IReposit> Create(int amount);
    }

    public interface IFlexibleRepository<TData> : IFlexibleRepository, IRepository<TData>
    {
        public new IEnumerable<IReposit<TData>> Create(int amount);

        IEnumerable<IReposit> IFlexibleRepository.Create(int amount)
            => Create(amount).OfType<IReposit>();
    }

    public static class IRepositoryExtensions 
    {
        public static void Insert<T>(this IRepository self, int index, IReposit<T> reposit, bool replace = false) 
        {
            var target = self
                .SearchAt(index)
                .To<IReposit<T>>();
            var insert = target.Data.IsDefault() || replace;

            if (insert) 
            {
                target.SetIdentify(reposit.Identity);
                target.Preserve(reposit.Data);
            }
        }

        public static void Swap(this IRepository self, int index1, int index2) 
        {
            var reposit1 = self.SearchAt(index1);
            var reposit2 = self.SearchAt(index2);

            var identity = reposit1.Identity;
            var data     = reposit1.Data;

            reposit1.SetIdentify(reposit2.Identity);
            reposit1.Preserve(reposit2.Data);

            reposit2.SetIdentify(identity);
            reposit2.Preserve(data);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using NHibernate;
using Notes.DB.Repositories.Interfaces;

namespace Notes.DB.Repositories
{
    public class NHBaseRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        public virtual T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public virtual void Delete(long id)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var obj = session.Get<T>(id);
                    session.Delete(obj);
                    transaction.Commit();
                }
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            var entities = session.CreateCriteria<T>().List<T>();

            NHibernateHelper.CloseSession();

            return entities;
        }

        public virtual T Load(long id)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            var user = session.Load<T>(id);

            NHibernateHelper.CloseSession();

            return user;
        }

        public virtual void Save(T entity)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            try
            {
                using (var tx = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    tx.Commit();
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
        }
    }
}
﻿using EwiPraca.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace EwiPraca.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly EwiPracaDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ObjectStateManager _objectStateManager;

        public Repository(EwiPracaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _objectStateManager = ((IObjectContextAdapter)_context).ObjectContext.ObjectStateManager;
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            _dbSet.Where(filterExpression).Delete();
            _context.SaveChanges();
        }

        public virtual int Update(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            var id = _dbSet.Update(updateExpression);

            _context.SaveChanges();

            return id;
        }

        public virtual int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return _dbSet.Where(filterExpression).Update(updateExpression);
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            var entity = Update(entityToUpdate, "Id");

            _context.SaveChanges();

            return entity;
        }

        public virtual TEntity Update(TEntity entityToUpdate, string primaryKeyName)
        {
            var entry = _context.Entry(entityToUpdate);

            if (entry.State == EntityState.Detached)
            {
                // Retreive the Id through reflection
                var pkey = _dbSet.Create().GetType().GetProperty(primaryKeyName).GetValue(entityToUpdate);

                TEntity attachedEntity = _dbSet.Find(pkey);  // access the key
                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                    return attachedEntity;
                }
            }

            entry.State = EntityState.Modified;

            return entityToUpdate;
        }

        public virtual void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public IEnumerable<TEntity> All()
        {
            return _dbSet;
        }

        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> Query(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.First(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }

        public int ExecuteSqlCommand(string command, object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(command, parameters);
        }
    }
}
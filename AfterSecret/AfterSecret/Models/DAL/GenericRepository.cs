using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.DAL
{
    public class GenericRepository<Model> where Model : BaseModel, new()
    {
        internal UnitOfWork uw;
        public DbContext context;
        public DbSet<Model> dbSet;

        public GenericRepository(UnitOfWork uw, DbContext context)
        {
            this.uw = uw;
            this.context = context;
            this.context.Database.CommandTimeout = 600;
            this.dbSet = context.Set<Model>();
        }

        public virtual IQueryable<Model> Get(bool noTrack = true,bool isValidate = true)
        {
            var result = dbSet.AsQueryable();
            if(isValidate)
            {
                result = result.Where(a => a.IsValidate == true);
            }

            if (!noTrack)
            {
                return result.AsNoTracking();
            }
            else
            {
                return result;
            }

        }

        public virtual Model GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(Model entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            Model entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(Model entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(Model entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
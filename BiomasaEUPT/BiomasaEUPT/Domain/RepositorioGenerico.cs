using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BiomasaEUPT.Domain
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private BiomasaEUPTEntities db = null;
        private DbSet<T> table = null;

        public RepositorioGenerico()
        {
            this.db = new BiomasaEUPTEntities();
            table = db.Set<T>();
        }

        public RepositorioGenerico(BiomasaEUPTEntities db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IEnumerable<T> SelectAll()
        {
            return table.ToList();
        }

        public T SelectByID(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}

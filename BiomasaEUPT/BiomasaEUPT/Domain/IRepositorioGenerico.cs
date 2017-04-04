using System.Collections.Generic;


namespace BiomasaEUPT.Domain
{
    public interface IRepositorioGenerico<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectByID(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}

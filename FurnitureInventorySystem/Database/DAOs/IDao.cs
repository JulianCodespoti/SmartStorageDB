using System.Collections.Generic;

namespace FurnitureInventorySystem.Database
{
    internal interface IDao<t>
    {
        public t Get(int id);

        public IEnumerable<t> GetAll();

        public void Update(int id, t item);

        public void Delete(int id);

        public void Add(t item);
    }
}
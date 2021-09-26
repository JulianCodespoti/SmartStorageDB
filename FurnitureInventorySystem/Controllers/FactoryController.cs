using FurnitureInventorySystem.Database;
using FurnitureInventorySystem.Database.DAOs;
using System.Collections.Generic;
using System.Linq;

namespace FurnitureInventorySystem.StoreObjects
{
    internal class FactoryController

    {
        private readonly IDao<Factory> _factoryDao = new FactoryDao();

        public Factory SelectedFactory { get; set; }

        internal Factory FindByName(string name)
        {
            return _factoryDao.GetAll().First(factory => factory.Name.Equals(name));
        }

        internal IEnumerable<Factory> GetAllFactories()
        {
            return _factoryDao.GetAll();
        }
    }
}
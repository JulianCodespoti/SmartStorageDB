using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FurnitureInventorySystem.Database.DAOs
{
    internal class FactoryDao : IDao<Factory>
    {
        private readonly string _connectionString;

        public FactoryDao()
        {
            var workingDirectory = Environment.CurrentDirectory;
            _connectionString = $"Data Source={workingDirectory}\\SmartStorageDB.db;Version=3;";        
        }

        public void Add(Factory item)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                insert  Factory(id, name, address, email)
                values (@id, @name, @address, @email)",
            item
             );
        }

        public void Delete(int id)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.QueryFirst(@"
                delete from Factories()
                where id = @id",
            id

             );
        }

        public Factory Get(int id)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            return connection.QueryFirst<Factory>(@"
                select * from Factories
                where id = @id",
            id
            );
        }

        public IEnumerable<Factory> GetAll()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            return connection.Query<Factory>(@"
               select * from Factories"
            );
        }

        public void Update(int id, Factory factory)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                insert into Factory(name, address, email)
                values (@name, @address, @email)
                where id = @id",
            factory
             );
        }
    }
}
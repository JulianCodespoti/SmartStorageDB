using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FurnitureInventorySystem.Database
{
    internal class FurnitureDao : IDao<Furniture>
    {
        private readonly string _connectionString;

        public FurnitureDao()
        {
            var workingDirectory = Environment.CurrentDirectory;
            _connectionString = $"Data Source={workingDirectory}\\SmartStorageDB.db;Version=3;";        
        }

        public void Add(Furniture item)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Execute(@"
                insert into Furniture(name, quantity, price, factory_id)
                values (@name, @quantity, @price, @Factory_Id)",
                item
            );
        }

        public async void Delete(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();
            var sqlStatement = @"DELETE from Furniture WHERE id = @Id";
            await connection.ExecuteAsync(sqlStatement, new { Id = id });
        }

        public Furniture Get(int id)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            return connection.QueryFirst<Furniture>(@"
                select * from Furniture
                where id = @id",
                id
            );
        }

        public IEnumerable<Furniture> GetAll()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            return connection.Query<Furniture>(@"
               select * from Furniture"
            );
        }

        public async void Update(int id, Furniture furniture)
        {
            await using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();
            var sqlStatement = @"
                UPDATE Furniture
                SET  name = @name
                ,factory_id = @Factory_Id
                ,quantity = @quantity
                ,price = @price
                WHERE id = @Id";
            await connection.ExecuteAsync(sqlStatement, furniture);
        }
    }
}
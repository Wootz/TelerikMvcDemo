using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using TelerikMvcDemo.Models;

namespace TelerikMvcDemo.Repositories
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository()
        {
            var path = HostingEnvironment.MapPath(@"~/App_Data/db.sqlite");
            _connectionString = $"data source={path}";
            EnsureDatabase(path);
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(int? pageIndex = -1, int? pageSize = -1, string sort = null, bool desc = false, string condition = null)
        {
            var tableName = GetTableName<T>();
            
            condition = string.IsNullOrEmpty(condition) ? string.Empty : $"WHERE {condition}";

            var sql = new StringBuilder($"SELECT * FROM { tableName } { condition }");

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sql.Append($" ORDER BY {sort}");

                if (desc)
                {
                    sql.Append(" DESC");
                }
            }

            if (pageIndex.HasValue && pageIndex.Value > 0 &&
                pageSize.HasValue && pageSize > 0)
            {
                sql.Append($" LIMIT {(pageIndex - 1) * pageSize}, {pageSize}");
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.QueryAsync<T>(sql.ToString());
            }
        }

        public virtual async Task<int> CountAsync<T>(string condition = null)
        {
            var tableName = GetTableName<T>();

            var sql = $"SELECT COUNT(*) FROM { tableName } {condition ?? string.Empty}";

            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<int>(sql);
            }
        }

        public virtual async Task<T> GetAsync<T>(int id) where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.GetAsync<T>(id);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.GetAllAsync<T>();
            }
        }

        public virtual async Task<int> InsertAsync<T>(T entity) where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.InsertAsync(entity);
            }
        }

        public virtual async Task<int> InsertRangeAsync<T>(IEnumerable<T> entities)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.InsertAsync(entities);
            }
        }

        public virtual async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.UpdateAsync(entity);
            }
        }

        public virtual async Task<bool> UpdateRangeAsync<T>(IEnumerable<T> entities)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.UpdateAsync(entities);
            }
        }

        public virtual async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.DeleteAsync(entity);
            }
        }

        public virtual async Task<bool> DeleteRangeAsync<T>(IEnumerable<T> eneities)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.DeleteAsync(eneities);
            }
        }

        private void EnsureDatabase(string path)
        {
            if (File.Exists(path))
            {
                return;
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute(@"
CREATE TABLE Products (
    ProductID INTEGER NOT NULL,
    ProductName NVARCHAR(50) NOT NULL DEFAULT '',
    UnitPrice DECIMAL(10, 5),
    UnitsInStock INTEGER NOT NULL DEFAULT 0,
    Discontinued INTEGER NOT NULL DEFAULT 0 CHECK(Discontinued IN (0,1)),
    LastSupply DATETIME,
    UnitsOnOrder INTEGER NOT NULL DEFAULT 0,
    CategoryID INTEGER,
    SubCategoryID INTEGER,
    QuantityPerUnit NVARCHAR(50),
    CONSTRAINT Product_PK PRIMARY KEY (ProductID));

CREATE TABLE ProductSpecs (
    ProductSpecID INTEGER NOT NULL,
    ProductID INTEGER NOT NULL,
    SpecName NVARCHAR(50) NOT NULL DEFAULT '',
    Desctiption NVARCHAR(200) NOT NULL DEFAULT '',
    CONSTRAINT ProductSpec_PK PRIMARY KEY (ProductSpecID));
");

                var products = Enumerable.Range(1, 50).Select(i => new Product
                {
                    ProductName = $"Product {i}",
                    UnitPrice = i * 10,
                    UnitsInStock = i * 100,
                    Discontinued = false,
                    LastSupply = DateTime.Now.AddDays(30 + i),
                    CategoryID = 1,
                    SubCategoryID = 1,
                });

                var specs = Enumerable.Range(1, 50).Select(i => Enumerable.Range(1, 5).Select(j => new ProductSpec
                {
                    ProductID = i,
                    SpecName = $"Product {i} Spec {j}",
                    Desctiption = $"Product {i} Spec {j}",
                })).SelectMany(i => i);

                connection.Execute(@"");

                connection.Insert(products);
                connection.Insert(specs);
            }
        }

        private string GetTableName<T>()
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;

            return attribute?.Name ?? type.Name;
        }
    }
}
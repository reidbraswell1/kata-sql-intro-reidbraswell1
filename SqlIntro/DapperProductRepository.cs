using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;

namespace SqlIntro
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public DapperProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Reads all the products from the products table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts()
        {
            using (var conn = new MySqlConnection(_connectionString.ToString()))
            {
                var products = new List<Product>();
                try
                {
                    var sql = "SELECT Id, Name FROM product;";
                    conn.Open();
                    products = conn.Query<Product>(sql).ToList();
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return products;
            }
        }
        public Product GetProduct(int id)
        {
            return new Product();
        }
        public bool DeleteProduct(int id)
        {
            return false;
        }
        public bool UpdateProduct(Product prod)
        {
            return false;
        }
        public bool InsertProduct(Product prod)
        {
            return false;
        }

    }

}
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
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "SELECT ProductID AS Id, Name FROM product;";
                    conn.Open();
                    products = conn.Query<Product>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return products;
        }
        public Product GetProduct(int id)
        {
            Product prod = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    conn.Open();
                    var sql = "SELECT ProductID AS ID, Name FROM product WHERE ProductID = @Id;";
                    prod = conn.Query<Product>(sql, new Product { Id = id }).FirstOrDefault();
                    if (prod == null)
                    {
                        prod = new Product() { Id = -1, Name = "" };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                prod = new Product { Id = -1, Name = "" };
            }
            return prod;
        }
        public IEnumerable<Product> GetProductsWithReview()
        {
            return null;
        }
        public IEnumerable<Product> GetProductsAndReviews()
        {
            return null;
        }
        public bool DeleteProduct(int id)
        {
            var result = false;
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "DELETE FROM product WHERE ProductID = @ID;";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { ID = id });
                    result = (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
        public bool UpdateProduct(Product prod)
        {
            var result = false;
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "UPDATE product SET Name = @Name WHERE ProductID = @ID;";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { ID = prod.Id, Name = prod.Name });
                    result = (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
        public bool InsertProduct(Product prod)
        {
            var result = false;
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "INSERT INTO product (Name) VALUES (@Name);";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { Name = prod.Name });
                    result = (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }

    }

}
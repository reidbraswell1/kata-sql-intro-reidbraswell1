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
        public IEnumerable<Product> GetProducts(int id)
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    // var sql = "SELECT ProductID AS Id, Name FROM product;";
                    var sql = (id > 0) ? "SELECT ProductID AS ID, Name FROM product WHERE ProductId > @ID1 AND ProductId < @ID2" :
                                         "SELECT ProductID AS ID, Name FROM product;";
                    conn.Open();
                    products = conn.Query<Product>(sql,new { ID1 = id -15, ID2 = id + 15 }).ToList();
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
        public IEnumerable<ProductsAndReviews> GetProductsWithReview()
        {
            var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A INNER JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    conn.Open();
                    products = conn.Query<ProductsAndReviews>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return products;
        }
        public IEnumerable<ProductsAndReviews> GetProductsAndReviews()
        {
           var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    var sql = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A LEFT JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    conn.Open();
                    products = conn.Query<ProductsAndReviews>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return products;
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
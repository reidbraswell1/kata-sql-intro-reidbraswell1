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
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "SELECT ProductID AS Id, Name FROM product;";
                    conn.Open();
                    return conn.Query<Product>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Reads range of products from the products table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProductsInRange(int id)
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    // var sql = "SELECT ProductID AS Id, Name FROM product;";
                    var sql = (id > 0) ? "SELECT ProductID AS ID, Name FROM product WHERE ProductId > @ID1 AND ProductId < @ID2" :
                                         "SELECT ProductID AS ID, Name FROM product;";
                    conn.Open();
                    return conn.Query<Product>(sql, new { ID1 = id - 15, ID2 = id + 15 }).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Gets a specific product from the table by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            Product prod = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var sql = "SELECT ProductID AS ID, Name FROM product WHERE ProductID = @Id;";
                    prod = conn.Query<Product>(sql, new Product { Id = id }).FirstOrDefault();
                    if (prod == null)
                    {
                        prod = new Product() { Id = -1 };
                    }
                }
                return prod;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                prod = new Product { Id = -1, Name = "" };
                return prod;
            }
        }

        /// <summary>
        /// Gets Products with Reviews with an Inner Join
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductsAndReviews> GetProductsWithReview()
        {
            var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A INNER JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    conn.Open();
                    return conn.Query<ProductsAndReviews>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Gets Products and reviews from the table with a Left Join
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductsAndReviews> GetProductsAndReviews()
        {
            var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A LEFT JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    conn.Open();
                    return conn.Query<ProductsAndReviews>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Delete a product from the table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteProduct(int id)
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "DELETE FROM product WHERE ProductID = @ID;";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { ID = id });
                    return (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Update a record in the table.
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool UpdateProduct(Product prod)
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "UPDATE product SET Name = @Name WHERE ProductID = @ID;";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { ID = prod.Id, Name = prod.Name });
                    return (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Insert a product into the table
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool InsertProduct(Product prod)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sql = "INSERT INTO product (Name) VALUES (@Name);";
                    conn.Open();
                    var affectedRows = conn.Execute(sql, new { Name = prod.Name });
                    return (affectedRows > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
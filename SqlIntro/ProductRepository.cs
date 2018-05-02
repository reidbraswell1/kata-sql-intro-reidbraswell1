using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SqlIntro
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    // Write a SELECT statement that gets all products  
                    cmd.CommandText = "SELECT ProductID AS ID, Name FROM product;";

                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // cant yield in a try catch but want to catch 
                        // exception so use list.
                        // yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                        products.Add(new Product { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString() });
                    }
                    return products;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Reads a range of products from the table
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
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    // Write a SELECT statement that gets all products  
                    cmd.CommandText = (id > 0) ? "SELECT ProductID AS ID, Name FROM product WHERE ProductId > @id1 AND ProductId < @id2" :
                                                 "SELECT ProductID AS ID, Name FROM product;";
                    cmd.Parameters.AddWithValue("@id1", id - 15);
                    cmd.Parameters.AddWithValue("@id2", id + 15);

                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // cant yield in a try catch but want to catch 
                        // exception so use list.
                        // yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                        products.Add(new Product { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString() });
                    }
                    return products;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }
        /// <summary>
        /// Gets a product from the database by key
        /// </summary>
        /// <param name="id"></param>
        public Product GetProduct(int id)
        {
            Product prod;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT ProductID AS ID, Name FROM product WHERE ProductID = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        return new Product { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString() };
                    }
                    return new Product { Id = -1 };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                prod = new Product { Id = -1 };
                return prod;
            }
        }

        /// <summary>
        /// Gets Products with Reviews with an Inner Join
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductsAndReviews> GetProductsWithReview()
        {
            /* 
              USE adventureworks;
              SELECT product.ProductID, 
                     product.Name,
                     productreview.Comments
              FROM product
              INNER JOIN productreview
              ON product.ProductID = productreview.ProductID;
            */
            var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    // Write a SELECT statement that gets all products  
                    cmd.CommandText = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A INNER JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // cant yield in a try catch but want to catch 
                        // exception so use list.
                        // yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                        products.Add(new ProductsAndReviews { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString(), Comments = dr["Comments"].ToString() });
                    }
                    return products;
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
            /*
              USE adventureworks;
              SELECT product.ProductID,
                     product.Name,
                     productreview.Comments
              FROM product
              LEFT JOIN productreview
              ON product.ProductID = productreview.ProductID;
            */
            var products = new List<ProductsAndReviews>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    // Write a SELECT statement that gets all products  
                    cmd.CommandText = "SELECT A.ProductID AS ID, A.Name, B.Comments FROM product AS A LEFT JOIN productreview AS B ON A.ProductID = B.ProductID;";
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // cant yield in a try catch but want to catch 
                        // exception so use list.
                        // yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                        var productsAndReviews = new ProductsAndReviews();
                        productsAndReviews.Id = Int32.Parse(dr["ID"].ToString());
                        productsAndReviews.Name = dr["Name"].ToString();
                        productsAndReviews.Comments = (dr.IsDBNull(2)) ? "" : dr["Comments"].ToString();
                        products.Add(productsAndReviews);
                        //products.Add(new ProductsAndReviews { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString() });
                    }
                    return products;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return products;
            }
        }

        /// <summary>
        /// Deletes a Product from the database
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteProduct(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    //Write a delete statement that deletes by id
                    cmd.CommandText = $"DELETE FROM product WHERE product.ProductId = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Updates the Product in the database
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool UpdateProduct(Product prod)
        {
            //This is annoying and unnecessarily tedious for large objects.
            //More on this in the future...  Nothing to do here..
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE product SET name = @name WHERE ProductId = @id";
                    cmd.Parameters.AddWithValue("@name", prod.Name);
                    cmd.Parameters.AddWithValue("@id", prod.Id);
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Inserts a new Product into the database
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool InsertProduct(Product prod)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT into product (name) values(@name)";
                    cmd.Parameters.AddWithValue("@name", prod.Name);
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
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

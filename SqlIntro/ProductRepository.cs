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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return products;
        }
        /// <summary>
        /// Deletes a Product from the database
        /// </summary>
        /// <param name="id"></param>
        public Product GetProduct(int id)
        {
            Product prod;

            using (var conn = new MySqlConnection(_connectionString))
            {
                try
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
                    conn.Close();
                    return new Product { Id = -1, Name = "" };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    prod = new Product { Id = -1, Name = "" };
                }
                conn.Close();
                return prod;
            }
        }
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
                using (var conn = new MySqlConnection(_connectionString.ToString()))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    // Write a SELECT statement that gets all products  
                    cmd.CommandText = "SELECT product.ProductID AS ID, product.Name, productreview.Comments FROM product INNER JOIN productreview ON product.ProductID = productreview.ProductID;";
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // cant yield in a try catch but want to catch 
                        // exception so use list.
                        // yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                        products.Add(new ProductsAndReviews { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString(), Comments = dr["Comments"].ToString() });
                    }
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
                using (var conn = new MySqlConnection(_connectionString.ToString()))
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
                        var productsAndReviews = new ProductsAndReviews();
                        productsAndReviews.Id = Int32.Parse(dr["ID"].ToString());
                        productsAndReviews.Name = dr["Name"].ToString();
                        productsAndReviews.Comments = (dr.IsDBNull(3)) ? "" : dr["Comments"].ToString();
                        products.Add(productsAndReviews);
                        //products.Add(new ProductsAndReviews { Id = Int32.Parse(dr["ID"].ToString()), Name = dr["Name"].ToString() });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return products;
        }

        /// <summary>
        /// Deletes a Product from the database
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteProduct(int id)
        {
            var result = false;
            using (var conn = new MySqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    //Write a delete statement that deletes by id
                    cmd.CommandText = $"DELETE FROM product WHERE product.ProductId = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                conn.Close();
            }
            return result;
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
            var result = false;
            using (var conn = new MySqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE product SET name = @name WHERE ProductId = @id";
                    cmd.Parameters.AddWithValue("@name", prod.Name);
                    cmd.Parameters.AddWithValue("@id", prod.Id);
                    result = (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// Inserts a new Product into the database
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool InsertProduct(Product prod)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var result = false;
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT into product (name) values(@name)";
                    cmd.Parameters.AddWithValue("@name", prod.Name);
                    result = (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                conn.Close();
                return result;
            }
        }
    }
}

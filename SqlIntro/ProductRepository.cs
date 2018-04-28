﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SqlIntro
{
    public class ProductRepository
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
        public IEnumerable<Product> GetProducts()
        {
            using (var conn = new MySqlConnection(_connectionString.ToString()))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                //TODO:  Write a SELECT statement that gets all products  
                cmd.CommandText = $"SELECT * FROM product;";
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                }
                conn.Close();
            }
        }
        /// <summary>
        /// Deletes a Product from the database
        /// </summary>
        /// <param name="id"></param>
        public Product GetProduct(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"SELECT ProductID, Name FROM product WHERE ProductId = {id};";
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        return new Product { Id = Int32.Parse(dr["ProductID"].ToString()), Name = dr["Name"].ToString() };
                    }
                    conn.Close();
                    return new Product { Id = -1, Name = "" };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new Product { Id = -1, Name = "" };
                }
            }
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
                    cmd.CommandText = $"DELETE FROM product WHERE product.ProductID = {id};";
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
            }
            return result;
        }
        /// <summary>
        /// Updates the Product in the database
        /// </summary>
        /// <param name="prod"></param>
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
                    cmd.CommandText = "UPDATE product SET name = @name WHERE id = @id";
                    cmd.Parameters.AddWithValue("@name", prod.Name);
                    cmd.Parameters.AddWithValue("@id", prod.Id);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return result;
        }
        /// <summary>
        /// Inserts a new Product into the database
        /// </summary>
        /// <param name="prod"></param>
        public void InsertProduct(Product prod)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT into product (name) values(@name)";
                cmd.Parameters.AddWithValue("@name", prod.Name);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

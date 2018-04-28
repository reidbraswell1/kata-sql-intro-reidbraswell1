﻿using System;

namespace SqlIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = "aws-maria-db.cliyienc3i9k.us-east-2.rds.amazonaws.com";
            var database = "adventureworks";
            var userId = "";
            var password = "";

            do
            {
                Console.WriteLine("Enter Your UserId");
            } while (String.IsNullOrEmpty(userId = Console.ReadLine()));

            do
            {
                Console.WriteLine("Enter Your Password To Database");
            } while (String.IsNullOrEmpty(password = Console.ReadLine()));

            //get connectionString format from connectionstrings.com and change to match your database
            var connectionString = $"Server={server};Database={database};Uid={userId};Pwd={password};";
            var repo = new ProductRepository(connectionString);

            foreach (var prod in repo.GetProducts())
            {
                Console.WriteLine("Product ID:" + prod.Id + " Product Name:" + prod.Name);
            }

            var id = 0;
            do
            {
                Console.WriteLine("Enter A Product ID to Delete");
                Int32.TryParse(Console.ReadLine().ToString(), out id);
            } while (id == 0);

            var deleteResult = "";
            deleteResult = (repo.DeleteProduct(id)) ? $"Product ID {id} was deleted from the database" :
                                                      $"Product ID {id} was not deleted from the database";
            Console.WriteLine(deleteResult);

            Console.WriteLine("Press Return to Exit");
            Console.ReadLine();
        }


    }
}

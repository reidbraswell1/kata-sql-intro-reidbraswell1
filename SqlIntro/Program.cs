using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SqlIntro
{
    class Program
    {
        private static string PromptProductName(Crud crud)
        {
            var name = "";
            var enumDesc = CrudMethods.getEnumDescription(crud);
            do
            {
                Console.WriteLine($"Enter Product Name To {enumDesc}");
            } while (String.IsNullOrEmpty(name = Console.ReadLine()));
            return name;
        }
        private static int PromptProductId(Crud crud)
        {
            var id = 0;
            do
            {
                var enumDesc = CrudMethods.getEnumDescription(crud);
                Console.WriteLine($"Enter A Product ID to {enumDesc} -- A number between 1 and {int.MaxValue}");
                Int32.TryParse(Console.ReadLine().ToString(), out id);
            } while (id == 0);
            return id;
        }
        private static void DisplayAllProducts(IProductRepository repo, int id)
        {
            if (id > 0)
            {
                foreach (var prod in repo.GetProductsInRange(id))
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\tProduct Name:" + prod.Name);
                }
            }
            else
            {
                foreach (var prod in repo.GetProducts())
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\tProduct Name:" + prod.Name);
                }
            }
        }
        private static void DisplayAllProductsWithReviews(IProductRepository repo)
        {
            foreach (var prod in repo.GetProductsWithReview())
            {
                if (string.IsNullOrEmpty(prod.Comments))
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\tProduct Name:" + prod.Name + "\tProduct Review:" + prod.Comments);
                }
                else
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\nProduct Name:" + prod.Name + "\nProduct Review:" + prod.Comments + "\n");
                }
            }
        }
        private static void DisplayAllProductsAndReviews(IProductRepository repo)
        {
            foreach (var prod in repo.GetProductsAndReviews())
            {
                if (string.IsNullOrEmpty(prod.Comments))
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\tProduct Name:" + prod.Name);
                }
                else
                {
                    Console.WriteLine("Product ID:" + prod.Id + "\nProduct Name:" + prod.Name + "\nProduct Review:" + prod.Comments + "\n");
                }
            }
        }

        private static void DeleteProduct(int id, IProductRepository repo)
        {
            var deleteResult = (repo.DeleteProduct(id)) ? $"Product ID \'{id}\' Was Deleted From The Database" :
                                                          $"Product ID \'{id}\' Not Found Not Deleted From The Database";
            Console.WriteLine(deleteResult);
            DisplayAllProducts(repo, id);
        }
        private static void UpdateProduct(int id, string name, IProductRepository repo)
        {
            var prod = repo.GetProduct(id);
            if (prod.Id < 0)
            {
                Console.WriteLine($"Product Not Updated Id \'{id}\' Not Found");
                DisplayAllProducts(repo, id);
            }
            else
            {
                Console.WriteLine($"Updating Product \'{prod.Id}\' \'{prod.Name}\'");
                prod.Name = name;
                var updateResult = (repo.UpdateProduct(prod)) ? ($"Product ID \'{id}\' Was Successfully Updated") :
                                                                ($"Product ID \'{id}\' Not Updated");
                Console.WriteLine(updateResult);
            }
        }
        private static void InsertProduct(string name, IProductRepository repo)
        {
            Product prod = new Product() { Id = 0, Name = name };
            Console.WriteLine($"Adding Product \'{name}\' To The Database");
            var addResult = (repo.InsertProduct(prod)) ? $"Product \'{name}\' Was Inserted Into The Database" :
                                                         $"Product \'{name}\' Was Not Inserted Into The Database";
            Console.WriteLine(addResult);
        }
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.debug.json", optional: false, reloadOnChange: true);

            var config = configuration.Build();
            var connectionString = config["connectionString"];
            var repo1 = new ProductRepository(connectionString);
            var repo2 = new DapperProductRepository(connectionString);

            Console.WriteLine("\n*** READ ALL PRODUCTS TEST - PRODUCT REPOSITORY ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProducts(repo1, 0);
            Console.WriteLine("\n*** READ ALL PRODUCTS TEST - DAPPER PRODUCT REPOSITORY ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProducts(repo2, 0);

            Console.WriteLine("\n*** READ ALL PRODUCTS WITH REVIEWS TEST SQL ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProductsWithReviews(repo1);
            Console.WriteLine("\n*** READ ALL PRODUCTS WITH REVIEWS TEST DAPPER ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProductsWithReviews(repo2);
            Console.WriteLine("\n*** READ ALL PRODUCTS AND REVIEWS TEST SQL ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProductsAndReviews(repo1);
            Console.WriteLine("\n*** READ ALL PRODUCTS AND REVIEWS TEST DAPPER ***" + " Press Return");
            Console.ReadLine();
            DisplayAllProductsAndReviews(repo2);

            Console.WriteLine("\n*** DELETE PRODUCT TEST SQL ***");
            DeleteProduct(PromptProductId(Crud.Delete), repo1);
            Console.WriteLine("\n*** DELETE PRODUCT TEST DAPPER ***");
            DeleteProduct(PromptProductId(Crud.Delete), repo2);


            Console.WriteLine("\n*** UPDATE PRODUCT TEST SQL ***");
            UpdateProduct(PromptProductId(Crud.Update), PromptProductName(Crud.Update), repo1);
            Console.WriteLine("\n*** UPDATE PRODUCT TEST DAPPER ***");
            UpdateProduct(PromptProductId(Crud.Update), PromptProductName(Crud.Update), repo2);

            Console.WriteLine("\n*** INSERT PRODUCT TEST SQL ***");
            InsertProduct(PromptProductName(Crud.Create), repo1);
            Console.WriteLine("\n*** INSERT PRODUCT TEST DAPPER ***");
            InsertProduct(PromptProductName(Crud.Create), repo2);

            Console.WriteLine("\nPress Return to Exit -- Tests Completed");
            Console.ReadLine();
        }
    }
}

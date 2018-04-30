using System;

namespace SqlIntro
{
    class Program
    {
        private static string promptUserId()
        {
            var userId = "";
            do
            {
                Console.WriteLine("Enter Your UserId");
            } while (String.IsNullOrEmpty(userId = Console.ReadLine()));
            return userId;
        }
        private static string PromptPassword()
        {
            var password = "";
            do
            {
                Console.WriteLine("Enter Your Password To Database");
            } while (String.IsNullOrEmpty(password = Console.ReadLine()));
            return password;
        }
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
        private static void DisplayAllProducts(IProductRepository repo)
        {
            foreach (var prod in repo.GetProducts())
            {
                Console.WriteLine("Product ID:" + prod.Id + " Product Name:" + prod.Name);
            }
        }
        private static void DeleteProduct(int id, ProductRepository repo)
        {
            var deleteResult = (repo.DeleteProduct(id)) ? $"Product ID {id} Was Deleted From The Database" :
                                                          $"Product ID {id} Not Found Not Deleted From The Database";
            Console.WriteLine(deleteResult);
        }
        private static void UpdateProduct(int id, string name, ProductRepository repo)
        {
            var prod = repo.GetProduct(id);
            if (prod.Id < 0)
            {
                Console.WriteLine($"Product Not Updated Id {id} Not Found");
            }
            else
            {
                Console.WriteLine($"Updating Product {prod.Id} {prod.Name}");
                prod.Name = name;
                var updateResult = (repo.UpdateProduct(prod)) ? ($"Product ID {id} Was Successfully Updated") :
                                                                ($"Product ID {id} Not Updated");
                Console.WriteLine(updateResult);
            }
        }
        private static void InsertProduct(string name, ProductRepository repo)
        {
            Product prod = new Product() { Id = 0, Name = name };
            Console.WriteLine($"Adding Product {name} To The Database");
            var addResult = (repo.InsertProduct(prod)) ? $"Product {name} Was Inserted Into The Database" :
                                                         $"Product {name} Was Not Inserted Into The Database";
            Console.WriteLine(addResult);
        }
        static void Main(string[] args)
        {
            var server = "aws-maria-db.cliyienc3i9k.us-east-2.rds.amazonaws.com";
            var database = "adventureworks";
            var userId = promptUserId();
            var password = PromptPassword();

            //get connectionString format from connectionstrings.com and change to match your database
            var connectionString = $"Server={server};Database={database};Uid={userId};Pwd={password};";
            var repo1 = new ProductRepository(connectionString);
            var repo2 = new DapperProductRepository(connectionString);

            Console.WriteLine("\n***READ ALL PRODUCTS TEST PRODUCT REPOSITORY***");
            DisplayAllProducts(repo1);
            Console.WriteLine("\nPress Return For Dapper Test");
            Console.ReadLine();
            Console.WriteLine("\n***READ ALL PRODUCTS TEST DAPPER PRODUCT REPOSITORY***");
            DisplayAllProducts(repo2);

            Console.WriteLine("\n***DELETE PRODUCT TEST***");
            DeleteProduct(PromptProductId(Crud.Delete), repo1);

            Console.WriteLine("\n***UPDATE PRODUCT TEST***");
            UpdateProduct(PromptProductId(Crud.Update), PromptProductName(Crud.Update), repo1);

            Console.WriteLine("\n***INSERT PRODUCT TEST***");
            InsertProduct(PromptProductName(Crud.Create), repo1);

            Console.WriteLine("Press Return to Exit");
            Console.ReadLine();
        }
    }
}

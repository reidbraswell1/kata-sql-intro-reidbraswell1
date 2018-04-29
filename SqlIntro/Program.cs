using System;
using System.ComponentModel;
using System.Reflection;

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
        private static string promptPassword()
        {
            var password = "";
            do
            {
                Console.WriteLine("Enter Your Password To Database");
            } while (String.IsNullOrEmpty(password = Console.ReadLine()));
            return password;
        }
        private static string promptProductName()
        {
            var name = "";
            do
            {
                Console.WriteLine("Enter Product Name");
            } while (String.IsNullOrEmpty(name = Console.ReadLine()));
            return name;
        }
        private static int promptProductId(Crud crud)
        {
            var id = 0;
            do
            {
                FieldInfo fi = crud.GetType().GetField(crud.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var enumDesc = (attributes.Length > 0) ? attributes[0].Description : crud.ToString();
                switch (crud)
                {
                    case Crud.Delete:
                        Console.WriteLine($"Enter A Product ID to {enumDesc} -- A number between 1 and {int.MaxValue}");
                        break;
                    case Crud.Update:
                        Console.WriteLine($"Enter A Product ID to Update -- A number between 1 and {int.MaxValue}");
                        break;
                }
                Int32.TryParse(Console.ReadLine().ToString(), out id);
            } while (id == 0);
            return id;
        }
        private static void deleteProduct(int id, ProductRepository repo)
        {
            var deleteResult = "";
            deleteResult = (repo.DeleteProduct(id)) ? $"Product ID {id} was deleted from the database" :
                                                      $"Product ID {id} was not deleted from the database";
            Console.WriteLine(deleteResult);
        }
        private static void updateProduct(int id, string name, ProductRepository repo)
        {
            var prod = repo.GetProduct(id);
            if (prod.Id < 0)
            {
                Console.WriteLine($"Product Not Updated Id Not Found {id}");
            }
            else
            {
                Console.WriteLine($"Updating Product {prod.Id} {prod.Name}");
                prod.Name = name;
                if (repo.UpdateProduct(prod))
                {
                    Console.WriteLine($"Product {id} Was Successfully Updated");
                }
                else
                {
                    Console.WriteLine($"Product {id} Was Not Updated");
                }
            }
        }
        static void Main(string[] args)
        {
            var server = "aws-maria-db.cliyienc3i9k.us-east-2.rds.amazonaws.com";
            var database = "adventureworks";
            var userId = promptUserId();
            var password = promptPassword();

            //get connectionString format from connectionstrings.com and change to match your database
            var connectionString = $"Server={server};Database={database};Uid={userId};Pwd={password};";
            var repo = new ProductRepository(connectionString);

            foreach (var prod in repo.GetProducts())
            {
                Console.WriteLine("Product ID:" + prod.Id + " Product Name:" + prod.Name);
            }

            deleteProduct(promptProductId(Crud.Delete), repo);

            updateProduct(promptProductId(Crud.Update), promptProductName(), repo);

            Console.WriteLine("Press Return to Exit");
            Console.ReadLine();
        }
    }
}

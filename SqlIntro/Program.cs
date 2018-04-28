using System;

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
            Console.WriteLine("Enter Your UserId");
            userId = Console.ReadLine();
            Console.WriteLine("Enter Your Password To Database");
            password = Console.ReadLine();

            //get connectionString format from connectionstrings.com and change to match your database
            var connectionString = $"Server={server};Database={database};Uid={userId};Pwd={password};";
            var repo = new ProductRepository(connectionString);

            foreach (var prod in repo.GetProducts())
            {
                Console.WriteLine("Product ID:" + prod.Id + " Product Name:" + prod.Name);
            }

            Console.WriteLine("Press Return to Exit");
            Console.ReadLine();
        }

       
    }
}

using Dapper;
using DapperApp;
using System.Data.SqlClient;

string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DapperDb;Integrated Security=True;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    connection.Execute("create table Product(id int primary key identity, name varchar(64), price int)");

    for (int i = 0; i < 10; i++)
    {
        int price = 100;
        connection.Execute("insert Product values (@name, @price)",
        new Product { Name = $"Product {i + 1}", Price = price + ((i + 1) * 10) });
    }

    var list1 = connection.Query<Product>("select * from Product");

    Console.WriteLine("Count: " + connection.ExecuteScalar("select count(*) from Product"));
    foreach (var item in list1)
    {
        Console.WriteLine($"{item.Id} {item.Name} {item.Price}");
    }

    Console.WriteLine("\nProducts price > 140\n");

    var list2 = connection.Query<Product>("select * from Product where price > 140");

    foreach (var item in list2)
    {
        Console.WriteLine($"{item.Id} {item.Name} {item.Price}");
    }

    Console.WriteLine("\nFirst Product price > 139\n");

    var list3 = connection.QueryFirstOrDefault<Product>("select * from Product where price > 139");
    if (list3 != null) { Console.WriteLine($"{list3.Id} {list3.Name} {list3.Price}"); }
    else { Console.WriteLine("List3 == null"); }

    Console.WriteLine("\nDelete Product 4; Updating Product 7\n");

    // Delete Product 4

    var list4 = connection.Query<Product>("delete from Product where id = 4");

    // Updating Product 7

    var list5 = connection.Query<Product>($"update Product set name = '{"Apple"}', price = 50 where id = 7");

    // Result

    var result = connection.Query<Product>("select * from Product");

    foreach (var item in result)
    {
        Console.WriteLine($"{item.Id} {item.Name} {item.Price}");
    }
}
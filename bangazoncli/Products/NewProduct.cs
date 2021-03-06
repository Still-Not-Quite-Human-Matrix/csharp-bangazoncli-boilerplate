﻿using System;
using System.Data.SqlClient;
using System.Configuration;


namespace bangazoncli
{
    class NewProduct
    {
        readonly string _connectionString = ConfigurationManager.ConnectionStrings["SNQHM_bangazoncli_db"].ConnectionString;

        public bool InsertProduct(string name, double price, int owner, int count, string description)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Product 
                                (Name,
                                 Price,
                                 Owner,
                                 Description,
                                 Count)
                                VALUES (@Name, @Price, @Owner, @Description, @Count)";

                        
                var NameParam = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                NameParam.Value = name;
                cmd.Parameters.Add(NameParam);

                var PriceParam = new SqlParameter("@Price", System.Data.SqlDbType.Money);
                PriceParam.Value = price;
                cmd.Parameters.Add(PriceParam);

                var OwnerParam = new SqlParameter("@Owner", System.Data.SqlDbType.Int);
                OwnerParam.Value = owner;
                cmd.Parameters.Add(OwnerParam);

                var DescriptionParam = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar);
                DescriptionParam.Value = description;
                cmd.Parameters.Add(DescriptionParam);

                var CountParam = new SqlParameter("@Count", System.Data.SqlDbType.Int);
                CountParam.Value = count;
                cmd.Parameters.Add(CountParam);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result == 1;

            }
        }
    }

    public class ProductMaker
    {
        public static bool ProductCreator(int owner)
        {
            var productQuery = new NewProduct();

            Console.WriteLine("Please type product name:");
            var productName = Console.ReadLine();

            Console.WriteLine($"How much is {productName}?:");
            var price = double.Parse(Console.ReadLine());

            Console.WriteLine($"Provide a description for {productName}:");
            var description = Console.ReadLine();

            Console.WriteLine($"How many {productName} do you have to sell?:");
            var count = int.Parse(Console.ReadLine());

            var result = productQuery.InsertProduct(productName, price, owner, count, description);

            Console.WriteLine("Type [0] to return to the main menu.");

            return result;
        }
    }
}

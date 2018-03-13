﻿using bangazoncli.Customers;
using bangazoncli.Products;
using bangazoncli.Models;
using System;
using System.Collections.Generic;
using cki = System.ConsoleKeyInfo;

namespace bangazoncli
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = SetupNewApp();
            Customer activeCustomer = null;

            var run = true;
            while (run)
            {
                cki userInput = MainMenu(activeCustomer);

                switch (userInput.KeyChar)
                {
                    case '0':
                        run = false;
                        break;

                    case '1':
                        Console.Clear();

                        if (TheCustomerAccountMaker.CustomerCreator())
                        {
                            Console.WriteLine("Customer added successfully.");
                        }

                        Console.ReadLine();

                        break;

                    case '2':
                        Console.Clear();

                        var customerDataQuery = new GetCustomerData();
                        var customerData = customerDataQuery.GetCustomerByName();

                        var chosenCustomer = int.Parse(ChooseActiveCustomerMenu(customerData).KeyChar.ToString());

                        if (chosenCustomer != 0)
                        {
                            activeCustomer = customerData[chosenCustomer - 1];
                        }

                        break;

                    case '3':

                        var productData = new GetProductData().getProducts();

                        View ProductsView = new View().AddMenuText("These are all available products.");

                        foreach (var product in productData)
                        {
                            ProductsView.AddMenuText($"Product ID: {product.ProductID}, Name: {product.Name}, Price: {product.Price}");
                        }
                        ProductsView.AddMenuText("Press the any key to return to the Main Menu");

                        Console.Write(ProductsView.GetFullMenu());

                        Console.ReadKey();

                        break;

                    case '4':

                        Console.Clear();

                        if (ProductMaker.ProductCreator())
                        {
                            Console.WriteLine("Product added successfully.");
                        }

                        Console.ReadLine();

                        break;

                    case '5':
                        if (activeCustomer != null)
                        {
                            Console.Clear();
                            Console.WriteLine("1: Remove Product");
                            Console.WriteLine("2: Return to Main Menu");
                            cki productSubSelection = Console.ReadKey();
                            var remove = true;
                            while (remove)
                            {
                                switch (productSubSelection.KeyChar)
                                {
                                    case '0':
                                        remove = false;
                                        break;

                                    case '1':
                                        Console.Clear();
                                        Console.WriteLine("Type a product id and press enter...");
                                        // Generate Product Menu //
                                        var customerProducts = new GetProductList();
                                        var ProductData = customerProducts.GetProducts(activeCustomer.CustomerID);

                                        foreach (var product in ProductData)
                                        {
                                            Console.WriteLine($"{product.ProductID}. {product.Name}: {product.Price}");
                                        }
                                        Console.WriteLine("\nPress [0] to return to the main menu");

                                        // Read Input and Remove Product by ID // 
                                        var selection = Console.ReadLine();
                                        var productDelete = new RemoveProduct().DeleteProduct(int.Parse(selection));
                                        if (int.Parse(selection) == 0)
                                        {
                                            remove = false;
                                        }
                                        else if (productDelete)
                                        {
                                            Console.WriteLine("Product deleted press enter to relaod list");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Product not deleted or does not exist");
                                        }
                                        //Console.ReadKey();
                                        break;

                                    case '2':
                                        remove = false;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(" Please Select a Customer First. Press [enter] to try again.");
                            Console.ReadKey();
                        }

                        break;
                }
            }
        }

        static DatabaseContext SetupNewApp()
        {
            Console.Title = "Bangazon Command Line Ordering System";
            var db = new DatabaseContext();
            return db;
        }

        static cki MainMenu(Customer activeCustomer)
        {

            View mainMenu = new View()
                .AddMenuOption("Create a customer account")
                .AddMenuOption("Choose active customer")
                //.AddMenuOption("Create a payment option")
                .AddMenuOption("(Under Construction, Only displays products) Add product to shopping cart")
                .AddMenuOption("Add product to sell");
                //.AddMenuOption("Complete an order")
            if (activeCustomer != null)
            {
                mainMenu.AddMenuOption($"Remove product(s) from {activeCustomer.FirstName} {activeCustomer.LastName}");
            }
            else
            {
                mainMenu.AddMenuOption($"Select a customer to remove product(s)");
            };
            //.AddMenuOption("Update product information")
            //.AddMenuOption("Show stale products")
            //.AddMenuOption("Show customer revenue report")
            //.AddMenuOption("Show overall product popularity")
            mainMenu.AddMenuText("Press [0] To Leave Bangazon!");

            Console.Write(mainMenu.GetFullMenu());

            if (activeCustomer != null)
            {
                Console.WriteLine($"Your current active Customer is {activeCustomer.FirstName} {activeCustomer.LastName}");
            }

            cki userOption = Console.ReadKey();
            return userOption;
        }

        static cki ChooseActiveCustomerMenu(List<Customer> CustomerData)
        {
            View ChooseMenu = new View().AddMenuText("Which customer will be active?");

            foreach (var customer in CustomerData)
            {
                ChooseMenu.AddMenuOption($"Customer ID: {customer.CustomerID} Name: {customer.FirstName} {customer.LastName}");
            }
            ChooseMenu.AddMenuOption("Exit!");

            Console.Write(ChooseMenu.GetFullMenu());
            cki userOption = Console.ReadKey();
            return userOption;
        }

    }
}


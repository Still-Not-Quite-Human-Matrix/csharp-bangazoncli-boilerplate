﻿using bangazoncli.Customers;
using bangazoncli.Products;
using bangazoncli.Models;
using System;
using System.Collections.Generic;
using cki = System.ConsoleKeyInfo;
using bangazoncli.Menus;
using bangazoncli.OrderItems;
using bangazoncli.Payments;

namespace bangazoncli
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = SetupNewApp();
            Customer activeCustomer = null;
            List<Product> listOfOrderItems = new List<Product>();

            var run = true;
            while (run)
            {
                string userInput = MainMenu(activeCustomer);

                switch (userInput)
                {
                    case "0":
                        run = false;
                        break;

                    case "1":
                        Console.Clear();

                        if (TheCustomerAccountMaker.CustomerCreator())
                        {
                            Console.WriteLine("Customer added successfully.");
                        }

                        Console.ReadLine();

                        break;

                    case "2":
                        Console.Clear();

                        var customerDataQuery = new GetCustomerData();
                        var customerData = customerDataQuery.GetCustomerByName();

                        var chosenCustomer = int.Parse(ChooseActiveCustomerMenu(customerData));

                        if (chosenCustomer != 0 && chosenCustomer < customerData.Count + 1)
                        {
                            activeCustomer = customerData[chosenCustomer - 1];
                        }

                        break;

                    case "3":

                        var productData = new GetProductData().getProducts();

                        if (activeCustomer != null)
                        {
                            Console.Clear();

                            if (ThePaymentTypeCreator.PaymentCreator(activeCustomer.CustomerID))
                            {
                                Console.WriteLine("Payment added successfully.");
                            }
                        }
                            Console.ReadLine();

                            break;

                    case "4":

                        var runThisMenu = true;

                        while (runThisMenu)
                        {

                            var productDataQuery = new GetProductData();
                            productData = productDataQuery.getProducts();

                            var chosenInput = int.Parse(ChooseProductMenu(productData));

                            if (chosenInput != 0)
                            {
                                var chosenProduct = productData[chosenInput - 1];
                                listOfOrderItems.Add(chosenProduct);
                            }
                            else
                            {
                                runThisMenu = false;
                            }

                        }


                        break;

                    case "5":

                        Console.Clear();

                        if (ProductMaker.ProductCreator(activeCustomer.CustomerID))
                        {
                            Console.WriteLine("Product added successfully.");
                        }

                        Console.ReadLine();

                        break;


                    case "6":
                        var customerOrderMenu = new CompleteOrderMenu();
                        var completeCustomerOrder = customerOrderMenu.CompleteOrder(listOfOrderItems, activeCustomer);

                        listOfOrderItems.Clear();

                        var customerChosenPayment = Console.ReadLine();
                        
                        if (completeCustomerOrder)
                        {
                            Console.WriteLine("You completed your order");
                        }

                        break;

                    case "7":
                        if (activeCustomer != null)
                        {
                            var menu = new ProductMenus();
                            var result = menu.DeleteProduct(activeCustomer);
                        }
                        else
                        {
                            Console.WriteLine(" Please Select a Customer First. Press [enter] to try again.");
                            Console.ReadKey();
                        } 
                        break;

                    case "8":
                        if (activeCustomer != null)
                        {
                            var menu = new ProductMenus();
                            var result = menu.UpdateProduct(activeCustomer);
                        }
                        else
                        {
                            Console.WriteLine(" Please Select a Customer First. Press [enter] to try again.");
                            Console.ReadKey();
                        }
                        break;

                    case "" +
                    "9":

                        if (listOfOrderItems.Count > 0)
                        {
                            var itemsList = new ShowOrderItems();
                            itemsList.ShowListOfItems(listOfOrderItems);
                        }
                        else
                        {
                            Console.WriteLine("sorry no items in cart");
                        }

                        break;

                    case "10":

                        if (listOfOrderItems.Count > 0)
                        {
                            var itemsList = new RemoveOrderItemsFromList();
                            itemsList.RemoveFromListOfItems(listOfOrderItems);
                        }
                        else
                        {
                            Console.WriteLine("sorry no items in cart");
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

        static string MainMenu(Customer activeCustomer)
        {

            View mainMenu = new View()
                .AddMenuOption("Create a customer account")
                .AddMenuOption("Choose active customer")
                .AddMenuOption("Create a payment option")
                .AddMenuOption("Add product to shopping cart")
                .AddMenuOption("Add product to sell")
                .AddMenuOption("Complete an order");
                if (activeCustomer != null)
                {
                    mainMenu.AddMenuOption($"Remove product(s) from {activeCustomer.FirstName} {activeCustomer.LastName}");
                }
                else
                {
                    mainMenu.AddMenuOption($"Select a customer to remove product(s)");
                };
                mainMenu.AddMenuOption("Update product information")
                //.AddMenuOption("Show stale products")
                //.AddMenuOption("Show customer revenue report")
                //.AddMenuOption("Show overall product popularity")
                .AddMenuOption("See products in customer cart")
                .AddMenuOption("Remove products in customer cart")
                .AddMenuText("Press [0] To Leave Bangazon!");

                Console.Write(mainMenu.GetFullMenu());

                if (activeCustomer != null)
                {
                    Console.WriteLine($"Your current active Customer is {activeCustomer.FirstName} {activeCustomer.LastName}");
                }
                else
                {
                    Console.WriteLine("No active customer set");
                }

                string userOption = Console.ReadLine();
                return userOption;
        }

        static string ChooseActiveCustomerMenu(List<Customer> customerData)
        {
            View ChooseMenu = new View().AddMenuText("Which customer will be active?");

            foreach (var customer in customerData)
            {
                ChooseMenu.AddMenuOption($"Customer ID: {customer.CustomerID} Name: {customer.FirstName} {customer.LastName}");
            }
            ChooseMenu.AddMenuText("0. Return to Main Menu");

            Console.Write(ChooseMenu.GetFullMenu());
            string userOption = Console.ReadLine();
            return userOption;
        }

        static string ChooseProductMenu(List<Product> productData)
        {

            View ProductsView = new View().AddMenuText("These are all available products.");

            foreach (var product in productData)
            {
                ProductsView.AddMenuOption($"Product ID: {product.ProductID}, Name: {product.Name}, Price: {product.Price}");
            }

            ProductsView.AddMenuText("0. Done adding products");

            Console.Write(ProductsView.GetFullMenu());
            string userOption = Console.ReadLine();

            return userOption;
        }

    }
}
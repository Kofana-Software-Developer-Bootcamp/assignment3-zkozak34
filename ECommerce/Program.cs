using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace ECommerce
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            int appSection = 1;
            string[] operations = {"Sipariş ekle", "Siparişleri listele", "Müşteri bilgilerini göster"};
            Customer selectedCustomer = null;
            // Dummy Data
            List<Customer> customers = new List<Customer>()
            {
                new Customer
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8), TcNo = "12345678901", Fullname = "Zeynel KOZAK",
                    Gsm = "5435434343",
                    Address = "Istanbul"
                },
                new Customer
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8), TcNo = "12345678902", Fullname = "Ahmet Bey",
                    Gsm = "5325323232",
                    Address = "Ankara"
                },
                new Customer
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8), TcNo = "12345678903", Fullname = "Mehmet Bey",
                    Gsm = "5215212121",
                    Address = "Konya"
                },
                new Customer
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8), TcNo = "12345678904", Fullname = "Ali Bey",
                    Gsm = "5105101010",
                    Address = "Eskişehir"
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product {Id = Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10},
                new Product {Id = Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 12},
                new Product {Id = Guid.NewGuid(), Name = "Product 3", Price = 300, Stock = 14},
                new Product {Id = Guid.NewGuid(), Name = "Product 4", Price = 400, Stock = 16},
                new Product {Id = Guid.NewGuid(), Name = "Product 5", Price = 500, Stock = 18}
            };

            Console.WriteLine("# Müşteri Sipariş Hattı");
            // App Section 1 - Choose customer or add new customer
            while (appSection == 1)
            {
                Console.WriteLine("1. Yeni müşteri oluştur");
                Console.WriteLine("2. Mevcut müşteri üzerinden işlem yap");
                Console.Write("İşlem seçiniz: ");
                string op = Console.ReadLine();
                if (op == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Müşteri oluştur.");
                    AddNewCustomer();
                }
                else if (op == "2")
                {
                    Console.Clear();
                    GetCustomers();
                    Console.Write("Müşteri seç: ");
                    int c = int.Parse(Console.ReadLine());
                    if (c == 0 || c > customers.Count)
                    {
                        Console.WriteLine("Listeden müşteri seçiniz.");
                    }
                    else
                    {
                        Console.Clear();
                        selectedCustomer = customers[c - 1];
                        Console.WriteLine("Seçilen müşteri: {0}", selectedCustomer.Fullname);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Lütfen bir işlem seçiniz.");
                }
            }

            // Functions
            Customer GetCustomer(string tcNo)
            {
                return customers.SingleOrDefault(c => c.TcNo == tcNo);
            }

            void GetCustomers()
            {
                customers = customers.ToList();
                var table = new ConsoleTable("#", "Ad ve Soyad", "T.C. No.", "Telefon Numarası", "Adres");
                for (int i = 0; i < customers.Count; i++)
                {
                    table.AddRow(i + 1, customers[i].Fullname, customers[i].TcNo, customers[i].Gsm,
                        customers[i].Address);
                }

                table.Write();
            }

            void AddNewCustomer()
            {
                Console.Write("T.C. No.: ");
                string tcNo = Console.ReadLine();
                Console.Write("Ad Soyad: ");
                string fullName = Console.ReadLine();
                Console.Write("Telefon Numarası: ");
                string gsm = Console.ReadLine();
                Console.Write("Adres: ");
                string address = Console.ReadLine();

                if (tcNo == "" || fullName == "" || gsm == "" || address == "")
                {
                    Console.WriteLine("Lütfen istenilen bilgileri eksiksiz giriniz.");
                }
                else
                {
                    customers.Add(new Customer()
                    {
                        Id = Guid.NewGuid().ToString().Substring(0, 8), Fullname = fullName, Gsm = gsm,
                        Address = address, TcNo = tcNo
                    });
                    Console.WriteLine("Yeni müşteri oluşturuldu.");
                }
            }
        }
    }

    class Customer
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string TcNo { get; set; }
        public string Gsm { get; set; }
        public string Address { get; set; }
    }

    class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
    }

    class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
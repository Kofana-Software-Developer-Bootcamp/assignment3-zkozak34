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
            int appSection = 1;
            List<Customer> customers = null;
            List<Product> products = null;
            List<Order> orders = null;
            Customer selectedCustomer = null;
            InitializeData();
            
            Console.WriteLine("# Müşteri Sipariş Hattı");
            while (true)
            {
                if (appSection == 1)
                {
                    Console.WriteLine("1. Yeni müşteri oluştur");
                    Console.WriteLine("2. Müşteri listesi");
                    Console.WriteLine("3. T.C. No ile işlem yap");
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
                        DisplayCustomer();
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
                            appSection = 2;
                        }
                    }
                    else if (op == "3")
                    {
                        Console.Clear();
                        Console.Write("T.C. No. giriniz: ");
                        string tc = Console.ReadLine();
                        Customer customer = customers.SingleOrDefault(c => c.TcNo == tc);
                        if (customer != null)
                        {
                            selectedCustomer = customer;
                            appSection = 2;
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(tc + " nolu T.C. bulunamadı.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Lütfen bir işlem seçiniz.");
                    }
                }
                else if (appSection == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Seçilen müşteri: {0}", selectedCustomer.FullName);
                    Console.WriteLine("İşlem Listesi");
                    Console.WriteLine("1. Sipariş ekle\n2. Sipariş listesi\n3. Müşteri bilgilerini göster\n4. Başka müşteri seç");
                    Console.Write("İşlem seçiniz: ");
                    string op = Console.ReadLine();
                    if (op == "1")
                    {
                        Console.Clear();
                        AddNewOrder();
                        Continue();
                    } else if (op == "2")
                    {
                        Console.Clear();
                        DisplayOrder();
                        Continue();
                    } else if (op == "3")
                    {
                        Console.Clear();
                        Console.WriteLine("T.C. No.: {0}\nAd Soyad: {1}\nTelefon Numarası: {2}\nAdres: {3}\n", selectedCustomer.TcNo, selectedCustomer.FullName, selectedCustomer.PhoneNumber, selectedCustomer.Address);
                        Continue();
                    } else if (op == "4")
                    {
                        Console.Clear();
                        appSection = 1;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Lütfen geçerli bir işlem seçiniz.");
                    }
                }
            }

            void InitializeData()
            {
                customers = new List<Customer>() {
                    new Customer
                    {
                        Id = GenerateGuid(), TcNo = "12345678901", FullName = "Zeynel KOZAK",
                        PhoneNumber = "5435434343",
                        Address = "Istanbul"
                    },
                    new Customer
                    {
                        Id = GenerateGuid(), TcNo = "12345678902", FullName = "Ahmet Bey",
                        PhoneNumber = "5325323232",
                        Address = "Ankara"
                    }
                };
                products = new List<Product>() {
                    new Product() {Id = GenerateGuid(), Name = "Product 1", Price = 100},
                    new Product() {Id = GenerateGuid(), Name = "Product 2", Price = 120},
                    new Product() {Id = GenerateGuid(), Name = "Product 3", Price = 150},
                    new Product() {Id = GenerateGuid(), Name = "Product 4", Price = 175},
                };
                orders = new List<Order>() {
                    new Order() {
                        Id = GenerateGuid(),
                        ProductPerOrder = new List<ProductPerOrder>()
                        {
                            new ProductPerOrder()
                            {
                                Id = GenerateGuid(), Product = products[0], Quantity = 5
                            },
                            new ProductPerOrder()
                            {
                                Id = GenerateGuid(), Product = products[3], Quantity = 3
                            },
                        },
                        CustomerId = customers[0].Id
                    },
                    new Order() {
                        Id = GenerateGuid(),
                        ProductPerOrder = new List<ProductPerOrder>()
                        {
                            new ProductPerOrder()
                            {
                                Id = GenerateGuid(), Product = products[1], Quantity = 1
                            },
                            new ProductPerOrder()
                            {
                                Id = GenerateGuid(), Product = products[2], Quantity = 2
                            },
                        },
                        CustomerId = customers[1].Id
                    }
                };
            }
            string GenerateGuid()
            {
                string guid = Guid.NewGuid().ToString().Substring(0, 8);
                return guid;
            }
            void DisplayCustomer()
            {
                if (customers.Count > 0)
                {
                    var table = new ConsoleTable("#", "Ad ve Soyad", "T.C. No.", "Telefon Numarası", "Adres");
                    for (int i = 0; i < customers.Count; i++)
                    {
                        table.AddRow(i + 1, customers[i].FullName, customers[i].TcNo, customers[i].PhoneNumber,
                            customers[i].Address);
                    }
                    table.Write();
                }
                else
                {
                    Console.WriteLine("Hiç müşteri bulunmamaktadır.");
                }
            }
            void DisplayOrder()
            {
                List<Order> ordersByCustomerId = orders.Where(o => o.CustomerId == selectedCustomer.Id).ToList();
                if (ordersByCustomerId.Count > 0)
                {
                    var table = new ConsoleTable("#", "Sipariş Tarihi", "Ürün Adedi", "Toplam Fiyat");
                    for (int i = 0; i < ordersByCustomerId.Count; i++)
                    {
                        int countProductInOrder = ordersByCustomerId[i].ProductPerOrder.Count;
                        float totalPriceInOrder = 0;
                        for (int j = 0; j < ordersByCustomerId[i].ProductPerOrder.Count; j++)
                        {
                            totalPriceInOrder = ordersByCustomerId[i].ProductPerOrder[j].Product.Price *
                                                ordersByCustomerId[i].ProductPerOrder[j].Quantity;
                        }
                        table.AddRow(i + 1, ordersByCustomerId[i].CreatedAt, countProductInOrder, totalPriceInOrder);
                    }
                    table.Write();
                }
                else
                {
                    Console.WriteLine("Sipariş bulunmamaktadır.");
                }
            }
            void DisplayProduct()
            {
                if (products.Count > 0)
                {
                    var table = new ConsoleTable("#", "Ürün Adı", "Satış Fiyatı");
                    for (int i = 0; i < products.Count; i++)
                    {
                        table.AddRow(i + 1, products[i].Name, products[i].Price);
                    }
                    table.Write();
                }
                else
                {
                    Console.WriteLine("Ürün bulunmamaktadır.");
                }
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
                        Id = Guid.NewGuid().ToString().Substring(0, 8), FullName = fullName, PhoneNumber = gsm,
                        Address = address, TcNo = tcNo
                    });
                    Console.Clear();
                    Console.WriteLine("Yeni müşteri oluşturuldu.");
                }
            }
            void AddNewOrder()
            {
                Console.WriteLine("Sipariş oluştur.");
            }
            void Continue()
            {
                Console.WriteLine("Devam etmek için ENTER tuşuna basınız.");
                Console.ReadLine();
            }
        }
    }

    class Customer
    {
        public string Id { get; set; }
        public string TcNo { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }

    class ProductPerOrder
    {
        public string Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }

    class Order
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<ProductPerOrder> ProductPerOrder { get; set; }
        public string CustomerId { get; set; }
    }
}
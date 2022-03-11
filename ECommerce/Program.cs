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
            List<Customer> customers = new List<Customer>();
            List<Product> products = new List<Product>();
            List<Order> orders = new List<Order>();
            Customer selectedCustomer = null;
            InitializeData();
            int appSection = 1;

            while (true)
            {
                switch (appSection)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("# Müşteri Sipariş Hattı");
                        Console.WriteLine(
                            "1. Yeni müşteri oluştur\n2. Müşteri seç\n3. T.C. No ile işlem yap\n4. Çıkış");
                        Console.Write("İşlem seçiniz: ");
                        string op1 = Console.ReadLine();
                        if (op1 == "1")
                            AddNewCustomer();
                        else if (op1 == "2")
                            LoginWithCustomerList();
                        else if (op1 == "3")
                            LoginWithTc();
                        else if (op1 == "4")
                            Exit();
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("İşlem seçiniz!");
                        }

                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Seçilen müşteri: {0}", selectedCustomer.FullName);
                        Console.WriteLine("İşlem Listesi");
                        Console.WriteLine(
                            "1. Sipariş oluştur\n2. Sipariş listesi\n3. Siparişi iptal et\n4. Müşteri bilgilerini göster\n5. Başka müşteri seç\n6. Çıkış");
                        Console.Write("İşlem seçiniz: ");
                        string op2 = Console.ReadLine();
                        if (op2 == "1")
                            AddNewOrder();
                        else if (op2 == "2")
                        {
                            DisplayOrderList();
                            Continue();
                        }
                        else if (op2 == "3")
                            CancelOrder();
                        else if (op2 == "4")
                            DisplayCustomerDetail();
                        else if (op2 == "5")
                            appSection = 1;
                        else if (op2 == "6")
                            Exit();
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Lütfen geçerli bir işlem seçiniz.");
                        }
                        break;
                }
            }

            void InitializeData()
            {
                products.Add(new Product() {Id = GenerateGuid(), Name = "Ürün 1", Price = 100, Stock = 10});
                products.Add(new Product() {Id = GenerateGuid(), Name = "Ürün 2", Price = 110, Stock = 12});
                products.Add(new Product() {Id = GenerateGuid(), Name = "Ürün 3", Price = 120, Stock = 14});
                products.Add(new Product() {Id = GenerateGuid(), Name = "Ürün 4", Price = 130, Stock = 16});
                customers.Add(new Customer()
                {
                    Id = GenerateGuid(), FullName = "Müşteri 1", TcNo = "12345678912", PhoneNumber = "5435434343",
                    Address = "İstanbul"
                });
                customers.Add(new Customer()
                {
                    Id = GenerateGuid(), FullName = "Müşteri 2", TcNo = "12345678913", PhoneNumber = "5325323232",
                    Address = "Ankara"
                });
                orders.Add(new Order()
                {
                    Id = GenerateGuid(), CustomerId = customers[0].Id,
                    Products = new List<ProductInOrder>()
                    {
                        new ProductInOrder() {Id = GenerateGuid(), ProductId = products[0].Id, Quantity = 5},
                        new ProductInOrder() {Id = GenerateGuid(), ProductId = products[1].Id, Quantity = 3}
                    }
                });
                orders.Add(new Order()
                {
                    Id = GenerateGuid(), CustomerId = customers[1].Id,
                    Products = new List<ProductInOrder>()
                    {
                        new ProductInOrder() {Id = GenerateGuid(), ProductId = products[2].Id, Quantity = 8},
                        new ProductInOrder() {Id = GenerateGuid(), ProductId = products[3].Id, Quantity = 5}
                    }
                });
            }

            string GenerateGuid()
            {
                string guid = Guid.NewGuid().ToString().Substring(0, 8);
                return guid;
            }

            void DisplayCustomerList()
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

            void DisplayProductList()
            {
                if (products.Count > 0)
                {
                    var table = new ConsoleTable("#", "Ürün Adı", "Satış Fiyatı", "Stok");
                    for (int i = 0; i < products.Count; i++)
                    {
                        table.AddRow(i + 1, products[i].Name, products[i].Price, products[i].Stock);
                    }

                    table.Write();
                }
                else
                {
                    Console.WriteLine("Ürün bulunmamaktadır.");
                }
            }

            void DisplayOrderList()
            {
                Console.Clear();
                List<Order> ordersByCustomer = orders.Where(o => o.CustomerId == selectedCustomer.Id).ToList();
                if (ordersByCustomer.Count > 0)
                {
                    var table = new ConsoleTable("#", "Sipariş Tarihi", "Ürün Adedi", "Toplam Fiyat");
                    for (int i = 0; i < ordersByCustomer.Count; i++)
                    {
                        int countProductInOrder = ordersByCustomer[i].Products.Count;
                        float totalPriceInOrder = 0;
                        for (int j = 0; j < ordersByCustomer[i].Products.Count; j++)
                        {
                            ProductInOrder pForSelected = ordersByCustomer[i].Products[j];
                            totalPriceInOrder += products.SingleOrDefault(p => p.Id == pForSelected.ProductId).Price *
                                                 pForSelected.Quantity;
                        }

                        table.AddRow(i + 1, ordersByCustomer[i].CreatedAt, countProductInOrder, totalPriceInOrder);
                    }

                    table.Write();
                }
                else
                {
                    Console.WriteLine("Sipariş bulunmamaktadır.");
                }
            }

            void AddNewCustomer()
            {
                string tcNo = "";
                string fullName = "";
                string phoneNumber = "";
                string address = "";

                Console.Clear();
                Console.WriteLine("## Yeni müşteri oluştur ##");
                // TC No.
                while (true)
                {
                    try
                    {
                        Console.Write("T.C. No.: ");
                        long input = long.Parse(Console.ReadLine());
                        if (input.ToString().Length != 11)
                        {
                            Console.WriteLine("Geçerli bir T.C. numarası giriniz.");
                        }
                        else
                        {
                            tcNo = input.ToString();
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Geçerli bir T.C. numarası giriniz.");
                    }
                }

                // Full Name
                while (true)
                {
                    Console.Write("Ad Soyad: ");
                    string a = Console.ReadLine();
                    if (a == "")
                    {
                        Console.WriteLine("Lütfen ad ve soyad bilgisini giriniz.");
                    }
                    else
                    {
                        fullName = a;
                        break;
                    }
                }

                // Phone Number
                while (true)
                {
                    Console.Write("Telefon Numarası: ");
                    string a = Console.ReadLine();
                    if (a == "")
                    {
                        Console.WriteLine("Lütfen telefon numarası bilgisini giriniz.");
                    }
                    else
                    {
                        phoneNumber = a;
                        break;
                    }
                }

                // Address
                while (true)
                {
                    Console.Write("Adres: ");
                    string a = Console.ReadLine();
                    if (a == "")
                    {
                        Console.WriteLine("Lütfen adres bilgisini giriniz.");
                    }
                    else
                    {
                        address = a;
                        break;
                    }
                }

                customers.Add(new Customer()
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8), FullName = fullName, PhoneNumber = phoneNumber,
                    Address = address, TcNo = tcNo
                });
                Console.Clear();
                Console.WriteLine("Yeni müşteri oluşturuldu.");
            }

            void LoginWithCustomerList()
            {
                Console.Clear();
                Console.WriteLine("## Müşteri listesinden müşteri seç ##");
                DisplayCustomerList();
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

            void LoginWithTc()
            {
                Console.Clear();
                Console.WriteLine("## T.C. No ile müşteri seç ##");
                Console.Write("T.C. No. giriniz: ");
                string tc = Console.ReadLine();
                if (tc == "")
                {
                    Console.Clear();
                    Console.WriteLine("T.C. no girilmedi.");
                }
                else
                {
                    Customer customer = customers.SingleOrDefault(c => c.TcNo == tc);
                    if (customer != null)
                    {
                        Console.Clear();
                        selectedCustomer = customer;
                        appSection = 2;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(tc + " nolu T.C. bulunamadı.");
                    }
                }
            }

            void DisplayCustomerDetail()
            {
                Console.Clear();
                Console.WriteLine("T.C. No.: {0}\nAd Soyad: {1}\nTelefon Numarası: {2}\nAdres: {3}\n",
                    selectedCustomer.TcNo, selectedCustomer.FullName, selectedCustomer.PhoneNumber,
                    selectedCustomer.Address);
                Continue();
            }

            void AddNewOrder()
            {
                Console.Clear();
                Order newOrder = new Order()
                    {Id = GenerateGuid(), CustomerId = selectedCustomer.Id, Products = new List<ProductInOrder>()};
                Console.WriteLine("## Sipariş oluştur ##");
                while (true)
                {
                    DisplayProductList();
                    Console.Write("Sipariş etmek istediğiniz ürünü seçiniz (İptal için Q): ");
                    string p = Console.ReadLine();
                    if (p.ToLower() == "q")
                    {
                        orders.Add(newOrder);
                        break;
                    }

                    try
                    {
                        if (int.Parse(p) > 0 && int.Parse(p) - 1 < products.Count)
                        {
                            Product product = products[int.Parse(p) - 1];
                            Console.Write("Adet: ");
                            int quantity = int.Parse(Console.ReadLine());
                            if (CheckStock(product, quantity))
                            {
                                newOrder.Products.Add(new ProductInOrder()
                                    {Id = GenerateGuid(), ProductId = product.Id, Quantity = quantity});
                                product.Stock -= quantity;
                                Console.WriteLine("Ürün siparişe eklendi.");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Yeterli ürün stoğu bulunmamaktadır.");
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Lütfen eklemek istediğiniz ürünü seçiniz.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine("Listeden ürün seçiniz.");
                    }
                }
            }

            void CancelOrder()
            {
                while (true)
                {
                    List<Order> ordersByCustomer = orders.Where(o => o.CustomerId == selectedCustomer.Id).ToList();
                    Console.Clear();
                    DisplayOrderList();
                    if (ordersByCustomer.Count > 0)
                    {
                        Console.Write("İptal etmek istediğiniz siparişi seçiniz (İptal için Q): ");
                        string o = Console.ReadLine();
                        if (o.ToLower() == "q")
                        {
                            break;
                        }
                        else
                        {
                            orders.Remove(ordersByCustomer[int.Parse(o) - 1]);
                            Console.Clear();
                            Console.WriteLine("Siparişiniz iptal edildi.");
                            Continue();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Siparişiniz bulunmamaktadır!");
                        Continue();
                        break;
                    }
                }
            }

            bool CheckStock(Product product, int quantity)
            {
                if (product.Stock < quantity)
                    return false;
                return true;
            }

            void Continue()
            {
                Console.WriteLine("Devam etmek için ENTER tuşuna basınız.");
                Console.ReadLine();
            }

            void Exit()
            {
                Console.WriteLine("Güle güle...");
                Environment.Exit(0);
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

    class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<ProductInOrder> Products { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }

    class ProductInOrder
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
    }
}
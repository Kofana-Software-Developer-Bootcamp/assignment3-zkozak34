using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            int appSection = 1;
            string[] operations = {"Sipariş ekle", "Siparişleri listele", "Müşteri bilgilerini göster"};
            // Dummy Data
            List<Customer> customers = new List<Customer>() {
                new Customer
                {
                    Id = Guid.NewGuid(), TcNo = "12345678901", Fullname = "Zeynel KOZAK", Gsm = "5435434343",
                    Address = "Istanbul"
                },
                new Customer
                {
                    Id = Guid.NewGuid(), TcNo = "12345678902", Fullname = "Ahmet Bey", Gsm = "5325323232",
                    Address = "Ankara"
                },
                new Customer
                {
                    Id = Guid.NewGuid(), TcNo = "12345678903", Fullname = "Mehmet Bey", Gsm = "5215212121",
                    Address = "Konya"
                },
                new Customer
                {
                    Id = Guid.NewGuid(), TcNo = "12345678904", Fullname = "Ali Bey", Gsm = "5105101010",
                    Address = "Eskişehir"
                },
            };
            List<Product> products = new List<Product>() {
                new Product {Id = Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10},
                new Product {Id = Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 12},
                new Product {Id = Guid.NewGuid(), Name = "Product 3", Price = 300, Stock = 14},
                new Product {Id = Guid.NewGuid(), Name = "Product 4", Price = 400, Stock = 16},
                new Product {Id = Guid.NewGuid(), Name = "Product 5", Price = 500, Stock = 18}
            };
            List<Order> orders = new List<Order>() {
                new Order() {Id = Guid.NewGuid(), CustomerId = customers[0].Id, Products = {products[0], products[3]}},
                new Order() {Id = Guid.NewGuid(), CustomerId = customers[1].Id, Products = {products[1], products[2]}},
                new Order() {Id = Guid.NewGuid(), CustomerId = customers[2].Id, Products = {products[2], products[3]}},
                new Order() {Id = Guid.NewGuid(), CustomerId = customers[3].Id, Products = {products[2], products[4]}},
            };
            
            Customer GetCustomer(string tcNo)
            {
                return customers.SingleOrDefault(c => c.TcNo == tcNo);
            }
        }
    }

    class Customer
    {
        public Guid Id { get; set; }
        public string TcNo { get; set; }
        public string Fullname { get; set; }
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
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
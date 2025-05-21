using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; }
    public decimal Price { get; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public interface IDiscount
{
    decimal ApplyDiscount(decimal total);
}

public class PercentageDiscount : IDiscount
{
    private readonly decimal percentage;

    public PercentageDiscount(decimal percentage)
    {
        this.percentage = percentage;
    }

    public decimal ApplyDiscount(decimal total)
    {
        return total - (total * percentage / 100);
    }
}

public class FixedDiscount : IDiscount
{
    private readonly decimal amount;

    public FixedDiscount(decimal amount)
    {
        this.amount = amount;
    }

    public decimal ApplyDiscount(decimal total)
    {
        return Math.Max(0, total - amount);
    }
}

public class Order
{
    private readonly List<Product> products;
    private readonly IDiscount discount;

    public Order(IDiscount discount = null)
    {
        products = new List<Product>();
        this.discount = discount; 
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach (var product in products)
        {
            total += product.Price;
        }
        return total;
    }

    public decimal CalculateFinalTotal()
    {
        decimal total = CalculateTotal();
        return discount != null ? discount.ApplyDiscount(total) : total;
    }

    public IReadOnlyList<Product> GetProducts() => products.AsReadOnly();
}

public class ReceiptPrinter
{
    public void PrintReceipt(Order order)
    {
        Console.WriteLine("\n=== Order Receipt ===");
        Console.WriteLine("Products:");
        foreach (var product in order.GetProducts())
        {
            Console.WriteLine($"- {product.Name}: {product.Price:C}");
        }
        decimal total = order.CalculateTotal();
        decimal finalTotal = order.CalculateFinalTotal();
        decimal discountAmount = total - finalTotal;

        Console.WriteLine($"\nTotal Amount: {total:C}");
        Console.WriteLine($"Discount: {discountAmount:C}");
        Console.WriteLine($"Final Amount: {finalTotal:C}");
        Console.WriteLine("=================");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var products = new List<Product>
        {
            new Product("Book", 20m),
            new Product("Pen", 5m),
            new Product("Notebook", 10m)
        };

        Console.WriteLine("Welcome to the Order System!");
        Console.WriteLine("Available Products:");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price:C}");
        }

        Console.WriteLine("\nSelect discount type (1: 10% off, 2: $5 off, 3: No discount):");
        IDiscount discount = null;
        string discountChoice = Console.ReadLine();
        switch (discountChoice)
        {
            case "1":
                discount = new PercentageDiscount(10); 
                break;
            case "2":
                discount = new FixedDiscount(5); 
                break;
            default:
                discount = null; 
                break;
        }

        Order order = new Order(discount);

        while (true)
        {
            Console.WriteLine("\nEnter product number to add (or 0 to finish):");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 0)
                break;

            if (choice > 0 && choice <= products.Count)
            {
                order.AddProduct(products[choice - 1]);
                Console.WriteLine($"{products[choice - 1].Name} added to the order.");
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }

        ReceiptPrinter printer = new ReceiptPrinter();
        printer.PrintReceipt(order);
    }
}
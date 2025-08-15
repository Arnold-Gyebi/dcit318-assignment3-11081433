using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment.Q3
{
    public interface IInventoryItem { int Id { get; } string Name { get; } int Quantity { get; set; } }

    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; } public string Name { get; } public int Quantity { get; set; }
        public string Brand { get; } public int WarrantyMonths { get; }
        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths) { Id = id; Name = name; Quantity = quantity; Brand = brand; WarrantyMonths = warrantyMonths; }
        public override string ToString() => $"Electronic(Id={Id}, Name={Name}, Qty={Quantity}, Brand={Brand}, Warranty={WarrantyMonths}m)";
    }

    public class GroceryItem : IInventoryItem
    {
        public int Id { get; } public string Name { get; } public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }
        public GroceryItem(int id, string name, int quantity, DateTime expiryDate) { Id = id; Name = name; Quantity = quantity; ExpiryDate = expiryDate; }
        public override string ToString() => $"Grocery(Id={Id}, Name={Name}, Qty={Quantity}, Expiry={ExpiryDate:d})";
    }

    public class DuplicateItemException : Exception { public DuplicateItemException(string msg) : base(msg) { } }
    public class ItemNotFoundException : Exception { public ItemNotFoundException(string msg) : base(msg) { } }
    public class InvalidQuantityException : Exception { public InvalidQuantityException(string msg) : base(msg) { } }

    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();
        public void AddItem(T item) { if (_items.ContainsKey(item.Id)) throw new DuplicateItemException($"Item with Id={item.Id} already exists."); _items[item.Id] = item; }
        public T GetItemById(int id) { if (!_items.TryGetValue(id, out var item)) throw new ItemNotFoundException($"Item with Id={id} not found."); return item; }
        public void RemoveItem(int id) { if (!_items.Remove(id)) throw new ItemNotFoundException($"Cannot remove. Item with Id={id} not found."); }
        public List<T> GetAllItems() => _items.Values.ToList();
        public void UpdateQuantity(int id, int newQuantity) { if (newQuantity < 0) throw new InvalidQuantityException("Quantity cannot be negative."); var item = GetItemById(id); item.Quantity = newQuantity; }
    }

    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Smartphone", 20, "TechCo", 24));
            _electronics.AddItem(new ElectronicItem(2, "Laptop", 10, "ComputeX", 12));
            _electronics.AddItem(new ElectronicItem(3, "Headphones", 50, "SoundMax", 6));

            _groceries.AddItem(new GroceryItem(101, "Rice (5kg)", 40, DateTime.Today.AddMonths(12)));
            _groceries.AddItem(new GroceryItem(102, "Milk (1L)", 80, DateTime.Today.AddDays(10)));
            _groceries.AddItem(new GroceryItem(103, "Eggs (tray)", 25, DateTime.Today.AddDays(14)));
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem { foreach (var item in repo.GetAllItems()) Console.WriteLine(item); }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try { var current = repo.GetItemById(id).Quantity; repo.UpdateQuantity(id, current + quantity); Console.WriteLine($"Stock increased for Id={id} by {quantity}. New Qty={current + quantity}"); }
            catch (Exception ex) { Console.WriteLine($"[IncreaseStock Error] {ex.Message}"); }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try { repo.RemoveItem(id); Console.WriteLine($"Item Id={id} removed."); }
            catch (Exception ex) { Console.WriteLine($"[RemoveItem Error] {ex.Message}"); }
        }

        public void Run()
        {
            Console.WriteLine("=== Q3: Warehouse Inventory ===");
            SeedData();
            Console.WriteLine("-- Grocery Items --"); PrintAllItems(_groceries);
            Console.WriteLine("-- Electronic Items --"); PrintAllItems(_electronics);

            try { _electronics.AddItem(new ElectronicItem(1, "Duplicate Phone", 5, "XBrand", 12)); }
            catch (DuplicateItemException ex) { Console.WriteLine($"[Duplicate Add Caught] {ex.Message}"); }

            RemoveItemById(_groceries, 999);

            try { _electronics.UpdateQuantity(2, -5); }
            catch (InvalidQuantityException ex) { Console.WriteLine($"[Invalid Qty Caught] {ex.Message}"); }

            IncreaseStock(_groceries, 101, 10);
            Console.WriteLine();
        }
    }
}

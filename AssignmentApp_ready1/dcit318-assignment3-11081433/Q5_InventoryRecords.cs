using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Assignment.Q5
{
    public interface IInventoryEntity { int Id { get; } }

    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item) => _log.Add(item);
        public List<T> GetAll() => new(_log);

        public void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
                using var writer = new StreamWriter(_filePath, false);
                writer.Write(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SaveToFile Error] {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("[LoadFromFile] File not found; starting with empty log.");
                    return;
                }

                using var reader = new StreamReader(_filePath);
                var json = reader.ReadToEnd();
                var loaded = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

                _log.Clear();
                _log.AddRange(loaded);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadFromFile Error] {ex.Message}");
            }
        }
    }

    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp(string path)
        {
            _logger = new InventoryLogger<InventoryItem>(path);
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "USB Cable", 50, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Notebook", 200, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Stapler", 35, DateTime.Now));
            _logger.Add(new InventoryItem(4, "Marker Pen", 120, DateTime.Now));
            _logger.Add(new InventoryItem(5, "Flash Drive 32GB", 60, DateTime.Now));
        }

        public void SaveData() => _logger.SaveToFile();
        public void LoadData() => _logger.LoadFromFile();

        public void PrintAllItems()
        {
            var all = _logger.GetAll();
            if (all.Count == 0) Console.WriteLine("(no items loaded)");
            foreach (var item in all)
                Console.WriteLine($"InventoryItem(Id={item.Id}, Name={item.Name}, Qty={item.Quantity}, DateAdded={item.DateAdded})");
        }

        public void Run()
        {
            Console.WriteLine("=== Q5: Inventory Records ===");
            string baseDir = Path.Combine(Directory.GetCurrentDirectory(), "q5_io");
            Directory.CreateDirectory(baseDir);
            string filePath = Path.Combine(baseDir, "inventory.json");

            _logger = new InventoryLogger<InventoryItem>(filePath);

            SeedSampleData();
            SaveData();

            _logger = new InventoryLogger<InventoryItem>(filePath);
            LoadData();
            PrintAllItems();
            Console.WriteLine();
        }
    }
}

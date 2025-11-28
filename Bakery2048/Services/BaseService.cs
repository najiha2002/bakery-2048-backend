using System.Text.Json;

namespace Bakery2048.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected List<T> items;
        protected readonly string dataFilePath;

        protected BaseService(List<T> itemList, string filePath)
        {
            items = itemList;
            dataFilePath = filePath;
            LoadFromFile();
        }

        protected void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(items, options);
                File.WriteAllText(dataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        protected void LoadFromFile()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    string jsonString = File.ReadAllText(dataFilePath);
                    var loadedItems = JsonSerializer.Deserialize<List<T>>(jsonString);

                    if (loadedItems != null)
                    {
                        items.Clear();
                        items.AddRange(loadedItems);
                        Console.WriteLine($"✓ Loaded {items.Count} {typeof(T).Name.ToLower()}(s) from file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        protected void PauseForUser()
        {
            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }

        public abstract void ShowMenu();
    }
}

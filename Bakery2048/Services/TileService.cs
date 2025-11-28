using Bakery2048.Models;
using Bakery2048.Utilities;

namespace Bakery2048.Services;

public class TileService : BaseService<Tile>
{
    public TileService(List<Tile> tileList) : base(tileList, "tiles.json")
    {
    }

    public override void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleUI.ShowTitle("🍰 Tile Management");
            ConsoleUI.MenuOption("1", "Add New Tile");
            ConsoleUI.MenuOption("2", "View All Tiles");
            ConsoleUI.MenuOption("3", "Search Tile");
            ConsoleUI.MenuOption("4", "Update Tile");
            ConsoleUI.MenuOption("5", "Delete Tile");
            ConsoleUI.MenuOption("6", "View Tile Statistics");
            ConsoleUI.MenuOption("0", "Back to Main Menu");
            Console.WriteLine();

            string? choice = ConsoleUI.Prompt("Enter your choice");

            switch (choice)
            {
                case "1":
                    AddTile();
                    break;
                case "2":
                    ViewAllTiles();
                    break;
                case "3":
                    SearchTile();
                    break;
                case "4":
                    UpdateTile();
                    break;
                case "5":
                    DeleteTile();
                    break;
                case "6":
                    ViewTileStatistics();
                    break;
                case "0":
                    return;
                default:
                    ConsoleUI.Error("Invalid choice. Please try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    private void AddTile()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Add New Tile");

        string? itemName = ConsoleUI.Prompt("Enter item name (e.g., Flour, Cookie)");
        if (string.IsNullOrWhiteSpace(itemName))
        {
            ConsoleUI.Warning("Item name cannot be empty.");
            PauseForUser();
            return;
        }

        // Check for duplicate
        if (items.Any(t => t.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
        {
            ConsoleUI.Warning($"A tile with name '{itemName}' already exists.");
            PauseForUser();
            return;
        }

        Console.Write("Enter tile value (e.g., 2, 4, 8): ");
        if (!int.TryParse(Console.ReadLine(), out int tileValue) || tileValue <= 0)
        {
            ConsoleUI.Error("Invalid tile value. Must be a positive number.");
            PauseForUser();
            return;
        }

        // Check for duplicate tile value (only among active tiles)
        if (items.Any(t => t.IsActive && t.TileValue == tileValue))
        {
            ConsoleUI.Warning($"A tile with value {tileValue} already exists and is active.");
            PauseForUser();
            return;
        }

        string? icon = ConsoleUI.Prompt("Enter icon/emoji (e.g., 🌾, 🍪)");
        
        string? color;
        while (true)
        {
            color = ConsoleUI.Prompt("Enter color hex code (e.g., #F5DEB3)");
            
            if (string.IsNullOrWhiteSpace(color))
            {
                color = "#FFFFFF";
                break;
            }
            
            // Validate hex color format
            if (System.Text.RegularExpressions.Regex.IsMatch(color, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
            {
                break;
            }
            
            ConsoleUI.Error("Invalid hex color format. Please use format #RRGGBB (e.g., #FF5733) or #RGB (e.g., #F57)");
        }

        Console.Write("Is this a special item? (y/n): ");
        bool isSpecialItem = Console.ReadLine()?.Trim().ToLower() == "y";

        var newTile = new Tile(itemName, tileValue)
        {
            Icon = icon ?? string.Empty,
            Color = color ?? "#FFFFFF",
            IsSpecialItem = isSpecialItem
        };

        items.Add(newTile);
        SaveToFile();

        ConsoleUI.Success($"Tile '{itemName}' added successfully!");
        Console.WriteLine();
        ConsoleUI.KeyValue("Tile ID", newTile.TileId.ToString());
        ConsoleUI.KeyValue("Item Name", newTile.ItemName);
        ConsoleUI.KeyValue("Tile Value", newTile.TileValue.ToString());
        ConsoleUI.KeyValue("Icon", newTile.Icon);
        ConsoleUI.KeyValue("Special Item", newTile.IsSpecialItem ? "Yes" : "No");

        PauseForUser();
    }

    private void ViewAllTiles()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("All Tiles");

        if (items.Count == 0)
        {
            ConsoleUI.Warning("No tiles found in the system.");
            PauseForUser();
            return;
        }

        var activeTiles = items.Where(t => t.IsActive).OrderBy(t => t.TileValue).ToList();
        var inactiveTiles = items.Where(t => !t.IsActive).OrderBy(t => t.TileValue).ToList();

        if (activeTiles.Count > 0)
        {
            Console.WriteLine();
            ConsoleUI.Info($"Active Tiles: {activeTiles.Count}");
            Console.WriteLine($"{"Icon",-8} {"Item Name",-20} {"Value",-10} {"Color",-12} {"Special",-10}");
            ConsoleUI.Divider();

            foreach (var tile in activeTiles)
            {
                Console.WriteLine($"{tile.Icon,-8} {tile.ItemName,-20} {tile.TileValue,-10} {tile.Color,-12} {(tile.IsSpecialItem ? "★" : "-"),-10}");
            }
        }

        if (inactiveTiles.Count > 0)
        {
            Console.WriteLine();
            ConsoleUI.Warning($"Inactive Tiles: {inactiveTiles.Count}");
            Console.WriteLine($"{"Icon",-8} {"Item Name",-20} {"Value",-10} {"Status",-10}");
            ConsoleUI.Divider();

            foreach (var tile in inactiveTiles)
            {
                Console.WriteLine($"{tile.Icon,-8} {tile.ItemName,-20} {tile.TileValue,-10} {"Inactive",-10}");
            }
        }

        PauseForUser();
    }

    private void SearchTile()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Search Tile");

        string? searchTerm = ConsoleUI.Prompt("Enter tile name or value to search");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var results = items.Where(t =>
            t.ItemName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            t.TileValue.ToString() == searchTerm
        ).ToList();

        if (results.Count == 0)
        {
            ConsoleUI.Warning($"No tiles found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Success($"Found {results.Count} tile(s):");
        Console.WriteLine();

        foreach (var tile in results)
        {
            ConsoleUI.KeyValue("Tile ID", tile.TileId.ToString());
            ConsoleUI.KeyValue("Item Name", tile.ItemName);
            ConsoleUI.KeyValue("Tile Value", tile.TileValue.ToString());
            ConsoleUI.KeyValue("Icon", tile.Icon);
            ConsoleUI.KeyValue("Color", tile.Color);
            ConsoleUI.KeyValue("Special Item", tile.IsSpecialItem ? "Yes" : "No");
            ConsoleUI.KeyValue("Status", tile.IsActive ? "Active" : "Inactive");
            ConsoleUI.KeyValue("Created", tile.DateCreated.ToString("yyyy-MM-dd"));
            ConsoleUI.Divider();
        }

        PauseForUser();
    }

    private void UpdateTile()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Update Tile");

        string? searchTerm = ConsoleUI.Prompt("Enter tile name or value to update");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var tile = items.FirstOrDefault(t =>
            t.ItemName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            t.TileValue.ToString() == searchTerm
        );

        if (tile == null)
        {
            ConsoleUI.Error($"No tile found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Info($"Updating tile: {tile.ItemName} (Value: {tile.TileValue})");
        Console.WriteLine();

        ConsoleUI.MenuOption("1", "Update Item Name");
        ConsoleUI.MenuOption("2", "Update Tile Value");
        ConsoleUI.MenuOption("3", "Update Icon");
        ConsoleUI.MenuOption("4", "Update Color");
        ConsoleUI.MenuOption("5", "Toggle Special Item Status");
        ConsoleUI.MenuOption("6", "Toggle Active Status");
        ConsoleUI.MenuOption("0", "Cancel");
        Console.WriteLine();

        string? choice = ConsoleUI.Prompt("Select field to update");

        try
        {
            switch (choice)
            {
                case "1":
                    string? newName = ConsoleUI.Prompt("Enter new item name");
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        tile.ItemName = newName;
                        tile.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Item name updated successfully!");
                    }
                    break;

                case "2":
                    Console.Write("Enter new tile value: ");
                    if (int.TryParse(Console.ReadLine(), out int newValue) && newValue > 0)
                    {
                        tile.TileValue = newValue;
                        tile.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Tile value updated successfully!");
                    }
                    else
                    {
                        ConsoleUI.Error("Invalid tile value.");
                    }
                    break;

                case "3":
                    string? newIcon = ConsoleUI.Prompt("Enter new icon/emoji");
                    if (!string.IsNullOrWhiteSpace(newIcon))
                    {
                        tile.Icon = newIcon;
                        tile.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Icon updated successfully!");
                    }
                    break;

                case "4":
                    string? newColor = ConsoleUI.Prompt("Enter new color hex code");
                    if (!string.IsNullOrWhiteSpace(newColor))
                    {
                        // Validate hex color format
                        if (System.Text.RegularExpressions.Regex.IsMatch(newColor, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
                        {
                            tile.Color = newColor;
                            tile.UpdateModifiedDate();
                            SaveToFile();
                            ConsoleUI.Success("Color updated successfully!");
                        }
                        else
                        {
                            ConsoleUI.Error("Invalid hex color format. Please use format #RRGGBB (e.g., #FF5733) or #RGB (e.g., #F57)");
                        }
                    }
                    break;

                case "5":
                    tile.IsSpecialItem = !tile.IsSpecialItem;
                    tile.UpdateModifiedDate();
                    SaveToFile();
                    ConsoleUI.Success($"Special item status: {(tile.IsSpecialItem ? "Yes" : "No")}");
                    break;

                case "6":
                    if (tile.IsActive)
                        tile.Deactivate();
                    else
                        tile.Activate();
                    SaveToFile();
                    ConsoleUI.Success($"Tile is now {(tile.IsActive ? "Active" : "Inactive")}");
                    break;

                case "0":
                    ConsoleUI.Info("Update cancelled.");
                    break;

                default:
                    ConsoleUI.Error("Invalid choice.");
                    break;
            }
        }
        catch (Exception ex)
        {
            ConsoleUI.Error($"An error occurred while updating the tile: {ex.Message}");
        }

        PauseForUser();
    }

    private void DeleteTile()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Delete Tile");

        string? searchTerm = ConsoleUI.Prompt("Enter tile name or value to delete");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var tile = items.FirstOrDefault(t =>
            t.ItemName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            t.TileValue.ToString() == searchTerm
        );

        if (tile == null)
        {
            ConsoleUI.Error($"No tile found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Warning($"Are you sure you want to delete tile '{tile.ItemName}' (Value: {tile.TileValue})?");
        if (ConsoleUI.Confirm("This action cannot be undone"))
        {
            items.Remove(tile);
            SaveToFile();
            ConsoleUI.Success($"Tile '{tile.ItemName}' deleted successfully!");
        }
        else
        {
            ConsoleUI.Info("Deletion cancelled.");
        }

        PauseForUser();
    }

    private void ViewTileStatistics()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Tile Statistics");

        if (items.Count == 0)
        {
            ConsoleUI.Warning("No tiles found in the system.");
            PauseForUser();
            return;
        }

        var activeTiles = items.Where(t => t.IsActive).ToList();
        var specialTiles = items.Where(t => t.IsSpecialItem).ToList();

        Console.WriteLine();
        ConsoleUI.StatBox("Total Tiles", items.Count.ToString());
        ConsoleUI.StatBox("Active Tiles", activeTiles.Count.ToString());
        ConsoleUI.StatBox("Inactive Tiles", (items.Count - activeTiles.Count).ToString());
        ConsoleUI.StatBox("Special Tiles", specialTiles.Count.ToString());
        Console.WriteLine();

        if (activeTiles.Count > 0)
        {
            var minValue = activeTiles.Min(t => t.TileValue);
            var maxValue = activeTiles.Max(t => t.TileValue);
            var avgValue = activeTiles.Average(t => t.TileValue);

            ConsoleUI.Info("Value Statistics:");
            ConsoleUI.KeyValue("Lowest Value", minValue.ToString());
            ConsoleUI.KeyValue("Highest Value", maxValue.ToString());
            ConsoleUI.KeyValue("Average Value", avgValue.ToString("F1"));
        }

        if (specialTiles.Count > 0)
        {
            Console.WriteLine();
            ConsoleUI.Header("Special Tiles");
            foreach (var tile in specialTiles)
            {
                ConsoleUI.KeyValue($"{tile.Icon} {tile.ItemName}", $"Value: {tile.TileValue}");
            }
        }

        PauseForUser();
    }
}

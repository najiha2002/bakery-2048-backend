using Bakery2048.Models;
using Bakery2048.Utilities;

namespace Bakery2048.Services;

public class PowerUpService : BaseService<PowerUp>
{
    public PowerUpService(List<PowerUp> powerUpList) : base(powerUpList, "powerups.json")
    {
    }

    public override void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleUI.ShowTitle("⚡ Power-Up Management");
            ConsoleUI.MenuOption("1", "Add New Power-Up");
            ConsoleUI.MenuOption("2", "View All Power-Ups");
            ConsoleUI.MenuOption("3", "Search Power-Up");
            ConsoleUI.MenuOption("4", "Update Power-Up");
            ConsoleUI.MenuOption("5", "Delete Power-Up");
            ConsoleUI.MenuOption("6", "View Power-Up Statistics");
            ConsoleUI.MenuOption("0", "Back to Main Menu");
            Console.WriteLine();

            string? choice = ConsoleUI.Prompt("Enter your choice");

            switch (choice)
            {
                case "1":
                    AddPowerUp();
                    break;
                case "2":
                    ViewAllPowerUps();
                    break;
                case "3":
                    SearchPowerUp();
                    break;
                case "4":
                    UpdatePowerUp();
                    break;
                case "5":
                    DeletePowerUp();
                    break;
                case "6":
                    ViewPowerUpStatistics();
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

    private void AddPowerUp()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Add New Power-Up");

        string? powerUpName = ConsoleUI.Prompt("Enter power-up name (e.g., Double Score, Undo Move)");
        if (string.IsNullOrWhiteSpace(powerUpName))
        {
            ConsoleUI.Warning("Power-up name cannot be empty.");
            PauseForUser();
            return;
        }

        // Check for duplicate name (only among active power-ups)
        if (items.Any(p => p.IsActive && p.PowerUpName.Equals(powerUpName, StringComparison.OrdinalIgnoreCase)))
        {
            ConsoleUI.Warning($"A power-up with name '{powerUpName}' already exists.");
            PauseForUser();
            return;
        }

        // Display power-up types
        Console.WriteLine();
        ConsoleUI.Info("Available Power-Up Types:");
        ConsoleUI.MenuOption("1", "ScoreBoost - Multiplies score earned");
        ConsoleUI.MenuOption("2", "TimeExtension - Extends gameplay time");
        ConsoleUI.MenuOption("3", "Undo - Undo previous moves");
        ConsoleUI.MenuOption("4", "SwapTiles - Swap tile positions");
        Console.WriteLine();

        Console.Write("Select power-up type (1-4): ");
        if (!int.TryParse(Console.ReadLine(), out int typeChoice) || typeChoice < 1 || typeChoice > 4)
        {
            ConsoleUI.Error("Invalid power-up type selection.");
            PauseForUser();
            return;
        }

        PowerUpType type = typeChoice switch
        {
            1 => PowerUpType.ScoreBoost,
            2 => PowerUpType.TimeExtension,
            3 => PowerUpType.Undo,
            4 => PowerUpType.SwapTiles,
            _ => PowerUpType.ScoreBoost
        };

        Console.Write("Enter cost (points): ");
        if (!int.TryParse(Console.ReadLine(), out int cost) || cost < 0)
        {
            ConsoleUI.Error("Invalid cost. Must be a non-negative number.");
            PauseForUser();
            return;
        }

        string? description = ConsoleUI.Prompt("Enter description");
        
        Console.Write("Enter duration (seconds/moves): ");
        if (!int.TryParse(Console.ReadLine(), out int duration) || duration < 1)
        {
            ConsoleUI.Error("Invalid duration. Must be a positive number.");
            PauseForUser();
            return;
        }

        Console.Write("Enter cooldown (moves): ");
        if (!int.TryParse(Console.ReadLine(), out int cooldown) || cooldown < 0)
        {
            ConsoleUI.Error("Invalid cooldown. Must be a non-negative number.");
            PauseForUser();
            return;
        }

        Console.Write("Enter effect multiplier (e.g., 2.0 for double): ");
        if (!double.TryParse(Console.ReadLine(), out double multiplier) || multiplier < 0)
        {
            ConsoleUI.Error("Invalid multiplier. Must be a non-negative number.");
            PauseForUser();
            return;
        }

        Console.Write("Is this power-up unlocked? (y/n): ");
        bool isUnlocked = Console.ReadLine()?.Trim().ToLower() == "y";

        var newPowerUp = new PowerUp(powerUpName, type, cost)
        {
            Description = description ?? string.Empty,
            Duration = duration,
            Cooldown = cooldown,
            EffectMultiplier = multiplier,
            IsUnlocked = isUnlocked
        };

        items.Add(newPowerUp);
        SaveToFile();

        ConsoleUI.Success($"Power-up '{powerUpName}' added successfully!");
        Console.WriteLine();
        ConsoleUI.KeyValue("Power-Up ID", newPowerUp.PowerUpId.ToString());
        ConsoleUI.KeyValue("Name", newPowerUp.PowerUpName);
        ConsoleUI.KeyValue("Type", newPowerUp.PowerUpType.ToString());
        ConsoleUI.KeyValue("Cost", $"{newPowerUp.Cost} points");
        ConsoleUI.KeyValue("Duration", $"{newPowerUp.Duration} moves");
        ConsoleUI.KeyValue("Cooldown", $"{newPowerUp.Cooldown} moves");
        ConsoleUI.KeyValue("Effect Multiplier", $"{newPowerUp.EffectMultiplier}x");
        ConsoleUI.KeyValue("Unlocked", newPowerUp.IsUnlocked ? "Yes" : "No");

        PauseForUser();
    }

    private void ViewAllPowerUps()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("All Power-Ups");

        if (items.Count == 0)
        {
            ConsoleUI.Warning("No power-ups found in the system.");
            PauseForUser();
            return;
        }

        var activePowerUps = items.Where(p => p.IsActive).OrderBy(p => p.Cost).ToList();
        var inactivePowerUps = items.Where(p => !p.IsActive).OrderBy(p => p.Cost).ToList();

        if (activePowerUps.Count > 0)
        {
            Console.WriteLine();
            ConsoleUI.Info($"Active Power-Ups: {activePowerUps.Count}");
            Console.WriteLine($"{"Icon",-6} {"Name",-22} {"Type",-18} {"Cost",-10} {"Cooldown",-12} {"Unlocked",-10}");
            ConsoleUI.Divider();

            foreach (var powerUp in activePowerUps)
            {
                string unlocked = powerUp.IsUnlocked ? "✓" : "✗";
                string icon = string.IsNullOrEmpty(powerUp.IconUrl) ? "-" : powerUp.IconUrl;
                Console.WriteLine($"{icon,-6} {powerUp.PowerUpName,-22} {powerUp.PowerUpType,-18} {powerUp.Cost,-10} {powerUp.Cooldown,-12} {unlocked,-10}");
            }
        }

        if (inactivePowerUps.Count > 0)
        {
            Console.WriteLine();
            ConsoleUI.Warning($"Inactive Power-Ups: {inactivePowerUps.Count}");
            Console.WriteLine($"{"Name",-25} {"Type",-18} {"Status",-10}");
            ConsoleUI.Divider();

            foreach (var powerUp in inactivePowerUps)
            {
                Console.WriteLine($"{powerUp.PowerUpName,-25} {powerUp.PowerUpType,-18} {"Inactive",-10}");
            }
        }

        PauseForUser();
    }

    private void SearchPowerUp()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Search Power-Up");

        string? searchTerm = ConsoleUI.Prompt("Enter power-up name to search");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var results = items.Where(p =>
            p.PowerUpName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        if (results.Count == 0)
        {
            ConsoleUI.Warning($"No power-ups found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Success($"Found {results.Count} power-up(s):");
        Console.WriteLine();

        foreach (var powerUp in results)
        {
            ConsoleUI.KeyValue("Power-Up ID", powerUp.PowerUpId.ToString());
            ConsoleUI.KeyValue("Name", powerUp.PowerUpName);
            ConsoleUI.KeyValue("Type", powerUp.PowerUpType.ToString());
            ConsoleUI.KeyValue("Description", powerUp.Description);
            ConsoleUI.KeyValue("Cost", $"{powerUp.Cost} points");
            ConsoleUI.KeyValue("Duration", $"{powerUp.Duration} moves");
            ConsoleUI.KeyValue("Cooldown", $"{powerUp.Cooldown} moves");
            ConsoleUI.KeyValue("Effect Multiplier", $"{powerUp.EffectMultiplier}x");
            ConsoleUI.KeyValue("Usage Count", powerUp.UsageCount.ToString());
            ConsoleUI.KeyValue("Unlocked", powerUp.IsUnlocked ? "Yes" : "No");
            ConsoleUI.KeyValue("Status", powerUp.IsActive ? "Active" : "Inactive");
            ConsoleUI.KeyValue("Created", powerUp.DateCreated.ToString("yyyy-MM-dd"));
            ConsoleUI.Divider();
        }

        PauseForUser();
    }

    private void UpdatePowerUp()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Update Power-Up");

        string? searchTerm = ConsoleUI.Prompt("Enter power-up name to update");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var powerUp = items.FirstOrDefault(p =>
            p.PowerUpName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        if (powerUp == null)
        {
            ConsoleUI.Error($"No power-up found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Info($"Updating power-up: {powerUp.PowerUpName} (Type: {powerUp.PowerUpType})");
        Console.WriteLine();

        ConsoleUI.MenuOption("1", "Update Name");
        ConsoleUI.MenuOption("2", "Update Cost");
        ConsoleUI.MenuOption("3", "Update Duration");
        ConsoleUI.MenuOption("4", "Update Cooldown");
        ConsoleUI.MenuOption("5", "Update Effect Multiplier");
        ConsoleUI.MenuOption("6", "Update Description");
        ConsoleUI.MenuOption("7", "Toggle Unlock Status");
        ConsoleUI.MenuOption("8", "Toggle Active Status");
        ConsoleUI.MenuOption("0", "Cancel");
        Console.WriteLine();

        string? choice = ConsoleUI.Prompt("Select field to update");

        try
        {
            switch (choice)
            {
                case "1":
                    string? newName = ConsoleUI.Prompt("Enter new name");
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        powerUp.PowerUpName = newName;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Name updated successfully!");
                    }
                    break;

                case "2":
                    Console.Write("Enter new cost: ");
                    if (int.TryParse(Console.ReadLine(), out int newCost) && newCost >= 0)
                    {
                        powerUp.Cost = newCost;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Cost updated successfully!");
                    }
                    else
                    {
                        ConsoleUI.Error("Invalid cost.");
                    }
                    break;

                case "3":
                    Console.Write("Enter new duration: ");
                    if (int.TryParse(Console.ReadLine(), out int newDuration) && newDuration > 0)
                    {
                        powerUp.Duration = newDuration;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Duration updated successfully!");
                    }
                    else
                    {
                        ConsoleUI.Error("Invalid duration.");
                    }
                    break;

                case "4":
                    Console.Write("Enter new cooldown: ");
                    if (int.TryParse(Console.ReadLine(), out int newCooldown) && newCooldown >= 0)
                    {
                        powerUp.Cooldown = newCooldown;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Cooldown updated successfully!");
                    }
                    else
                    {
                        ConsoleUI.Error("Invalid cooldown.");
                    }
                    break;

                case "5":
                    Console.Write("Enter new effect multiplier: ");
                    if (double.TryParse(Console.ReadLine(), out double newMultiplier) && newMultiplier >= 0)
                    {
                        powerUp.EffectMultiplier = newMultiplier;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Effect multiplier updated successfully!");
                    }
                    else
                    {
                        ConsoleUI.Error("Invalid multiplier.");
                    }
                    break;

                case "6":
                    string? newDescription = ConsoleUI.Prompt("Enter new description");
                    if (!string.IsNullOrWhiteSpace(newDescription))
                    {
                        powerUp.Description = newDescription;
                        powerUp.UpdateModifiedDate();
                        SaveToFile();
                        ConsoleUI.Success("Description updated successfully!");
                    }
                    break;

                case "7":
                    powerUp.IsUnlocked = !powerUp.IsUnlocked;
                    powerUp.UpdateModifiedDate();
                    SaveToFile();
                    ConsoleUI.Success($"Unlock status: {(powerUp.IsUnlocked ? "Unlocked" : "Locked")}");
                    break;

                case "8":
                    if (powerUp.IsActive)
                        powerUp.Deactivate();
                    else
                        powerUp.Activate();
                    SaveToFile();
                    ConsoleUI.Success($"Power-up is now {(powerUp.IsActive ? "Active" : "Inactive")}");
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
            ConsoleUI.Error($"An error occurred while updating the power-up: {ex.Message}");
        }

        PauseForUser();
    }

    private void DeletePowerUp()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Delete Power-Up");

        string? searchTerm = ConsoleUI.Prompt("Enter power-up name to delete");
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleUI.Warning("Search term cannot be empty.");
            PauseForUser();
            return;
        }

        var powerUp = items.FirstOrDefault(p =>
            p.PowerUpName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        if (powerUp == null)
        {
            ConsoleUI.Error($"No power-up found matching '{searchTerm}'.");
            PauseForUser();
            return;
        }

        ConsoleUI.Warning($"Are you sure you want to delete power-up '{powerUp.PowerUpName}' (Type: {powerUp.PowerUpType})?");
        if (ConsoleUI.Confirm("This action cannot be undone"))
        {
            items.Remove(powerUp);
            SaveToFile();
            ConsoleUI.Success($"Power-up '{powerUp.PowerUpName}' deleted successfully!");
        }
        else
        {
            ConsoleUI.Info("Deletion cancelled.");
        }

        PauseForUser();
    }

    private void ViewPowerUpStatistics()
    {
        Console.Clear();
        ConsoleUI.SimpleHeader("Power-Up Statistics");

        if (items.Count == 0)
        {
            ConsoleUI.Warning("No power-ups found in the system.");
            PauseForUser();
            return;
        }

        var activePowerUps = items.Where(p => p.IsActive).ToList();
        var unlockedPowerUps = items.Where(p => p.IsUnlocked).ToList();

        Console.WriteLine();
        ConsoleUI.StatBox("Total Power-Ups", items.Count.ToString());
        ConsoleUI.StatBox("Active Power-Ups", activePowerUps.Count.ToString());
        ConsoleUI.StatBox("Inactive Power-Ups", (items.Count - activePowerUps.Count).ToString());
        ConsoleUI.StatBox("Unlocked Power-Ups", unlockedPowerUps.Count.ToString());
        ConsoleUI.StatBox("Locked Power-Ups", (items.Count - unlockedPowerUps.Count).ToString());
        Console.WriteLine();

        if (activePowerUps.Count > 0)
        {
            var avgCost = activePowerUps.Average(p => p.Cost);
            var minCost = activePowerUps.Min(p => p.Cost);
            var maxCost = activePowerUps.Max(p => p.Cost);
            var totalUsage = activePowerUps.Sum(p => p.UsageCount);

            ConsoleUI.Info("Cost Statistics:");
            ConsoleUI.KeyValue("Lowest Cost", $"{minCost} points");
            ConsoleUI.KeyValue("Highest Cost", $"{maxCost} points");
            ConsoleUI.KeyValue("Average Cost", $"{avgCost:F1} points");
            ConsoleUI.KeyValue("Total Usage", totalUsage.ToString());
        }

        // Power-ups by type
        var byType = items.GroupBy(p => p.PowerUpType)
                          .Select(g => new { Type = g.Key, Count = g.Count() })
                          .OrderByDescending(x => x.Count);

        if (byType.Any())
        {
            Console.WriteLine();
            ConsoleUI.Header("Power-Ups by Type");
            foreach (var group in byType)
            {
                ConsoleUI.KeyValue(group.Type.ToString(), $"{group.Count} power-up(s)");
            }
        }

        // Most used power-ups
        var mostUsed = items.Where(p => p.UsageCount > 0)
                            .OrderByDescending(p => p.UsageCount)
                            .Take(5);

        if (mostUsed.Any())
        {
            Console.WriteLine();
            ConsoleUI.Header("Most Used Power-Ups");
            foreach (var powerUp in mostUsed)
            {
                ConsoleUI.KeyValue(powerUp.PowerUpName, $"{powerUp.UsageCount} uses");
            }
        }

        PauseForUser();
    }
}

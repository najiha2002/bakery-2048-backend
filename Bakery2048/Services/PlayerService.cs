using System.Text.Json;
using Bakery2048.Services;
using Bakery2048.Utilities;

public class PlayerService : BaseService<Player>
{
    public PlayerService(List<Player> playerList) : base(playerList, "players.json")
    {
    }

    public void RegisterPlayer()
    {
        ConsoleUI.SimpleHeader("Player Registration");
        
        string username = ConsoleUI.Prompt("Enter your username", ConsoleColor.Cyan);

        if (string.IsNullOrWhiteSpace(username))
        {
            ConsoleUI.Error("Username cannot be empty.");
            return;
        }

        // Check if player already exists
        if (items.Any(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            ConsoleUI.Warning($"Player '{username}' already registered.");
            return;
        }

        string email = ConsoleUI.Prompt("Enter your email", ConsoleColor.Cyan);

        if (string.IsNullOrWhiteSpace(email))
        {
            ConsoleUI.Error("Email cannot be empty.");
            return;
        }

        Player newPlayer = new Player(username, email);
        items.Add(newPlayer);

        SaveToFile();

        Console.WriteLine();
        ConsoleUI.Success($"Welcome to Bakery 2048, {username}!");
        ConsoleUI.KeyValue("Player ID", newPlayer.PlayerId.ToString(), ConsoleColor.DarkGray);
        ConsoleUI.KeyValue("Registration Date", newPlayer.DateRegistered.ToString("yyyy-MM-dd HH:mm"), ConsoleColor.DarkGray);
        ConsoleUI.KeyValue("Starting Rank", newPlayer.GetRankCategory(), ConsoleColor.Yellow);
        
        Console.WriteLine();
        if (ConsoleUI.Confirm("Do you want to record a game session now?"))
        {
            RecordGameSession(newPlayer);
        }
    }

    public void ViewAllPlayers()
    {
        ConsoleUI.SimpleHeader("All Players");

        if (items.Count == 0)
        {
            ConsoleUI.Warning("No players found.");
            return;
        }

        // Header row
        string[] headers = { "Username", "Level", "High Score", "Games Played", "Status" };
        int[] widths = { 20, 8, 12, 15, 10 };
        ConsoleUI.TableRow(headers, widths, true);
        ConsoleUI.Divider('─', 75);

        foreach (var player in items)
        {
            string status = player.IsActive ? "Active" : "Inactive";
            string[] row = { 
                player.Username, 
                player.Level.ToString(), 
                player.HighestScore.ToString(), 
                player.GamesPlayed.ToString(), 
                status 
            };
            
            // Color code status
            Console.Write($"{row[0],-20} {row[1],-8} {row[2],-12} {row[3],-15} ");
            ConsoleUI.WriteLineColored(row[4], player.IsActive ? ConsoleColor.Green : ConsoleColor.DarkGray);
        }

        ConsoleUI.PauseForUser();
    }

    public void SearchPlayer()
    {
        ConsoleUI.SimpleHeader("Search Player");
        
        string searchName = ConsoleUI.Prompt("Enter player username to search", ConsoleColor.Cyan);

        var foundPlayers = items.Where(p => p.Username.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundPlayers.Count == 0)
        {
            ConsoleUI.Warning($"No players found with username containing '{searchName}'.");
            return;
        }

        Console.WriteLine();
        ConsoleUI.WriteLineColored($"Found {foundPlayers.Count} Player(s)", ConsoleColor.Cyan);
        foreach (var player in foundPlayers)
        {
            Console.WriteLine($"\n{player.GetPlayerStats()}");
            ConsoleUI.KeyValue("Rank", player.GetRankCategory(), ConsoleColor.Yellow);
            ConsoleUI.Divider('─', 50);
        }

        PauseForUser();
    }

    public void UpdatePlayer()
    {
        ConsoleUI.SimpleHeader("Update Player");
        
        string searchName = ConsoleUI.Prompt("Enter player username to update", ConsoleColor.Cyan);

        var player = items.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            ConsoleUI.Error($"Player '{searchName}' not found.");
            return;
        }

        Console.WriteLine();
        ConsoleUI.WriteLineColored($"Updating player: {player.Username}", ConsoleColor.Cyan);
        ConsoleUI.MenuOption("1", "Update Email");
        ConsoleUI.MenuOption("2", "Update Score");
        ConsoleUI.MenuOption("3", "Update Level");
        ConsoleUI.MenuOption("4", "Toggle Active Status");
        ConsoleUI.MenuOption("5", "Add Play Time");
        ConsoleUI.MenuOption("6", "Cancel");
        
        string? choice = ConsoleUI.Prompt("Select option", ConsoleColor.Yellow);

        switch (choice)
        {
            case "1":
                string newEmail = ConsoleUI.Prompt("Enter new email", ConsoleColor.Cyan);
                player.Email = newEmail;
                ConsoleUI.Success("Email updated.");
                break;
            case "2":
                string scoreInput = ConsoleUI.Prompt("Enter new score", ConsoleColor.Cyan);
                if (int.TryParse(scoreInput, out int score))
                {
                    player.UpdateScore(score);
                    player.IncrementGamesPlayed();
                    ConsoleUI.Success($"Score updated. New high score: {player.HighestScore}");
                }
                else
                {
                    ConsoleUI.Error("Invalid score.");
                }
                break;
            case "3":
                string levelInput = ConsoleUI.Prompt("Enter new level", ConsoleColor.Cyan);
                if (int.TryParse(levelInput, out int level))
                {
                    player.Level = level;
                    ConsoleUI.Success("Level updated.");
                }
                else
                {
                    ConsoleUI.Error("Invalid level.");
                }
                break;
            case "4":
                if (player.IsActive)
                {
                    player.Deactivate();
                    ConsoleUI.Success("Player deactivated.");
                }
                else
                {
                    player.Activate();
                    ConsoleUI.Success("Player activated.");
                }
                break;
            case "5":
                string hoursInput = ConsoleUI.Prompt("Enter hours played", ConsoleColor.Cyan);
                if (double.TryParse(hoursInput, out double hours))
                {
                    player.AddPlayTime(TimeSpan.FromHours(hours));
                    ConsoleUI.Success($"Play time added. Total: {player.TotalPlayTime.TotalHours:F2} hours");
                }
                else
                {
                    ConsoleUI.Error("Invalid hours.");
                }
                break;
            case "6":
                ConsoleUI.Info("Update cancelled.");
                return;
            default:
                ConsoleUI.Error("Invalid option.");
                return;
        }

        SaveToFile();
        
        PauseForUser();
    }

    public void DeletePlayer()
    {
        ConsoleUI.SimpleHeader("Delete Player");
        
        string searchName = ConsoleUI.Prompt("Enter player username to delete", ConsoleColor.Cyan);

        var player = items.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            ConsoleUI.Error($"Player '{searchName}' not found.");
            return;
        }

        if (ConsoleUI.Confirm($"Are you sure you want to delete player '{player.Username}'?"))
        {
            items.Remove(player);
            SaveToFile();
            ConsoleUI.Success($"Player '{player.Username}' deleted successfully.");
        }
        else
        {
            ConsoleUI.Info("Deletion cancelled.");
        }

        PauseForUser();
    }

    public void ViewPlayerStatistics()
    {
        if (items.Count == 0)
        {
            ConsoleUI.Warning("No players available for statistics.");
            return;
        }

        ConsoleUI.SimpleHeader("Player Statistics");

        // Basic stats
        ConsoleUI.KeyValue("Total Players", items.Count.ToString(), ConsoleColor.Cyan);
        ConsoleUI.KeyValue("Active Players", items.Count(p => p.IsActive).ToString(), ConsoleColor.Green);
        ConsoleUI.KeyValue("Inactive Players", items.Count(p => !p.IsActive).ToString(), ConsoleColor.DarkGray);
        ConsoleUI.KeyValue("Average Score", items.Average(p => p.HighestScore).ToString("F2"), ConsoleColor.Yellow);
        ConsoleUI.KeyValue("Highest Score", items.Max(p => p.HighestScore).ToString(), ConsoleColor.Magenta);
        ConsoleUI.KeyValue("Total Games Played", items.Sum(p => p.GamesPlayed).ToString(), ConsoleColor.Blue);

        var topPlayer = items.OrderByDescending(p => p.HighestScore).FirstOrDefault();
        if (topPlayer != null)
        {
            Console.WriteLine();
            ConsoleUI.WriteLineColored("🏆 Top Player", ConsoleColor.Yellow);
            ConsoleUI.KeyValue("   Username", topPlayer.Username, ConsoleColor.White);
            ConsoleUI.KeyValue("   Score", topPlayer.HighestScore.ToString(), ConsoleColor.Magenta);
            ConsoleUI.KeyValue("   Rank", topPlayer.GetRankCategory(), ConsoleColor.Yellow);
        }

        Console.WriteLine();
        ConsoleUI.WriteLineColored("═══ Top 5 Leaderboard ═══", ConsoleColor.Cyan);
        var top5 = items.OrderByDescending(p => p.HighestScore).Take(5);
        int rank = 1;
        foreach (var player in top5)
        {
            ConsoleUI.WriteColored($"{rank}. ", ConsoleColor.Yellow);
            Console.WriteLine($"{player.GetLeaderboardEntry()} - {player.GetRankCategory()}");
            rank++;
        }

        ConsoleUI.PauseForUser();
    }

    public override void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleUI.ShowTitle("👤 Player Management");
            ConsoleUI.MenuOption("1", "Register New Player");
            ConsoleUI.MenuOption("2", "View All Players");
            ConsoleUI.MenuOption("3", "Search Player by Username");
            ConsoleUI.MenuOption("4", "Update Player Info");
            ConsoleUI.MenuOption("5", "Delete Player");
            ConsoleUI.MenuOption("6", "View Player Statistics");
            ConsoleUI.MenuOption("7", "Record New Game Session");
            ConsoleUI.MenuOption("0", "Back to Main Menu");
            Console.WriteLine();

            string? choice = ConsoleUI.Prompt("Enter your choice");

            switch (choice)
            {
                case "1":
                    RegisterPlayer();
                    break;
                case "2":
                    ViewAllPlayers();
                    break;
                case "3":
                    SearchPlayer();
                    break;
                case "4":
                    UpdatePlayer();
                    break;
                case "5":
                    DeletePlayer();
                    break;
                case "6":
                    ViewPlayerStatistics();
                    break;
                case "7":
                    RecordGameSession();
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

    public void RecordGameSession(Player? player = null)
    {
        // get player by username if not provided
        if (player == null)
        {
            string searchName = ConsoleUI.Prompt("Enter player username", ConsoleColor.Cyan);

            player = items.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

            if (player == null)
            {
                ConsoleUI.Error($"Player '{searchName}' not found.");
                return;
            }
        }

        ConsoleUI.SimpleHeader($"Recording Game Session for {player.Username}");

        // get final score
        string scoreInput = ConsoleUI.Prompt("Enter final score", ConsoleColor.Cyan);
        if (!int.TryParse(scoreInput, out int finalScore) || finalScore < 0)
        {
            ConsoleUI.Error("Invalid score.");
            return;
        }

        // get best tile achieved
        string tileInput = ConsoleUI.Prompt("Enter best tile achieved (e.g., 2048, 4096)", ConsoleColor.Cyan);
        if (!int.TryParse(tileInput, out int bestTile) || bestTile < 0)
        {
            ConsoleUI.Error("Invalid tile value.");
            return;
        }

        // get moves made
        string movesInput = ConsoleUI.Prompt("Enter number of moves made", ConsoleColor.Cyan);
        if (!int.TryParse(movesInput, out int moves) || moves < 0)
        {
            ConsoleUI.Error("Invalid number of moves.");
            return;
        }

        // Get play duration
        string durationInput = ConsoleUI.Prompt("Enter play duration in minutes", ConsoleColor.Cyan);
        if (!double.TryParse(durationInput, out double minutes) || minutes < 0)
        {
            ConsoleUI.Error("Invalid duration.");
            return;
        }

        // Get power-ups used
        List<string> powerUpsUsedInSession = new List<string>();
        string powerUpCountInput = ConsoleUI.Prompt("Enter number of power-ups used (0 if none)", ConsoleColor.Cyan);
        if (int.TryParse(powerUpCountInput, out int powerUpCount) && powerUpCount > 0)
        {
            Console.WriteLine();
            ConsoleUI.Info("Enter the name of each power-up used:");
            for (int i = 1; i <= powerUpCount; i++)
            {
                string powerUpName = ConsoleUI.Prompt($"Power-up #{i}", ConsoleColor.Yellow);
                if (!string.IsNullOrWhiteSpace(powerUpName))
                {
                    powerUpsUsedInSession.Add(powerUpName.Trim());
                }
            }
        }

        // check if reached win condition (e.g., 2048 tile)
        bool reachedWin = bestTile >= 2048 ? true : false;

        // record the session
        TimeSpan playDuration = TimeSpan.FromMinutes(minutes);
        player.RecordGameSession(finalScore, bestTile, moves, playDuration, powerUpsUsedInSession.Count > 0 ? powerUpsUsedInSession : null, reachedWin);

        SaveToFile();

        Console.WriteLine();
        ConsoleUI.Success("Game session recorded successfully!");
        ConsoleUI.KeyValue("High Score", player.HighestScore.ToString(), ConsoleColor.Magenta);
        ConsoleUI.KeyValue("Best Tile Ever", player.BestTileAchieved.ToString(), ConsoleColor.Yellow);
        ConsoleUI.KeyValue("Games Played", player.GamesPlayed.ToString(), ConsoleColor.Cyan);
        ConsoleUI.KeyValue("Average Score", player.AverageScore.ToString("F2"), ConsoleColor.Green);
        ConsoleUI.KeyValue("Current Level", player.Level.ToString(), ConsoleColor.Blue);
        ConsoleUI.KeyValue("Rank", player.GetRankCategory(), ConsoleColor.Yellow);
        ConsoleUI.KeyValue("Win Streak", player.WinStreak.ToString(), ConsoleColor.Magenta);
        ConsoleUI.KeyValue("Total Power-Ups Used", player.PowerUpsUsed.ToString(), ConsoleColor.DarkGray);
        
        PauseForUser();
    }
}
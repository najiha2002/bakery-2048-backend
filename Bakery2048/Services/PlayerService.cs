using System.Text.Json;

public class PlayerService
{
    private List<Player> players;
    private readonly string dataFilePath = "players.json";

    public PlayerService(List<Player> playerList)
    {
        players = playerList;
        LoadFromFile();
    }

    public void RegisterPlayer()
    {
        Console.WriteLine("\n=== Player Registration ===");
        
        Console.Write("Enter your name: ");
        string name = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        // Check if player already exists
        if (players.Any(p => p.Username.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"Player '{name}' already registered.");
            return;
        }

        Console.Write("Enter your email: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Email cannot be empty.");
            return;
        }

        Player newPlayer = new Player(name, email);
        players.Add(newPlayer);

        SaveToFile();

        Console.WriteLine($"\n✓ Welcome to Bakery 2048, {name}!");
        Console.WriteLine($"Player ID: {newPlayer.PlayerId}");
        Console.WriteLine($"Registration Date: {newPlayer.DateRegistered:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Starting Rank: {newPlayer.GetRankCategory()}");
        Console.WriteLine("\nYou can now start playing and recording game sessions! Do you want to record a game session now? (yes/no): ");

        string? recordNow = Console.ReadLine()?.ToLower();
        if (recordNow == "yes" || recordNow == "y")
        {            RecordGameSession();
        }
    }

    public void ViewAllPlayers()
    {
        Console.WriteLine("\n=== All Players ===");

        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine($"{"Username",-20} {"Level",-8} {"High Score",-12} {"Games Played",-15} {"Status",-10}");
        Console.WriteLine(new string('-', 75));

        foreach (var player in players)
        {
            string status = player.IsActive ? "Active" : "Inactive";
            Console.WriteLine($"{player.Username,-20} {player.Level,-8} {player.HighestScore,-12} {player.GamesPlayed,-15} {status,-10}");
        }
    }

    public void SearchPlayer()
    {
        Console.Write("\nEnter player name to search: ");
        string searchName = Console.ReadLine() ?? "";

        var foundPlayers = players.Where(p => p.Username.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundPlayers.Count == 0)
        {
            Console.WriteLine($"No players found with name containing '{searchName}'.");
            return;
        }

        Console.WriteLine($"\n=== Found {foundPlayers.Count} Player(s) ===");
        foreach (var player in foundPlayers)
        {
            Console.WriteLine($"\n{player.GetPlayerStats()}");
            Console.WriteLine($"Rank: {player.GetRankCategory()}");
            Console.WriteLine(new string('-', 50));
        }
    }

    public void UpdatePlayer()
    {
        Console.Write("\nEnter player name to update: ");
        string searchName = Console.ReadLine() ?? "";

        var player = players.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            Console.WriteLine($"Player '{searchName}' not found.");
            return;
        }

        Console.WriteLine($"\nUpdating player: {player.Username}");
        Console.WriteLine("1. Update Email");
        Console.WriteLine("2. Update Score");
        Console.WriteLine("3. Update Level");
        Console.WriteLine("4. Toggle Active Status");
        Console.WriteLine("5. Add Play Time");
        Console.WriteLine("6. Cancel");
        Console.Write("Select option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter new email: ");
                player.Email = Console.ReadLine() ?? "";
                Console.WriteLine("✓ Email updated.");
                break;
            case "2":
                Console.Write("Enter new score: ");
                if (int.TryParse(Console.ReadLine(), out int score))
                {
                    player.UpdateScore(score);
                    player.IncrementGamesPlayed();
                    Console.WriteLine($"✓ Score updated. New high score: {player.HighestScore}");
                }
                else
                {
                    Console.WriteLine("Invalid score.");
                }
                break;
            case "3":
                Console.Write("Enter new level: ");
                if (int.TryParse(Console.ReadLine(), out int level))
                {
                    player.Level = level;
                    Console.WriteLine("✓ Level updated.");
                }
                else
                {
                    Console.WriteLine("Invalid level.");
                }
                break;
            case "4":
                if (player.IsActive)
                {
                    player.Deactivate();
                    Console.WriteLine("✓ Player deactivated.");
                }
                else
                {
                    player.Activate();
                    Console.WriteLine("✓ Player activated.");
                }
                break;
            case "5":
                Console.Write("Enter hours played: ");
                if (double.TryParse(Console.ReadLine(), out double hours))
                {
                    player.AddPlayTime(TimeSpan.FromHours(hours));
                    Console.WriteLine($"✓ Play time added. Total: {player.TotalPlayTime.TotalHours:F2} hours");
                }
                else
                {
                    Console.WriteLine("Invalid hours.");
                }
                break;
            case "6":
                Console.WriteLine("Update cancelled.");
                return;
            default:
                Console.WriteLine("Invalid option.");
                return;
        }

        SaveToFile();
    }

    public void DeletePlayer()
    {
        Console.Write("\nEnter player name to delete: ");
        string searchName = Console.ReadLine() ?? "";

        var player = players.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            Console.WriteLine($"Player '{searchName}' not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete player '{player.Username}'? (yes/no): ");
        string confirm = Console.ReadLine()?.ToLower() ?? "";

        if (confirm == "yes" || confirm == "y")
        {
            players.Remove(player);
            SaveToFile();
            Console.WriteLine($"✓ Player '{player.Username}' deleted successfully.");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    public void ViewPlayerStatistics()
    {
        if (players.Count == 0)
        {
            Console.WriteLine("\nNo players available for statistics.");
            return;
        }

        Console.WriteLine("\n=== Player Statistics ===");
        Console.WriteLine($"Total Players: {players.Count}");
        Console.WriteLine($"Active Players: {players.Count(p => p.IsActive)}");
        Console.WriteLine($"Inactive Players: {players.Count(p => !p.IsActive)}");
        Console.WriteLine($"Average Score: {players.Average(p => p.HighestScore):F2}");
        Console.WriteLine($"Highest Score: {players.Max(p => p.HighestScore)}");
        Console.WriteLine($"Total Games Played: {players.Sum(p => p.GamesPlayed)}");

        var topPlayer = players.OrderByDescending(p => p.HighestScore).FirstOrDefault();
        if (topPlayer != null)
        {
            Console.WriteLine($"\n🏆 Top Player: {topPlayer.Username}");
            Console.WriteLine($"   Score: {topPlayer.HighestScore}");
            Console.WriteLine($"   Rank: {topPlayer.GetRankCategory()}");
        }

        Console.WriteLine("\n=== Top 5 Leaderboard ===");
        var top5 = players.OrderByDescending(p => p.HighestScore).Take(5);
        int rank = 1;
        foreach (var player in top5)
        {
            Console.WriteLine($"{rank}. {player.GetLeaderboardEntry()} - {player.GetRankCategory()}");
            rank++;
        }
    }

    public void ShowMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n=== Player Management ===");
            Console.WriteLine("1. Register New Player");
            Console.WriteLine("2. View All Players");
            Console.WriteLine("3. Search Player by Name");
            Console.WriteLine("4. Update Player Info");
            Console.WriteLine("5. Delete Player");
            Console.WriteLine("6. View Player Statistics");
            Console.WriteLine("7. Record New Game Session");
            Console.WriteLine("8. Back to Main Menu");
            Console.Write("Select an option (1-8): ");

            string? input = Console.ReadLine();

            switch (input)
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
                case "8":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void RecordGameSession()
    {
        // get player by name
        Console.Write("\nEnter player name: ");
        string searchName = Console.ReadLine() ?? "";

        var player = players.FirstOrDefault(p => p.Username.Equals(searchName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            Console.WriteLine($"Player '{searchName}' not found.");
            return;
        }

        Console.WriteLine($"\n=== Recording Game Session for {player.Username} ===");

        // get final score
        Console.Write("Enter final score: ");
        if (!int.TryParse(Console.ReadLine(), out int finalScore) || finalScore < 0)
        {
            Console.WriteLine("Invalid score.");
            return;
        }

        // get best tile achieved
        Console.Write("Enter best tile achieved (e.g., 2048, 4096): ");
        if (!int.TryParse(Console.ReadLine(), out int bestTile) || bestTile < 0)
        {
            Console.WriteLine("Invalid tile value.");
            return;
        }

        // get moves made
        Console.Write("Enter number of moves made: ");
        if (!int.TryParse(Console.ReadLine(), out int moves) || moves < 0)
        {
            Console.WriteLine("Invalid number of moves.");
            return;
        }

        // Get play duration
        Console.Write("Enter play duration in minutes: ");
        if (!double.TryParse(Console.ReadLine(), out double minutes) || minutes < 0)
        {
            Console.WriteLine("Invalid duration.");
            return;
        }

        // check if reached win condition (e.g., 2048 tile)
        bool reachedWin = bestTile >= 2048 ? true : false;

        // record the session
        TimeSpan playDuration = TimeSpan.FromMinutes(minutes);
        player.RecordGameSession(finalScore, bestTile, moves, playDuration, reachedWin);

        SaveToFile();

        Console.WriteLine("\n✓ Game session recorded successfully!");
        Console.WriteLine($"High Score: {player.HighestScore}");
        Console.WriteLine($"Best Tile Ever: {player.BestTileAchieved}");
        Console.WriteLine($"Games Played: {player.GamesPlayed}");
        Console.WriteLine($"Average Score: {player.AverageScore:F2}");
        Console.WriteLine($"Current Level: {player.Level}");
        Console.WriteLine($"Rank: {player.GetRankCategory()}");
        Console.WriteLine($"Win Streak: {player.WinStreak}");
    }

    private void SaveToFile()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(players, options);
            File.WriteAllText(dataFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    private void LoadFromFile()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                string jsonString = File.ReadAllText(dataFilePath);
                var loadedPlayers = JsonSerializer.Deserialize<List<Player>>(jsonString);

                if (loadedPlayers != null)
                {
                    players.Clear();
                    players.AddRange(loadedPlayers);
                    Console.WriteLine($"✓ Loaded {players.Count} player(s) from file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }
}


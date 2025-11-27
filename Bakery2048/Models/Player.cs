public class Player
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public int HighestScore { get; set; }
    public int CurrentScore { get; set; }
    public int BestTileAchieved { get; set; }
    public int Level { get; set; }
    public int GamesPlayed { get; set; }
    public double AverageScore { get; set; }
    public DateTime DateRegistered { get; set; }
    public DateTime LastPlayed { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan TotalPlayTime { get; set; }
    public int WinStreak { get; set; }
    public int TotalMoves { get; set; }
    public int PowerUpsUsed { get; set; }
    public string FavoriteItem { get; set; }

    public Player(string username, string email = "")
    {
        PlayerId = Guid.NewGuid();
        Username = username;
        Email = email;
        HighestScore = 0;
        CurrentScore = 0;
        BestTileAchieved = 0;
        Level = 1;
        GamesPlayed = 0;
        AverageScore = 0.0;
        DateRegistered = DateTime.Now;
        LastPlayed = DateTime.Now;
        IsActive = true;
        TotalPlayTime = TimeSpan.Zero;
        WinStreak = 0;
        TotalMoves = 0;
        PowerUpsUsed = 0;
        FavoriteItem = string.Empty;
    }

    public void UpdateScore(int newScore)
    {
        if (newScore > HighestScore)
        {
            HighestScore = newScore;
        }
        CurrentScore = newScore;
        CalculateAverageScore();
    }

    public void IncrementGamesPlayed()
    {
        GamesPlayed++;
        LastPlayed = DateTime.Now;
        CalculateAverageScore();
    }

    public void CalculateAverageScore()
    {
        if (GamesPlayed > 0)
        {
            AverageScore = (AverageScore * (GamesPlayed - 1) + CurrentScore) / GamesPlayed;
        }
    }

    public void LevelUp()
    {
        Level++;
    }

    public bool IsHighScore(int score)
    {
        return score > HighestScore;
    }

    public void AddPlayTime(TimeSpan duration)
    {
        TotalPlayTime += duration;
    }

    public string GetPlayerStats()
    {
        return $"Player: {Username}\n" +
               $"ID: {PlayerId}\n" +
               $"Email: {Email}\n" +
               $"Level: {Level}\n" +
               $"Highest Score: {HighestScore}\n" +
               $"Average Score: {AverageScore:F2}\n" +
               $"Games Played: {GamesPlayed}\n" +
               $"Best Tile: {BestTileAchieved}\n" +
               $"Win Streak: {WinStreak}\n" +
               $"Total Moves: {TotalMoves}\n" +
               $"Power-Ups Used: {PowerUpsUsed}\n" +
               $"Total Play Time: {TotalPlayTime.TotalHours:F2} hours\n" +
               $"Date Registered: {DateRegistered:yyyy-MM-dd}\n" +
               $"Last Played: {LastPlayed:yyyy-MM-dd}\n" +
               $"Status: {(IsActive ? "Active" : "Inactive")}";
    }

    // update best tile if a higher tile is achieved
    public void UpdateBestTile(int tileValue)
    {
        if (tileValue > BestTileAchieved)
        {
            BestTileAchieved = tileValue;
        }
    }

    public void IncrementWinStreak()
    {
        WinStreak++;
    }

    public void ResetWinStreak()
    {
        WinStreak = 0;
    }

    public void AddMoves(int moves)
    {
        TotalMoves += moves;
    }

    public void UsePowerUp()
    {
        PowerUpsUsed++;
    }

    // Deactivate player account
    public void Deactivate()
    {
        IsActive = false;
    }

    // Reactivate player account
    public void Activate()
    {
        IsActive = true;
    }

    public int GetDaysSinceRegistration()
    {
        return (DateTime.Now - DateRegistered).Days;
    }

    // Get days since last played
    public int GetDaysSinceLastPlayed()
    {
        return (DateTime.Now - LastPlayed).Days;
    }

    // Check if player is inactive (hasn't played in 30 days)
    public bool IsInactive()
    {
        return GetDaysSinceLastPlayed() > 30;
    }

    public void CalculateLevelFromScore()
    {
        Level = (HighestScore / 1000) + 1;
    }

    public string GetRankCategory()
    {
        if (HighestScore >= 50000) return "Master Baker";
        if (HighestScore >= 20000) return "Head Baker";
        if (HighestScore >= 10000) return "Pastry Chef";
        if (HighestScore >= 5000) return "Baker";
        if (HighestScore >= 1000) return "Apprentice Baker";
        return "Kitchen Helper";
    }

    // get efficiency (average score per move)
    public double GetEfficiency()
    {
        return TotalMoves > 0 ? (double)HighestScore / TotalMoves : 0;
    }

    // update favorite item based on usage
    public void SetFavoriteItem(string itemName)
    {
        FavoriteItem = itemName;
    }

    // Get summary for leaderboard display
    public string GetLeaderboardEntry()
    {
        return $"{Username} - Level {Level} - Score: {HighestScore}";
    }

    // reset current game stats (for new game)
    public void ResetCurrentGame()
    {
        CurrentScore = 0;
    }

    // Record a complete game session
    public void RecordGameSession(int finalScore, int bestTileAchieved, int movesMade, TimeSpan playDuration, bool reachedWinCondition = false)
    {
        // Update scores
        CurrentScore = finalScore;
        if (finalScore > HighestScore)
        {
            HighestScore = finalScore;
        }

        // Update best tile
        if (bestTileAchieved > BestTileAchieved)
        {
            BestTileAchieved = bestTileAchieved;
        }

        // Increment games played
        GamesPlayed++;

        // Update last played timestamp
        LastPlayed = DateTime.Now;

        // Add moves to total
        TotalMoves += movesMade;

        // Add play time
        TotalPlayTime += playDuration;

        // Recalculate average score
        CalculateAverageScore();

        // Update win streak
        if (reachedWinCondition)
        {
            IncrementWinStreak();
        }
        else
        {
            ResetWinStreak();
        }

        // Auto level up based on score
        CalculateLevelFromScore();
    }
}


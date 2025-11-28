using Bakery2048.Utilities;
using Bakery2048.Services;

class Program
{
    static List<Player> players = new List<Player>();
    static List<Tile> tiles = new List<Tile>();
    static List<PowerUp> powerUps = new List<PowerUp>();

    static PlayerService playerService = null!;
    static TileService tileService = null!;

    static void Main(string[] args)
    {
        playerService = new PlayerService(players);
        tileService = new TileService(tiles);
        bool exit = false;

        ConsoleUI.ShowTitle("🍰 Bakery 2048 - Data Management System 🍰");

        while (!exit)
        {
            ConsoleUI.SimpleHeader("Main Menu");
            ConsoleUI.MenuOption("1", "Manage Players");
            ConsoleUI.MenuOption("2", "Manage Tiles");
            ConsoleUI.MenuOption("3", "Manage Power-Ups");
            ConsoleUI.MenuOption("4", "Generate Random Data");
            ConsoleUI.MenuOption("5", "Run Data Analysis (LINQ)");
            ConsoleUI.MenuOption("6", "Exit");
            
            string? input = ConsoleUI.Prompt("\nSelect an option (1-6)", ConsoleColor.Yellow);

            switch (input)
            {
                case "1":
                    ManagePlayers();
                    break;
                case "2":
                    ManageTiles();
                    break;
                case "3":
                    ManagePowerUps();
                    break;
                case "4":
                    GenerateRandomData();
                    break;
                case "5":
                    RunAnalysis();
                    break;
                case "6":
                    exit = true;
                    ConsoleUI.Success("Exiting application. Goodbye!");
                    break;
                default:
                    ConsoleUI.Error("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ManagePlayers()
    {
        playerService.ShowMenu();
    }

    static void ManageTiles()
    {
        tileService.ShowMenu();
    }

    static void ManagePowerUps()
    {
        ConsoleUI.Info("Power-Up management menu (CRUD functions to be added)");
        ConsoleUI.PauseForUser();
    }

    static void GenerateRandomData()
    {
        ConsoleUI.Info("Random data generation logic will go here");
        ConsoleUI.PauseForUser();
    }

    static void RunAnalysis()
    {
        ConsoleUI.Info("LINQ data analysis logic will go here");
        ConsoleUI.PauseForUser();
    }
}

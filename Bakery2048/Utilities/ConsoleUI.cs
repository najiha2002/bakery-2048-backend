namespace Bakery2048.Utilities
{
    public static class ConsoleUI
    {
        // Color scheme
        public static class Colors
        {
            public static ConsoleColor Header = ConsoleColor.Cyan;
            public static ConsoleColor Success = ConsoleColor.Green;
            public static ConsoleColor Error = ConsoleColor.Red;
            public static ConsoleColor Warning = ConsoleColor.Yellow;
            public static ConsoleColor Info = ConsoleColor.Blue;
            public static ConsoleColor Highlight = ConsoleColor.Magenta;
            public static ConsoleColor Menu = ConsoleColor.White;
        }

        // Write colored text
        public static void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Success message
        public static void Success(string message)
        {
            WriteColored("✓ ", Colors.Success);
            Console.WriteLine(message);
        }

        // Error message
        public static void Error(string message)
        {
            WriteColored("✗ ", Colors.Error);
            Console.WriteLine(message);
        }

        // Warning message
        public static void Warning(string message)
        {
            WriteColored("⚠ ", Colors.Warning);
            Console.WriteLine(message);
        }

        // Info message
        public static void Info(string message)
        {
            WriteColored("ℹ ", Colors.Info);
            Console.WriteLine(message);
        }

        // Section header
        public static void Header(string title)
        {
            Console.WriteLine();
            WriteLineColored($"╔══════════════════════════════════════════╗", Colors.Header);
            WriteLineColored($"║  {title.PadRight(38)}║", Colors.Header);
            WriteLineColored($"╚══════════════════════════════════════════╝", Colors.Header);
            Console.WriteLine();
        }

        // Simple header
        public static void SimpleHeader(string title)
        {
            Console.WriteLine();
            WriteLineColored($"═══ {title} ═══", Colors.Header);
            Console.WriteLine();
        }

        // Divider
        public static void Divider(char character = '─', int length = 75)
        {
            Console.WriteLine(new string(character, length));
        }

        // Menu option
        public static void MenuOption(string number, string description)
        {
            WriteColored($"  [{number}] ", Colors.Highlight);
            Console.WriteLine(description);
        }

        // Prompt for input
        public static string Prompt(string message, ConsoleColor color = ConsoleColor.White)
        {
            WriteColored($"{message}: ", color);
            return Console.ReadLine() ?? "";
        }

        // Confirm action
        public static bool Confirm(string message)
        {
            WriteColored($"{message} (yes/no): ", Colors.Warning);
            string response = Console.ReadLine()?.ToLower() ?? "";
            return response == "yes" || response == "y";
        }

        // Display table row with alternating colors
        public static void TableRow(string[] columns, int[] widths, bool isHeader = false)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                string cell = columns[i].PadRight(widths[i]);
                if (isHeader)
                {
                    WriteColored(cell, Colors.Header);
                }
                else
                {
                    Console.Write(cell);
                }
            }
            Console.WriteLine();
        }

        // Progress indicator
        public static void ShowProgress(string message)
        {
            WriteColored("⟳ ", Colors.Info);
            Console.WriteLine(message);
        }

        // Clear and show title
        public static void ShowTitle(string title)
        {
            Console.Clear();
            WriteLineColored("╔════════════════════════════════════════════════╗", ConsoleColor.Cyan);
            WriteLineColored($"║  {title.PadRight(44)}║", ConsoleColor.Cyan);
            WriteLineColored("╚════════════════════════════════════════════════╝", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        // Pause for user
        public static void PauseForUser()
        {
            Console.WriteLine();
            WriteColored("Press Enter to continue...", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        // Display key-value pair
        public static void KeyValue(string key, string value, ConsoleColor keyColor = ConsoleColor.Gray)
        {
            WriteColored($"{key}: ", keyColor);
            Console.WriteLine(value);
        }

        // Display statistics box
        public static void StatBox(string label, string value, ConsoleColor valueColor = ConsoleColor.White)
        {
            WriteColored($"┌─ {label} ", Colors.Info);
            Console.WriteLine();
            WriteColored($"│  ", Colors.Info);
            WriteLineColored(value, valueColor);
            WriteColored($"└", Colors.Info);
            Console.WriteLine(new string('─', 20));
        }
    }
}

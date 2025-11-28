# Bakery 2048 - Data Management System

A comprehensive backend data management system for the Bakery 2048 game, built with C# and .NET. This system manages players, bakery item tiles, and power-ups with full CRUD operations, automated data generation, and advanced LINQ analytics.

## Overview

**Bakery 2048** is a themed variation of the popular 2048 puzzle game where players merge bakery items (Flour → Cookie → Cake → Wedding Cake) to achieve higher scores. This backend system provides:

- **Player Management**: Register, track, and analyze player accounts with comprehensive statistics
- **Tile Management**: Manage bakery item catalog (tiles with values, icons, colors)
- **Power-Up Management**: Control special abilities players can use during gameplay
- **Data Generation**: Automatically generate realistic test data (50-75 players with game sessions)
- **LINQ Analytics**: Advanced data analysis with 4 comprehensive analytics categories

### What Makes This Project Interesting?

- **Game-focused**: Designed specifically for 2048-style puzzle game mechanics
- **Data-driven**: Extensive analytics to understand player behavior and game balance
- **Automated Testing**: Generate realistic test data in seconds
- **Themed Experience**: All game elements themed around bakery items and baking
- **Simple Storage**: JSON-based persistence that's easy to read and version control

## Features

### 1. Player Management
- Register new players with email validation
- Track game sessions with detailed statistics
- Automatic rank system (Kitchen Helper → Master Baker)
- Level progression based on scores
- Win streak tracking
- Play time and efficiency metrics
- Search and filter capabilities

### 2. Tile Management
- Manage bakery item catalog (Flour, Cookie, Cake, etc.)
- Assign tile values (2, 4, 8, 16, ... 2048, 4096)
- Custom icons and color schemes
- Special/event item marking
- Active/inactive status management
- Statistics and analytics

### 3. Power-Up Management
- Four power-up types:
  - **ScoreBoost**: Multiply points earned
  - **TimeExtension**: Extend gameplay duration
  - **Undo**: Reverse previous moves
  - **SwapTiles**: Rearrange tile positions
- Cost and cooldown management
- Lock/unlock system
- Usage tracking across all game sessions
- Effect multipliers

### 4. Data Generation
- Generate 50-75 random players instantly
- Simulate 1-3 game sessions per player
- Realistic score distributions (100-50,000 points)
- Varied play patterns (casual to hardcore)
- Automatic power-up usage simulation
- Execution time: ~0.03 seconds

### 5. LINQ Analytics
- **Player Analytics**: Score distribution, activity levels, engagement metrics
- **Power-Up Analytics**: Usage statistics, popularity rankings, cost analysis
- **Cross-Entity Analysis**: Power-up impact, win rates, elite player behavior
- **Advanced Queries**: Percentiles, standard deviation, correlation insights

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher
- A code editor (VS Code, Visual Studio, or Rider)
- Terminal/Command Prompt

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/najiha2002/bakery-2048-backend.git
   cd bakery-2048-backend
   ```

2. **Navigate to project folder**
   ```bash
   cd Bakery2048
   ```

3. **Restore dependencies** (optional, done automatically on build)
   ```bash
   dotnet restore
   ```

## How to Run

### Method 1: Using dotnet run

```bash
cd Bakery2048
dotnet run
```

### Method 2: Build and Execute

```bash
cd Bakery2048
dotnet build
dotnet run
```

### Expected Output

```
Bakery 2048 - Data Management System
[1] Manage Players
[2] Manage Tiles
[3] Manage Power-Ups
[4] Generate Random Data
[5] Run Data Analysis (LINQ)
[6] Exit

Select an option (1-6):
```

## Project Structure

```
bakery-2048-backend/
├── Bakery2048/
│   ├── Models/                    # Data models
│   │   ├── Player.cs             # Player entity
│   │   ├── Tile.cs               # Tile entity
│   │   ├── PowerUp.cs            # PowerUp entity
│   │   ├── PowerUpType.cs        # Enum for power-up types
│   │   └── BaseEntity.cs         # Base class for all entities
│   ├── Services/                  # Business logic
│   │   ├── BaseService.cs        # Generic CRUD base service
│   │   ├── PlayerService.cs      # Player management
│   │   ├── TileService.cs        # Tile management
│   │   ├── PowerUpService.cs     # Power-up management
│   │   ├── DataGenerationService.cs   # Test data generation
│   │   └── DataAnalysisService.cs     # LINQ analytics
│   ├── Utilities/                 # Helper utilities
│   │   └── ConsoleUI.cs          # Console formatting helpers
│   ├── Program.cs                 # Main entry point
│   ├── players.json              # Player data storage
│   ├── tiles.json                # Tile data storage
│   └── powerups.json             # Power-up data storage
├── docs/                          # Documentation
│   ├── PLAYER_CRUD_OPERATIONS.md
│   ├── TILE_CRUD_OPERATIONS.md
│   └── POWERUP_CRUD_OPERATIONS.md
└── README.md                      # This file
```

## Core Functionalities

### 1. Player Management

**Register Player**
```
[1] Manage Players → [1] Register New Player
Enter username: BakerMaster
Enter email: baker@bakery.com
✓ Welcome to Bakery 2048, BakerMaster!
```

**Record Game Session**
```
Enter final score: 12450
Enter best tile: 2048
Enter moves: 234
Enter duration: 15.5 minutes
✓ Game session recorded!
```

**View Statistics**
```
Total Players: 150
Active Players: 132
Average Score: 8,543.25
Highest Score: 95,430 (BobTheBaker)
```

### 2. Tile Management

**Add Bakery Item**
```
[2] Manage Tiles → [1] Add New Tile
Item name: Rainbow Cake
Tile value: 4096
Icon: 🌈
Color: #FF00FF
Special item: Yes
✓ Tile added successfully!
```

### 3. Power-Up Management

**Add Power-Up**
```
[3] Manage Power-Ups → [1] Add New Power-Up
Name: Double Score
Type: [1] ScoreBoost
Cost: 200 points
Cooldown: 5 moves
Effect multiplier: 2.0
Icon: ⚡
✓ Power-up created!
```

### 4. Generate Test Data

```
[4] Generate Random Data
Continue? (y/n): y

✓ Data generation complete!
Players Generated: 56
Game Sessions: 65
Time: 0.03 seconds
```

### 5. LINQ Analytics

```
[5] Run Data Analysis (LINQ)
[1] Player Analytics
[2] Power-Up Analytics
[3] Cross-Entity Analysis
[4] Advanced Queries

Select category: 1

Player Analytics
Score Distribution:
  0-1K:    12 players
  1K-5K:   23 players
  5K-10K:  15 players
  10K-20K: 8 players
  20K+:    4 players
```

## Documentation

Detailed documentation for each entity:

- **[Player CRUD Operations](docs/PLAYER_CRUD_OPERATIONS.md)** - Complete player management guide
- **[Tile CRUD Operations](docs/TILE_CRUD_OPERATIONS.md)** - Bakery item catalog management
- **[PowerUp CRUD Operations](docs/POWERUP_CRUD_OPERATIONS.md)** - Special abilities management

## Data Persistence

### File-Based Storage

All data is stored in JSON format for easy reading and version control:

- `players.json` - Player accounts and statistics
- `tiles.json` - Bakery item catalog
- `powerups.json` - Power-up definitions

### Auto-Save

- Automatic save after every create/update/delete operation
- Automatic load on service initialization
- Data integrity maintained through exception handling

### JSON Format Example

**players.json**
```json
[
  {
    "PlayerId": "7f8a3c2d-1e4b-5a9c-8d7e-6f5a4b3c2d1e",
    "Username": "BakerMaster",
    "Email": "baker@bakery.com",
    "HighestScore": 12450,
    "Level": 5,
    "GamesPlayed": 23,
    "IsActive": true
  }
]
```


## Future Enhancements

### Planned Features
- RESTful API with ASP.NET Core Web API
- Database integration (Entity Framework Core + SQL Server)
- Authentication and authorization (JWT)
- Real-time leaderboards with SignalR
- Achievement system
- Friend system and social features
- Season pass and tournament support
- Cloud save synchronization
- Admin dashboard (Blazor/React)

### Technical Improvements
- Unit tests (xUnit)
- Integration tests
- Logging (Serilog)
- Dependency injection
- Configuration management
- Docker containerization
- CI/CD pipeline (GitHub Actions)

---

Happy Baking!
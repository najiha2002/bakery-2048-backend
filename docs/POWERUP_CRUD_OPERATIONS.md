# PowerUp CRUD Operations - Bakery 2048

## Overview
PowerUps are special abilities that players can use during gameplay to gain advantages (Double Points, Undo Move, Shuffle Board, etc.). The admin/developer manages the catalog of available power-ups through CRUD operations.

---

## CREATE - Add New Power-Up

### Use Case
Add a new power-up ability to the game

### Example
```
=== Add New Power-Up ===
Enter power-up name (e.g., Double Score, Undo Move): Time Freeze

Available Power-Up Types:
[1] ScoreBoost - Multiplies score earned
[2] TimeExtension - Extends gameplay time
[3] Undo - Undo previous moves
[4] SwapTiles - Swap tile positions

Select power-up type (1-4): 2
Enter description: Extend gameplay time by 30 seconds
Enter duration (in seconds/moves): 30
Enter cost in points: 300
Enter cooldown (moves before reuse): 10
Is this power-up unlocked? (y/n): y
Enter effect multiplier (e.g., 2.0 for double): 1.0
Enter icon/emoji: ⏰

✓ Power-up 'Time Freeze' added successfully!
```

### When Needed
- Introducing new game mechanics
- Adding premium/purchasable abilities
- Creating event-specific power-ups
- Testing gameplay features

---

## READ - View/Search Power-Ups

### 1. View All Power-Ups
Display catalog of all available power-ups

```
=== All Power-Ups ===
Active Power-Ups: 4

Icon   Name                   Type               Cost       Cooldown     Unlocked  
─────────────────────────────────────────────────────────────────────────────────
⚡     Double Score          ScoreBoost         200        5            ✓         
⏰     Time Freeze           TimeExtension      300        10           ✓         
↶      Undo Move             Undo               150        8            ✓         
🔄     Tile Swap             SwapTiles          250        6            ✓         
✨     Triple Score          ScoreBoost         500        15           ✗         
```

### Display Features
- **Icons**: Emojis displayed for each power-up
- **Status**: ✓ for unlocked, ✗ for locked
- **Sorting**: Ordered by cost (low to high)
- **Active/Inactive**: Separate sections for deactivated power-ups

### 2. Search/Filter Options
- **By Type:** Show all "ScoreBoost" power-ups
- **By Cost:** Find affordable power-ups (< 200 points)
- **By Unlock Status:** Show only available power-ups
- **By Usage:** Most/least used power-ups
- **By Cooldown:** Quick recharge power-ups

### 3. View Single Power-Up Details
```
Power-Up: Double Points
Type: ScoreBoost
Description: Double all points earned for 10 moves
Duration: 10 moves
Cost: 200 points
Cooldown: 3 moves
Unlocked: Yes
Times Used (All Players): 1,547
Effect Multiplier: 2.0x
Icon: /assets/double-points.png
```

---

## UPDATE - Modify Power-Up Properties

### Use Cases

#### 1. Balance Adjustments
Change cost or duration for better gameplay
- Example: "Double Points is too cheap, increase cost from 200 to 250"
- Example: "Undo Move duration too short, increase from 1 to 2 uses"

#### 2. Cooldown Tuning
Adjust how often players can use power-ups
- Example: "Shuffle Board used too often, increase cooldown from 5 to 10 moves"

#### 3. Effect Strength
Modify multiplier for power effectiveness
- Example: "Triple Points too strong, reduce from 3.0x to 2.5x"

#### 4. Cost Rebalancing
Adjust in-game currency requirements
- Example: "Time Freeze underused, reduce cost from 500 to 400"

#### 5. Unlock Status
Gate power-ups behind progression
- Example: "Super Shuffle should be locked until Level 10"

### Update Menu Example
```
Updating: Double Points
1. Update Cost
2. Update Duration
3. Update Cooldown
4. Update Effect Multiplier
5. Update Description
6. Toggle Unlock Status
7. Cancel
```

---

## DELETE - Remove Power-Up

### Use Cases
- Remove overpowered/unbalanced abilities
- Delete experimental power-ups after testing
- Remove event-specific power-ups after event ends
- Clean up deprecated features

### Example
```
Delete "Instant Win"?
This will:
- Remove from shop/inventory
- Refund players who purchased it
- Remove from future gameplay
Confirm? (yes/no)
```

---

## Additional Operations

### Statistics/Analytics
- Most used power-up across all players
- Least used power-up (potential rebalance candidate)
- Average cost of all power-ups
- Total usage count per power-up type
- Power-up purchase to usage ratio
- Revenue generated per power-up (if monetized)

### Data Analysis with LINQ

#### Most Used Power-Ups
```csharp
var mostUsed = powerUps.OrderByDescending(p => p.UsageCount).Take(5);
```

#### Power-Ups by Type
```csharp
var byType = powerUps.GroupBy(p => p.PowerUpType)
                     .Select(g => new { 
                         Type = g.Key, 
                         Count = g.Count() 
                     });
```

#### Average Cost of All Power-Ups
```csharp
var avgCost = powerUps.Average(p => p.Cost);
```

#### Affordable Power-Ups (< 200 points)
```csharp
var affordable = powerUps.Where(p => p.Cost < 200);
```

#### Unlocked vs Locked Power-Ups
```csharp
var unlockedCount = powerUps.Count(p => p.IsUnlocked);
var lockedCount = powerUps.Count(p => !p.IsUnlocked);
```

#### Power-Ups with Highest Effect Multipliers
```csharp
var strongest = powerUps.OrderByDescending(p => p.EffectMultiplier)
                        .Take(3);
```

#### Power-Ups Sorted by Usage Count
```csharp
var sortedByUsage = powerUps.OrderByDescending(p => p.UsageCount);
```

#### Average Cooldown by Type
```csharp
var avgCooldownByType = powerUps.GroupBy(p => p.PowerUpType)
                                .Select(g => new { 
                                    Type = g.Key, 
                                    AvgCooldown = g.Average(p => p.Cooldown) 
                                });
```

---

## Practical Workflow Examples

### Game Designer Workflow
1. **CREATE** new "Mega Merge" power-up (merges all same tiles)
2. **READ** all BoardManipulation power-ups for consistency
3. **UPDATE** cost based on beta testing feedback
4. **VIEW STATISTICS** to monitor usage patterns
5. **UPDATE** cooldown if overused

### Balance Team Workflow
1. **READ** all power-ups with usage < 100
2. **UPDATE** costs to make them more attractive
3. **VIEW STATISTICS** after 1 week
4. **DELETE** if still unused (dead feature)

### Event Workflow
1. **CREATE** "Halloween Candy Rush" (temporary power-up)
2. **UPDATE** to unlock for all players during event
3. **VIEW STATISTICS** to track engagement
4. **DELETE** or **UPDATE** (lock) after event ends

### Monetization Workflow
1. **READ** all premium power-ups
2. **UPDATE** costs for promotional period
3. **VIEW STATISTICS** for revenue tracking
4. **UPDATE** back to original prices

---

## PowerUpService Implementation Structure

### Menu Options
```
=== Power-Ups Management ===
1. Add New Power-Up
2. View All Power-Ups
3. Search Power-Ups
4. View Power-Ups by Type
5. Update Power-Up Properties
6. Delete Power-Up
7. View Power-Up Statistics
8. Track Power-Up Usage
9. Back to Main Menu
```

### Data Storage
- File: `powerups.json`
- Format: JSON array of PowerUp objects
- Auto-save after every modification
- Auto-load on service initialization

---

## PowerUp Types

### 1. ScoreBoost
Multiplies points earned
- Example: Double Points, Triple Points, Score Frenzy

### 2. ExtraMove
Provides additional moves/actions
- Example: Undo Move, Extra Turn, Time Extension

### 3. BoardManipulation
Modifies the game board state
- Example: Shuffle Board, Clear Row, Merge Assist

### 4. TimeExtension
Extends gameplay time
- Example: Time Freeze, Slow Motion, Bonus Time

---

## Player Interaction Tracking

### When Player Uses Power-Up
```csharp
// In Player class
public void UsePowerUp()
{
    PowerUpsUsed++;
}

// In PowerUp class (increment usage count)
powerUp.UsageCount++;
```

### Analytics Queries
```csharp
// Total power-ups used by all players
var totalUsed = players.Sum(p => p.PowerUpsUsed);

// Players who never used power-ups
var neverUsed = players.Where(p => p.PowerUpsUsed == 0);

// Average power-ups per player
var avgPerPlayer = players.Average(p => p.PowerUpsUsed);
```

---

## Benefits of PowerUp CRUD System

1. **Dynamic Gameplay**
   - Add new mechanics without code changes
   - Quick iteration on game features

2. **Balance & Tuning**
   - Real-time adjustments based on data
   - A/B testing different configurations

3. **Monetization Support**
   - Flexible pricing strategies
   - Premium/free power-up tiers

4. **Player Engagement**
   - Seasonal/event power-ups
   - Progression-locked abilities

5. **Analytics-Driven Design**
   - Track usage patterns
   - Identify underutilized features
   - Data-driven decision making

6. **Maintainability**
   - Centralized power-up management
   - Easy rollback of changes
   - JSON persistence for backup/migration

---

## Integration with Player System

### Recording Power-Up Usage in Game Session
When recording a game session, track which power-ups were used:

```csharp
public void RecordGameSession(
    int finalScore, 
    int bestTileAchieved, 
    int movesMade, 
    TimeSpan playDuration, 
    List<string> powerUpsUsed,  // NEW: Track power-ups used
    bool reachedWinCondition = false)
{
    // Existing code...
    
    // Track power-up usage
    PowerUpsUsed += powerUpsUsed.Count;
    
    // Update power-up statistics
    foreach (var powerUpName in powerUpsUsed)
    {
        var powerUp = powerUpService.FindByName(powerUpName);
        if (powerUp != null)
        {
            powerUp.UsageCount++;
        }
    }
}
```

---

## Cross-Entity Analytics

### Players vs Power-Ups
```csharp
// Top 5 players by power-up usage
var topPowerUpUsers = players.OrderByDescending(p => p.PowerUpsUsed).Take(5);

// Power-ups never used by any player
var unusedPowerUps = powerUps.Where(p => p.UsageCount == 0);

// Correlation between power-up usage and high scores
var powerUpImpact = players.Where(p => p.PowerUpsUsed > 0)
                           .Average(p => p.HighestScore);
```

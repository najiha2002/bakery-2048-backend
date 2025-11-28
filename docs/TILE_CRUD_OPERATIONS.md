# Tile CRUD Operations - Bakery 2048

## Overview
Tiles represent the bakery items that appear in the game (Croissant, Donut, Cake, etc.). The admin/developer manages the catalog of available tiles through CRUD operations.

---

## CREATE - Add New Bakery Item

### Use Case
Add a new bakery item to the game catalog

### Tile Management Submenu
```
🍰 Tile Management
[1] Add New Tile
[2] View All Tiles
[3] Search Tile
[4] Update Tile
[5] Delete Tile
[6] View Tile Statistics
[0] Back to Main Menu
```

### Example
```
=== Add New Tile ===
Enter item name (e.g., Flour, Cookie): Rainbow Cake
Enter tile value (e.g., 2, 4, 8): 4096
Enter icon/emoji (e.g., 🌾, 🍪): 🌈
Enter color hex code (e.g., #F5DEB3): #FF00FF
Is this a special item? (y/n): y

✓ Tile 'Rainbow Cake' added successfully!

Tile ID: a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6
Item Name: Rainbow Cake
Tile Value: 4096
Icon: 🌈
Special Item: Yes
```

### When Needed
- Introducing new bakery items in updates
- Creating seasonal/event items
- Expanding the game catalog

---

## READ - View/Search Tiles

### 1. View All Tiles
Display catalog of all bakery items

```
=== All Tiles ===
Active Tiles: 12

Icon     Item Name            Value      Color        Special   
─────────────────────────────────────────────────────────────
🌾       Flour                2          #F5DEB3      -         
🍪       Cookie               4          #D2691E      -         
🧁       Cupcake              8          #FFB6C1      -         
🎂       Cake                 16         #FF69B4      -         
🍰       Cheesecake           32         #FFF8DC      ★         
🥐       Croissant            64         #FFE4B5      -         
🥧       Pie                  128        #DEB887      -         
🍩       Donut                256        #FF1493      ★         
🧇       Waffle               512        #F4A460      -         
🍮       Pudding              1024       #FAFAD2      -         
🎂       Wedding Cake         2048       #FFD700      ★         
🌈       Rainbow Cake         4096       #FF00FF      ★         
```

### Display Features
- **Icons**: Emoji icons for visual identification
- **Special Marker**: ★ indicates special/event items
- **Sorting**: Ordered by tile value (low to high)
- **Color Codes**: Hex color values for game rendering

### 2. Search/Filter Options
- **By Rarity:** Show all "Legendary" items
- **By Value:** Find tiles worth 2048 points
- **By Unlock Level:** Show items available at Level 5
- **By Special Status:** List event/seasonal items

### 3. View Single Tile Details
```
Item: Wedding Cake
Value: 4096
Rarity: Epic
Points When Merged: 40960
Unlock Level: 10
Appearance Rate: 5%
Color: White/Gold
Special Item: Yes (Valentine's Event)
```

---

## UPDATE - Modify Tile Properties

### Use Cases

#### 1. Balance Adjustments
Change appearance rates
- Example: "Legendary Cake appears too often, reduce rate from 10% to 5%"

#### 2. Point Rebalancing
Adjust merge rewards
- Example: "Croissant merge gives too few points, increase from 20 to 30"

#### 3. Level Gating
Change unlock requirements
- Example: "Rainbow Cake should unlock at Level 20, not 15"

#### 4. Visual Updates
Change colors/icons
- Example: "Update Donut color to pink"

#### 5. Rarity Changes
Rebalance item tiers
- Example: "Cupcake should be Common, not Uncommon"

### Update Menu Example
```
Updating: Croissant
1. Update Rarity
2. Update Points When Merged
3. Update Appearance Rate
4. Update Unlock Level
5. Toggle Special Item Status
6. Update Color
7. Cancel
```

---

## DELETE - Remove Tile

### Use Cases
- Remove discontinued items
- Delete test/experimental items
- Remove seasonal items after event ends

### Example
```
Delete "Halloween Pumpkin Pie"?
This will remove it from:
- Game catalog
- Player inventories
- Future spawns
Confirm? (yes/no)
```

---

## Additional Operations

### Statistics/Analytics
- Most merged tile across all players
- Rarest tile obtained by players
- Average appearance rate effectiveness
- Tiles by popularity
- Distribution by rarity tier

### Data Analysis with LINQ

#### All Legendary Items
```csharp
var legendaryItems = tiles.Where(t => t.Rarity == "Legendary");
```

#### Tiles Unlocked at Level 10+
```csharp
var highLevelTiles = tiles.Where(t => t.UnlockLevel >= 10);
```

#### Average Points Per Rarity Tier
```csharp
var avgByRarity = tiles.GroupBy(t => t.Rarity)
                       .Select(g => new { 
                           Rarity = g.Key, 
                           AvgPoints = g.Average(t => t.PointsWorthWhenMerged) 
                       });
```

#### Most Common Tiles (High Appearance Rate)
```csharp
var commonTiles = tiles.OrderByDescending(t => t.AppearanceRate).Take(5);
```

#### Tiles Sorted by Value
```csharp
var sortedByValue = tiles.OrderBy(t => t.TileValue);
```

#### Count by Rarity
```csharp
var countByRarity = tiles.GroupBy(t => t.Rarity)
                         .Select(g => new { 
                             Rarity = g.Key, 
                             Count = g.Count() 
                         });
```

---

## Practical Workflow Examples

### Game Designer Workflow
1. **CREATE** new "Summer Lemonade Cupcake" for summer event
2. **READ** all summer items to check consistency
3. **UPDATE** appearance rate based on player feedback
4. **DELETE** item after summer event ends

### Game Balance Workflow
1. **READ** all Epic tier items
2. **UPDATE** their appearance rates to make them rarer
3. **VIEW STATISTICS** to verify balance
4. **UPDATE** point values if needed

### Content Update Workflow
1. **CREATE** 5 new holiday-themed items
2. **UPDATE** existing seasonal items with new colors
3. **READ** and verify all holiday items
4. Schedule **DELETE** for items after event

---

## TileService Implementation Structure

### Menu Options
```
=== Bakery Items Management ===
1. Add New Bakery Item
2. View All Items
3. Search Items
4. View Items by Rarity
5. Update Item Properties
6. Delete Item
7. View Item Statistics
8. Back to Main Menu
```

### Data Storage
- File: `tiles.json`
- Format: JSON array of Tile objects
- Auto-save after every modification
- Auto-load on service initialization

---

## Benefits of Tile CRUD System

1. **Dynamic Content Management**
   - Add items without recompiling code
   - Easy content updates and patches

2. **Game Balance**
   - Quick adjustments to appearance rates
   - Fine-tune progression and rewards

3. **Event Support**
   - Create limited-time items
   - Easy cleanup after events

4. **Testing & Analytics**
   - Track item performance
   - Data-driven balance decisions

5. **Maintainability**
   - Centralized item management
   - JSON persistence for easy backup

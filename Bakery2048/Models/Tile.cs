using Bakery2048.Models;

public class Tile : BaseEntity
{
    // Inherited from BaseEntity: Id, DateCreated, DateModified, IsActive
    public string ItemName { get; set; }
    public int TileValue { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public bool IsSpecialItem { get; set; }

    // Convenience property
    public Guid TileId => Id; // Alias for Id

    public Tile(string itemName, int tileValue) : base()
    {
        ItemName = itemName;
        TileValue = tileValue;
        Icon = string.Empty;
        Color = "#FFFFFF";
        IsSpecialItem = false;
    }

    public Tile() : base()
    {
        ItemName = string.Empty;
        TileValue = 0;
        Icon = string.Empty;
        Color = "#FFFFFF";
        IsSpecialItem = false;
    }
}

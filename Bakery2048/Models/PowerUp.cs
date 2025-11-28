using Bakery2048.Models;

public class PowerUp : BaseEntity
{
    // Inherited from BaseEntity: Id, DateCreated, DateModified, IsActive
    public string PowerUpName { get; set; }
    public PowerUpType PowerUpType { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public int Cost { get; set; }
    public int Cooldown { get; set; }
    public bool IsUnlocked { get; set; }
    public int UsageCount { get; set; }
    public double EffectMultiplier { get; set; }
    public string IconUrl { get; set; }

    // Convenience property
    public Guid PowerUpId => Id; // Alias for Id

    public PowerUp(string powerUpName, PowerUpType powerUpType, int cost) : base()
    {
        PowerUpName = powerUpName;
        PowerUpType = powerUpType;
        Description = string.Empty;
        Duration = 1;
        Cost = cost;
        Cooldown = 3;
        IsUnlocked = true;
        UsageCount = 0;
        EffectMultiplier = 1.0;
        IconUrl = string.Empty;
    }

    public PowerUp() : base()
    {
        PowerUpName = string.Empty;
        PowerUpType = PowerUpType.ScoreBoost;
        Description = string.Empty;
        Duration = 1;
        Cost = 0;
        Cooldown = 3;
        IsUnlocked = true;
        UsageCount = 0;
        EffectMultiplier = 1.0;
        IconUrl = string.Empty;
    }
}

using System.ComponentModel;

namespace Assets.Scripts
{
    public enum EntityStatTypes 
    {
        [Description("Max Health")]
        MaxHealth,
        [Description("Health")]
        CurrentHealth,
        [Description("Max Energy")]
        MaxEnergy,
        [Description("Energy")]
        CurrentEnergy,
        [Description("Max Morale")]
        MaxMorale,
        [Description("Morale")]
        CurrentMorale,
        [Description("Initiative")]
        Initiative,
        [Description("Attack")]
        Attack,
        [Description("Armor")]
        Armor,
        [Description("Critical")]
        Critical,
        [Description("Max Action Points")]
        MaxActionPoints,
        [Description("Current Action Points")]
        CurrentActionPoints
    }
}

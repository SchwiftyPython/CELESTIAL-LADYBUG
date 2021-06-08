using System.ComponentModel;

namespace Assets.Scripts.Entities
{
    public enum EntityClass 
    {
        [Description("Spearman")] Spearman,
        [Description("Crossbowman")] Crossbowman,
        [Description("Man-at-Arms")] ManAtArms,
        [Description("Gladiator")] Gladiator,
        [Description("Wizard")] Wizard,
        [Description("Battle-Mage")] BattleMage,
        [Description("Knight")] Knight,
        [Description("Paladin")] Paladin,
        [Description("Derpus")] Derpus,
        [Description("Beast")] Beast,
        [Description("Ethereal")] Ethereal,
    }
}

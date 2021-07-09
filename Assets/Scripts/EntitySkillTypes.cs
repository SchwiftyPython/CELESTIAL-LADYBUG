using System.ComponentModel;

namespace Assets.Scripts
{
    public enum EntitySkillTypes
    {
        [Description("Melee")]
        Melee,
        [Description("Ranged")]
        Ranged,
        [Description("Lockpicking")]
        Lockpicking,
        [Description("Endurance")]
        Endurance,
        [Description("Healing")]
        Healing,
        [Description("Survival")]
        Survival,
        [Description("Persuasion")]
        Persuasion,
        [Description("Dodge")]
        Dodge
    }
}
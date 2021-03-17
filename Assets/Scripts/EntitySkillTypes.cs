using System.ComponentModel;

namespace Assets.Scripts
{
    public enum EntitySkillTypes
    {
        [Description("Dodge")]
        Dodge,
        [Description("Lockpicking")]
        Lockpicking,
        [Description("Toughness")]
        Toughness,
        [Description("Healing")]
        Healing,
        [Description("Survival")]
        Survival,
        [Description("Persuasion")]
        Persuasion
    }
}
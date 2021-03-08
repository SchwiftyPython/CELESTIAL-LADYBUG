using System.ComponentModel;

namespace Assets.Scripts
{
    public enum EntitySkillTypes
    {
        [Description("Dodge")]
        Dodge,
        [Description("Lockpicking")]
        Lockpicking,
        [Description("Stamina")]
        Stamina,
        [Description("Healing")]
        Healing,
        [Description("Survival")]
        Survival,
        [Description("Persuasion")]
        Persuasion
    }
}
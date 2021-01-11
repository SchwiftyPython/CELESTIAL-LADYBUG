﻿using System.ComponentModel;

namespace Assets.Scripts
{
    public enum EntityStatTypes 
    {
        [Description("Max Health")]
        MaxHealth,
        [Description("Current Health")]
        CurrentHealth,
        [Description("Max Energy")]
        MaxEnergy,
        [Description("Current Energy")]
        CurrentEnergy,
        [Description("Max Morale")]
        MaxMorale,
        [Description("Current Morale")]
        CurrentMorale,
        [Description("Initiative")]
        Initiative,
        [Description("Attack")]
        Attack,
        [Description("Melee Skill")]
        MeleeSkill,
        [Description("Ranged Skill")]
        RangedSkill,
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
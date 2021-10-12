using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Abilities;
using Assets.Scripts.Entities;
using UnityEngine;

public class CrushingBlow : Ability
{
    private const int DamageMin = 4;
    private const int DamageMax = 7;

    public CrushingBlow(Entity abilityOwner) : base("Crushing Blow", "Attack target with a heavy unarmed strike.", 6, 1, abilityOwner, TargetType.Hostile, false, false)
    {
    }

    public override (int, int) GetAbilityDamageRange()
    {
        return (DamageMin, DamageMax);
    }
}

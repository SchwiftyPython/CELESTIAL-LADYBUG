﻿using Assets.Scripts.Abilities;
using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Rampart
{
    public class Treant : Entity
    {
        public Treant() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Treant");

            var abilityStore = Object.FindObjectOfType<AbilityStore>();

            var barkAttack = abilityStore.GetAbilityByName("bark blast", this);

            Abilities.Add(barkAttack.GetType(), barkAttack);

            var tank = abilityStore.GetAbilityByName("tank", this);

            Abilities.Add(tank.GetType(), tank);

            var audioStore = Object.FindObjectOfType<AudioStore>();

            HurtSound = audioStore.monsterHurt;
            DieSound = audioStore.monsterDie;
            AttackSound = audioStore.genericAttack;
        }
    }
}

﻿using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Skeleton : Entity
    {
        public Skeleton() : base(Race.RaceType.Undead, EntityClass.ManAtArms, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Skeleton");
        }
    }
}

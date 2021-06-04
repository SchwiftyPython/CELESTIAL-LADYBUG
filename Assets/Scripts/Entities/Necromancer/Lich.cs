using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class Lich : Entity
    {
        public Lich() : base(Race.RaceType.Undead, EntityClass.Wizard, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Lich");
        }
    }
}

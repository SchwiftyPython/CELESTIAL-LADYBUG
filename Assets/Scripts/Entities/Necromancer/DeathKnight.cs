using UnityEngine;

namespace Assets.Scripts.Entities.Necromancer
{
    public class DeathKnight : Entity
    {
        public DeathKnight() : base(Race.RaceType.Undead, EntityClass.BattleMage, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("DeathKnight");
        }
    }
}

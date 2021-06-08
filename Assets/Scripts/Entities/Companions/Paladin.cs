using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Paladin : Entity
    {
        public Paladin(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Paladin, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Paladin");
        }
    }
}

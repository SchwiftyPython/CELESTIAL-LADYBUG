using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Gladiator : Entity
    {
        public Gladiator(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Gladiator, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Gladiator");
        }
    }
}

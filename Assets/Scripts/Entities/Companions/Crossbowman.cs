using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Crossbowman : Entity
    {
        public Crossbowman(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Crossbowman, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Crossbowman");
        }
    }
}

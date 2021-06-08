using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Knight : Entity
    {
        public Knight(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Knight, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Knight");
        }
    }
}

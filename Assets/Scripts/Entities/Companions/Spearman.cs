using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Spearman : Entity
    {
        public Spearman(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Spearman, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Spearman");
        }
    }
}

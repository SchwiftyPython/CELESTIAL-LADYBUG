using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class ManAtArms : Entity
    {
        public ManAtArms(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.ManAtArms, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("ManAtArms");
        }
    }
}

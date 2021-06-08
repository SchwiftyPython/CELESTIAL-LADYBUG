using UnityEngine;

namespace Assets.Scripts.Entities.Companions
{
    public class Wizard : Entity
    {
        public Wizard(Race.RaceType rType, bool isPlayer) : base(rType, EntityClass.Wizard, isPlayer)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Wizard");
        }
    }
}

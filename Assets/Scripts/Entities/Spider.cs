using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Spider : Entity
    {
        public Spider() : base(Race.RaceType.Beast, EntityClass.Beast, false)
        {
            var entityPrefabStore = Object.FindObjectOfType<EntityPrefabStore>();

            CombatSpritePrefab = entityPrefabStore.GetCombatSpritePrefab("Spider");

            var bite = new Bite(this);

            Abilities.Add(bite.GetType(), bite);
        }
    }
}

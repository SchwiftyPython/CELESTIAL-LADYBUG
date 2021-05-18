using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class AbilityStore : MonoBehaviour
    {
        private Dictionary<string, Func<Entity, Ability>> _allAbilities = new Dictionary<string, Func<Entity, Ability>>
        {
            {"tank", abilityOwner => new Tank(abilityOwner)},
            {"well fed", abilityOwner => new WellFed(abilityOwner)},
            {"riposte", abilityOwner => new Riposte(abilityOwner)},
            {"quick recovery", abilityOwner => new QuickRecovery(abilityOwner)},
            {"guided strikes", abilityOwner => new GuidedStrikes(abilityOwner)},
            {"uncanny dodge", abilityOwner => new UncannyDodge(abilityOwner)},
            {"calculated", abilityOwner => new Calculated(abilityOwner)},
            {"divine intervention", abilityOwner => new DivineIntervention(abilityOwner)},
            {"endangered endurance", abilityOwner => new EndangeredEndurance(abilityOwner)},
            {"helmet charge", abilityOwner => new HelmetCharge(abilityOwner)},
            {"snapshot", abilityOwner => new Snapshot(abilityOwner)},
            {"demonic intervention", abilityOwner => new DemonicIntervention(abilityOwner)},
            {"works out", abilityOwner => new WorksOut(abilityOwner)},
            {"shoot", abilityOwner => new Shoot(abilityOwner)},
            {"aimed shot", abilityOwner => new AimedShot(abilityOwner)},
            {"knockback", abilityOwner => new Knockback(abilityOwner)},
            {"distracting", abilityOwner => new Distracting(abilityOwner)},
            {"soulbound", abilityOwner => new Soulbound(abilityOwner)},
            {"massive damage", abilityOwner => new MassiveDamage(abilityOwner)},
            {"cumbersome", abilityOwner => new Cumbersome(abilityOwner)}
        };

        public Ability GetAbilityByName(string abilityName, Entity abilityOwner)
        {
            abilityName = abilityName.ToLower();

            if (!_allAbilities.ContainsKey(abilityName))
            {
                Debug.LogError($"Ability {abilityName} does not exist!");
                return null;
            }

            return _allAbilities[abilityName].Invoke(abilityOwner);
        }
    }
}

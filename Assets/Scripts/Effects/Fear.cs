using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects.Args;
using GoRogue;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Effects
{
    public class Fear : Effect
    {
        private const int FearDuration = 4;
        private const int PanicChance = 20;

        public Fear(bool locationDependent, int duration = FearDuration) : base("Fear", $"{PanicChance}% chance that target loses turn and attacks at random if able.", duration, locationDependent, false)
        {
        }

        public override void Trigger(EffectArgs args)
        {
            if (Duration != INFINITE)
            {
                Duration--;
            }

            var basicEffectArgs = args as BasicEffectArgs;

            if (basicEffectArgs == null)
            {
                return;
            }

            if (Random.Range(1, 101) > PanicChance)
            {
                return;
            }

            var message = $"{basicEffectArgs.Target.Name} panics!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            var combatManager = Object.FindObjectOfType<CombatManager>();
            var targets = combatManager.TurnOrder.ToList();

            targets.Remove(basicEffectArgs.Target);

            targets = GlobalHelper.ShuffleList(targets);

            bool attackUsed = false;

            foreach (var target in targets.ToArray())
            {
                foreach (var ability in basicEffectArgs.Target.Abilities.Values)
                {
                    if (ability.IsPassive || !ability.TargetInRange(target) || !ability.HostileTargetsOnly ||
                        ability.ApCost > basicEffectArgs.Target.Stats.CurrentActionPoints)
                    {
                        continue;
                    }

                    ability.Use(target);
                    attackUsed = true;
                    break;
                }

                if (attackUsed)
                {
                    break;
                }
            }

            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }
    }
}

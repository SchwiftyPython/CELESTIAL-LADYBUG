using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Effects
{
    public class Blinded : Effect, IModifierProvider
    {
        private const int BlindDuration = 4;
        private const int ToHitPenalty = -25;

        public Blinded(bool locationDependent, int duration = BlindDuration) : base("Blinded", $"To hit chance reduced by {Math.Abs(ToHitPenalty)}%.",
            duration, locationDependent, false)
        {
        }

        protected override void OnTrigger(EffectArgs e)
        {
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes combatModType))
            {
                yield return 0f;
            }

            if (combatModType == CombatModifierTypes.MeleeToHit || combatModType == CombatModifierTypes.RangedToHit)
            {
                yield return ToHitPenalty;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}

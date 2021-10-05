using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Effects
{
    public class Blinded : Effect, IModifierProvider
    {
        private const int BlindDuration = 4;
        private const int ToHitPenalty = -25;

        public Blinded(Entity owner, bool locationDependent, int duration = BlindDuration) : base("Blinded", $"To hit chance reduced by {Math.Abs(ToHitPenalty)}%.",
            duration, locationDependent, false, TargetType.Hostile, owner)
        {
        }

        protected override void OnTrigger(EffectArgs e)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes combatModType))
            {
                return 0f;
            }

            if (combatModType == CombatModifierTypes.MeleeToHit || combatModType == CombatModifierTypes.RangedToHit)
            {
                return ToHitPenalty;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}

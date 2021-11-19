using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Effects
{
    public class Nimble : Effect, IModifierProvider
    {
        private const int NimbleDuration = 3;
        private const int DodgeMod = 2;

        public Nimble()
        {
        }

        public Nimble(Entity owner, bool locationDependent, int duration = NimbleDuration) : base("Nimble",
            $"+{DodgeMod} to dodge.", duration, locationDependent, false, TargetType.Friendly, owner)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(EntitySkillTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out EntitySkillTypes skillType))
            {
                return 0f;
            }

            if (skillType == EntitySkillTypes.Dodge)
            {
                return DodgeMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}

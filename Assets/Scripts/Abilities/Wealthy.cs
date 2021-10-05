using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Wealthy : Ability, IModifierProvider
    {
        private const int GoldBonus = 5;

        public Wealthy(Entity abilityOwner) : base("Wealthy", $"+{GoldBonus}% earned gold.", -1, -1, abilityOwner, TargetType.Friendly,
            true)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            return 0f;
        }

        public float GetPercentageModifiers(Enum supply)
        {
            if (!supply.GetType().Name.Equals(nameof(PartySupplyTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(supply.ToString(), out PartySupplyTypes supplyType))
            {
                return 0f;
            }

            if (supplyType == PartySupplyTypes.Gold)
            {
                return GoldBonus;
            }

            return 0f;
        }
    }
}

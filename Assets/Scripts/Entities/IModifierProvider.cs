using System;

namespace Assets.Scripts.Entities
{
    public interface IModifierProvider
    {
        float GetAdditiveModifiers(Enum stat);
        float GetPercentageModifiers(Enum stat);
    }
}

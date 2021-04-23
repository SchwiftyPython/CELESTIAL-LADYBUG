using System;
using System.Collections.Generic;

namespace Assets.Scripts.Entities
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Enum stat);
        IEnumerable<float> GetPercentageModifiers(Enum stat);
    }
}

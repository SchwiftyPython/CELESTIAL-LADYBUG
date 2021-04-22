using System.Collections.Generic;

namespace Assets.Scripts.Entities
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers<T>(T stat);
        IEnumerable<float> GetPercentageModifiers<T>(T stat);
    }
}

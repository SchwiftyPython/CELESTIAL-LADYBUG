using Assets.Scripts.Effects;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Allows the `EffectTooltipSpawner` to display the right information.
    /// </summary>
    public interface IEffectHolder
    {
        Effect GetEffect();
    }
}

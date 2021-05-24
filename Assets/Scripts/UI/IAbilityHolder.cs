using Assets.Scripts.Abilities;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Allows the `AbilityTooltipSpawner` to display the right information.
    /// </summary>
    public interface IAbilityHolder
    {
        Ability GetAbility();
    }
}

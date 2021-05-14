using Assets.Scripts.Entities;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Allows the `EntityTooltipSpawner` to display the right information.
    /// </summary>
    public interface IEntityHolder
    {
        Entity GetEntity();
    }
}
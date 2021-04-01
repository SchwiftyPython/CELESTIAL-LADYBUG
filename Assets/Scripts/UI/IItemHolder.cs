using Assets.Scripts.Items;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Allows the `ItemTooltipSpawner` to display the right information.
    /// </summary>
    public interface IItemHolder
    {
        Item GetItem();
    }
}

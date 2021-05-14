using Assets.Scripts.Combat;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Allows the `TileTooltipSpawner` to display the right information.
    /// </summary>
    public interface ITileHolder
    {
        Tile GetTile();
    }
}

using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(ITileHolder))]
    [RequireComponent(typeof(IEntityHolder))]
    public class TerrainTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var terrainTooltip = tooltip.GetComponent<TerrainTooltip>();
            if (!terrainTooltip)
            {
                return;
            }

            var tile = GetComponent<ITileHolder>().GetTile();

            terrainTooltip.Setup(tile);
        }

        public override bool CanCreateTooltip()
        {
            var entity = GetComponent<IEntityHolder>().GetEntity();

            if (entity != null)
            {
                return false;
            }

            Debug.Log($"No Entity! Spawn Terrain ToolTip.");

            var tile = GetComponent<ITileHolder>().GetTile();

            return tile != null;
        }
    }
}

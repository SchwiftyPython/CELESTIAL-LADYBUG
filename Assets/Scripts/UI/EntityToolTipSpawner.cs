using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(IEntityHolder))]
    public class EntityToolTipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var entityTooltip = tooltip.GetComponent<EntityTooltip>();
            if (!entityTooltip)
            {
                return;
            }

            var entity = GetComponent<IEntityHolder>().GetEntity();

            entityTooltip.Setup(entity);
        }

        public override bool CanCreateTooltip()
        {
            var entity = GetComponent<IEntityHolder>().GetEntity();

            if (entity == null)
            {
                Debug.Log($"No Entity! Can't Spawn Entity Tooltip");
            }

            return entity != null;
        }
    }
}

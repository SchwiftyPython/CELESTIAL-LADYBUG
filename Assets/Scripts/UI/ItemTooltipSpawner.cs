using Assets.Scripts.Entities;
using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be placed on a UI slot to spawn and show the correct item tooltip.
    /// </summary>
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner, ISubscriber
    {
        private Entity _currentCompanion;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(GlobalHelper.PopulateCharacterSheet, this);
        }

        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>().GetItem();
            return item != null;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip)
            {
                return;
            }

            var item = GetComponent<IItemHolder>().GetItem();

            if (_currentCompanion == null)
            {
                _currentCompanion = GetCurrentCompanion();
            }

            itemTooltip.Setup(item, _currentCompanion);
        }

        private static Entity GetCurrentCompanion()
        {
            var partyManagementWindow = GameObject.Find("PartyManagementWindowMask");
            var windowScript = partyManagementWindow.GetComponent<PartyManagementWindow>();
            return windowScript.GetCurrentCompanion();
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.PopulateCharacterSheet))
            {
                if (!(parameter is Entity companion))
                {
                    return;
                }

                _currentCompanion = companion;
            }
        }
    }
}
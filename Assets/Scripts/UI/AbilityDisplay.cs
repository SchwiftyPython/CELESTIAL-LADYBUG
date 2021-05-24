using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AbilityDisplay : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.EquipmentUpdated;
        private const string PopulateEvent = GlobalHelper.PopulateCharacterSheet;

        private Equipment _companionEquipment;
        private Entity _currentCompanion;

        [SerializeField] private GameObject _abilitySlotPrefab; 

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(RefreshEvent, this);
            eventMediator.SubscribeToEvent(PopulateEvent, this);
        }

        private void RedrawUi()
        {
            GlobalHelper.DestroyAllChildren(gameObject);

            foreach (var ability in _currentCompanion.Abilities)
            {
                var abilitySlot = Instantiate(_abilitySlotPrefab, Vector3.zero, Quaternion.identity);

                abilitySlot.transform.SetParent(transform);

                var script = abilitySlot.GetComponentInChildren<AbilitySlotUi>();

                if (script != null)
                {
                    script.SetAbility(ability.Value);
                }
            }
        }


        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopulateEvent))
            {
                if (!(parameter is Entity companion) || companion.GetEquipment() == null)
                {
                    return;
                }

                _currentCompanion = companion;
                _companionEquipment = companion.GetEquipment();

                RedrawUi();
            }
            else if (eventName.Equals(RefreshEvent))
            {
                RedrawUi();
            }
        }
    }
}

using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ActiveEntityStats : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _apValue;

        [SerializeField]
        private TextMeshProUGUI _hpValue;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(RefreshEvent, this);
        }

        private void Populate(Entity activeEntity)
        {
            _name.text = activeEntity.Name;
            _apValue.text = $@"{activeEntity.Stats.CurrentActionPoints}/{activeEntity.Stats.MaxActionPoints}";
            _hpValue.text = $@"{activeEntity.Stats.CurrentHealth}/{activeEntity.Stats.MaxHealth}";
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                if (!(parameter is Entity activeEntity))
                {
                    return;
                }

                Populate(activeEntity);
            }
        }
    }
}

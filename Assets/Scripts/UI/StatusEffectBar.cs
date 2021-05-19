using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class StatusEffectBar : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;

        private Entity _activeEntity;

        [SerializeField] private GameObject _effectPrefab;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(RefreshEvent, this);
        }

        private void RedrawUi()
        {
            GlobalHelper.DestroyAllChildren(gameObject);

            foreach (var effect in _activeEntity.EffectTriggers.Effects)
            {
                var effectSlot = Instantiate(_effectPrefab, Vector3.zero, Quaternion.identity);

                effectSlot.transform.SetParent(transform);

                var script = effectSlot.GetComponentInChildren<EffectSlotUi>();

                if (script != null)
                {
                    script.SetEffect((Effect) effect);
                }
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                if (!(parameter is Entity activeEntity))
                {
                    return;
                }

                _activeEntity = activeEntity;

                RedrawUi();
            }
        }
    }
}

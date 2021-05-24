using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TerrainTooltip : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.TileSelected;

        [SerializeField]
        private TextMeshProUGUI tileType;

        [SerializeField]
        private TextMeshProUGUI apCost;

        [SerializeField]
        private GameObject tileEffectsParent;

        [SerializeField]
        private GameObject tileEffectTextPrefab;

        private bool _subscribed;
        private CombatInputController _inputController;

        private void Start()
        {
            _inputController = FindObjectOfType<CombatInputController>();
        }

        public void Setup(Tile tile, int totalApCost = 0)
        {
            GlobalHelper.DestroyAllChildren(tileEffectsParent);

            tileType.text = tile.TileType.ToString();

            if (ReferenceEquals(_inputController, null))
            {
                _inputController = FindObjectOfType<CombatInputController>();
            }

            if (tile is Floor floor)
            {
                if (_inputController.TileSelected() && ReferenceEquals(tile, _inputController.GetSelectedTile()))
                {

                    if (totalApCost <= 0)
                    {
                        totalApCost = _inputController.GetTotalTileMovementCost();
                    }

                    apCost.text = $"{totalApCost} AP to move here";
                }
                else
                {
                    apCost.text = $"{floor.ApCost} AP to cross";
                }
            }
            else
            {
                apCost.text = "Impassable";
            }

            var effects = tile.GetEffects();

            if (effects == null || !effects.Any())
            {
                tileEffectsParent.SetActive(false);
            }
            else
            {
                var listedEffects = new List<string>();

                tileEffectsParent.SetActive(true);

                foreach (var effect in effects)
                {
                    if (listedEffects.Contains(effect.Name))
                    {
                        continue;
                    }

                    listedEffects.Add(effect.Name);

                    var effectText = Instantiate(tileEffectTextPrefab, tileEffectsParent?.transform);

                    effectText.GetComponent<TextMeshProUGUI>().text = effect.Name;
                }
            }

            if (_subscribed)
            {
                return;
            }

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(RefreshEvent, this);

            _subscribed = true;
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.UnsubscribeFromEvent(RefreshEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                if (!(broadcaster is Floor floorTile))
                {
                    return;
                }

                if (!(parameter is int totalApCost))
                {
                    return;
                }

                Setup(floorTile, totalApCost);
            }
        }
    }
}

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
        private TextMeshProUGUI _tileType;

        [SerializeField]
        private TextMeshProUGUI _apCost;

        [SerializeField]
        private GameObject _tileEffectsParent;

        [SerializeField]
        private GameObject _tileEffectTextPrefab;

        private bool _subscribed;

        public void Setup(Tile tile, int totalApCost = 0)
        {
            GlobalHelper.DestroyAllChildren(_tileEffectsParent);

            _tileType.text = tile.TileType.ToString();

            if (tile is Floor floor)
            {
                var inputController = FindObjectOfType<CombatInputController>();

                if (inputController.TileSelected() && ReferenceEquals(tile, inputController.GetSelectedTile()))
                {

                    if (totalApCost <= 0)
                    {
                        totalApCost = inputController.GetTotalTileMovementCost();
                    }

                    _apCost.text = $"{totalApCost} AP to move here";
                }
                else
                {
                    _apCost.text = $"{floor.ApCost} AP to cross";
                }
            }
            else
            {
                _apCost.text = "Impassable";
            }

            var effects = tile.GetEffects();

            if (effects == null || !effects.Any())
            {
                _tileEffectsParent?.SetActive(false);
            }
            else
            {
                var listedEffects = new List<string>();

                _tileEffectsParent?.SetActive(true);

                foreach (var effect in effects)
                {
                    if (listedEffects.Contains(effect.Name))
                    {
                        continue;
                    }

                    listedEffects.Add(effect.Name);

                    var effectText = Instantiate(_tileEffectTextPrefab, _tileEffectsParent?.transform);

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

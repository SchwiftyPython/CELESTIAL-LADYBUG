using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TerrainTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _tileType;

        [SerializeField]
        private TextMeshProUGUI _apCost;

        [SerializeField]
        private GameObject _tileEffectsParent;

        [SerializeField]
        private GameObject _tileEffectTextPrefab;

        public void Setup(Tile tile)
        {
            _tileType.text = tile.TileType.ToString();

            if (tile is Floor floor)
            {
                _apCost.text = $"{floor.ApCost} AP to cross";
            }
            else
            {
                _apCost.text = "Impassable";
            }

            var effects = tile.GetEffects();

            if (effects == null || !effects.Any())
            {
                _tileEffectsParent.SetActive(false);
            }
            else
            {
                var listedEffects = new List<string>();

                _tileEffectsParent.SetActive(true);

                foreach (var effect in effects)
                {
                    if (listedEffects.Contains(effect.Name))
                    {
                        continue;
                    }

                    listedEffects.Add(effect.Name);

                    var effectText = Instantiate(_tileEffectTextPrefab, _tileEffectsParent.transform);

                    effectText.GetComponent<TextMeshProUGUI>().text = effect.Name;
                }
            }
        }
    }
}

using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PostCombatCompanionStats : MonoBehaviour
    {
        [SerializeField] private Portrait cPortrait;
        [SerializeField] private TextMeshProUGUI cName;
        [SerializeField] private TextMeshProUGUI kills;
        [SerializeField] private TextMeshProUGUI damageDealt;
        [SerializeField] private TextMeshProUGUI damageReceived;
        [SerializeField] private TextMeshProUGUI xp;

        public void Populate(KeyValuePair<Entity, CompanionCombatStats> companionsStats)
        {
            SetPortrait(companionsStats.Key.Portrait);

            cName.text = companionsStats.Key.FirstName();
            kills.text = companionsStats.Value.Kills.ToString();
            damageDealt.text = companionsStats.Value.DamageDealt.ToString();
            damageReceived.text = companionsStats.Value.DamageReceived.ToString();
            xp.text = companionsStats.Value.Xp.ToString();
        }

        private void SetPortrait(Dictionary<Portrait.Slot, string> portraitKeys)
        {
            var sprites = new Dictionary<Portrait.Slot, Sprite>();

            var spriteStore = FindObjectOfType<SpriteStore>();

            foreach (var slot in portraitKeys.Keys)
            { var slotSprite = spriteStore.GetPortraitSpriteForSlotByKey(slot, portraitKeys[slot]);
                sprites.Add(slot, slotSprite);
            }

            cPortrait.SetPortrait(sprites);
        }
    }
}

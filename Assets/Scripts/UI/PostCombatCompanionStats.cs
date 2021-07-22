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
        [SerializeField] private TextMeshProUGUI hp;

        public void Populate(KeyValuePair<Entity, CompanionCombatStats> companionStats)
        {
            SetPortrait(companionStats.Key.Portrait);

            cName.text = companionStats.Key.FirstName();
            kills.text = companionStats.Value.Kills.ToString();
            damageDealt.text = companionStats.Value.DamageDealt.ToString();
            damageReceived.text = companionStats.Value.DamageReceived.ToString();
            hp.text = $"{companionStats.Key.Stats.CurrentHealth}/{companionStats.Key.Stats.MaxHealth}" ;
        }

        private void SetPortrait(Dictionary<Portrait.Slot, string> portraitKeys)
        {
            var sprites = new Dictionary<Portrait.Slot, Sprite>();

            var spriteStore = FindObjectOfType<SpriteStore>();

            foreach (var slot in portraitKeys.Keys)
            { 
                var slotSprite = spriteStore.GetPortraitSpriteForSlotByKey(slot, portraitKeys[slot]);
                sprites.Add(slot, slotSprite);
            }

            cPortrait.SetPortrait(sprites);
        }
    }
}

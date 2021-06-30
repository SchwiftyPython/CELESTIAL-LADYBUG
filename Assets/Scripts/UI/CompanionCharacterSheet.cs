using System.Collections.Generic;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CompanionCharacterSheet : MonoBehaviour, ISubscriber
    {
        private const string PopulateCharacterSheet = GlobalHelper.PopulateCharacterSheet;
        private const string EquipmentUpdated = GlobalHelper.EquipmentUpdated;

        [SerializeField] private GameObject _portraitParent;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _raceClass;

        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _energy;
        [SerializeField] private TextMeshProUGUI _morale;
        [SerializeField] private TextMeshProUGUI _melee;
        [SerializeField] private TextMeshProUGUI _ranged;
        [SerializeField] private TextMeshProUGUI _lockpick;
        [SerializeField] private TextMeshProUGUI _toughness;
        [SerializeField] private TextMeshProUGUI _healing;
        [SerializeField] private TextMeshProUGUI _agility;
        [SerializeField] private TextMeshProUGUI _coordination;
        [SerializeField] private TextMeshProUGUI _physique;
        [SerializeField] private TextMeshProUGUI _intellect;
        [SerializeField] private TextMeshProUGUI _acumen;
        [SerializeField] private TextMeshProUGUI _charisma;
        [SerializeField] private TextMeshProUGUI _survival;
        [SerializeField] private TextMeshProUGUI _persuasion;

        private Portrait _portrait;
        private Entity _companion;

        private void Awake()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(PopulateCharacterSheet, this);
            eventMediator.SubscribeToEvent(EquipmentUpdated, this);
        }

        private void Populate()
        {
            SetPortrait(_companion.Portrait);

            _name.text = _companion.Name;
            _raceClass.text = $"{_companion.Race.GetRaceType()} {GlobalHelper.GetEnumDescription(_companion.EntityClass)}";

            var stats = _companion.Stats;

            _health.text = $"{stats.CurrentHealth}/{stats.MaxHealth}";
            _energy.text = $"{stats.CurrentEnergy}/{stats.MaxEnergy}";
            _morale.text = $"{stats.CurrentMorale}/{stats.MaxMorale}";

            var skills = _companion.Skills;

            _melee.text = skills.Melee.ToString();
            _ranged.text = skills.Ranged.ToString();
            _lockpick.text = skills.Lockpicking.ToString();
            _toughness.text = skills.Endurance.ToString();
            _healing.text = skills.Healing.ToString();
            _survival.text = skills.Survival.ToString();
            _persuasion.text = skills.Persuasion.ToString();

            var attributes = _companion.Attributes;

            _agility.text = attributes.Agility.ToString();
            _coordination.text = attributes.Coordination.ToString();
            _physique.text = attributes.Physique.ToString();
            _intellect.text = attributes.Intellect.ToString();
            _acumen.text = attributes.Acumen.ToString();
            _charisma.text = attributes.Charisma.ToString();
        }

        private void SetPortrait(Dictionary<Portrait.Slot, string> portraitKeys)
        {
            var sprites = new Dictionary<Portrait.Slot, Sprite>();

            foreach (var slot in portraitKeys.Keys)
            {
                var spriteStore = Object.FindObjectOfType<SpriteStore>();
                var slotSprite = spriteStore.GetPortraitSpriteForSlotByKey(slot, portraitKeys[slot]);
                sprites.Add(slot, slotSprite);
            }

            if (_portrait == null)
            {
                _portrait = _portraitParent.GetComponent<Portrait>();
            }

            _portrait.SetPortrait(sprites);
        }


        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopulateCharacterSheet))
            {
                if(!(parameter is Entity companion))
                {
                    return;
                }

                _companion = companion;

                Populate();
            }
            else if(eventName.Equals(EquipmentUpdated))
            {
                Populate();
            }
        }
    }
}

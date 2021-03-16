using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CompanionCharacterSheet : MonoBehaviour, ISubscriber
    {
        private const string PopulateCharacterSheet = GlobalHelper.PopulateCharacterSheet;

        [SerializeField] private GameObject _portrait;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _raceClass;

        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _energy;
        [SerializeField] private TextMeshProUGUI _morale;
        [SerializeField] private TextMeshProUGUI _attack;
        [SerializeField] private TextMeshProUGUI _dodge;
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

        //private Entity _companion;

        private void Awake()
        {
            EventMediator.Instance.SubscribeToEvent(PopulateCharacterSheet, this);
        }

        private void Populate(Entity companion)
        {
            //todo set portrait
            _name.text = companion.Name;
            _raceClass.text = $"{companion._race}  {companion._entityClass}";

            var stats = companion.Stats;

            _health.text = $"{stats.CurrentHealth}/{stats.MaxHealth}";
            _energy.text = $"{stats.CurrentEnergy}/{stats.MaxEnergy}";
            _morale.text = $"{stats.CurrentMorale}/{stats.MaxMorale}";
            _attack.text = stats.Attack.ToString();

            var skills = companion.Skills;

            _dodge.text = skills.Dodge.ToString();
            _lockpick.text = skills.Lockpicking.ToString();
            _toughness.text = skills.Toughness.ToString();
            _healing.text = skills.Healing.ToString();
            _survival.text = skills.Survival.ToString();
            _persuasion.text = skills.Persuasion.ToString();

            var attributes = companion.Attributes;

            _agility.text = attributes.Agility.ToString();
            _coordination.text = attributes.Coordination.ToString();
            _physique.text = attributes.Physique.ToString();
            _intellect.text = attributes.Intellect.ToString();
            _acumen.text = attributes.Acumen.ToString();
            _charisma.text = attributes.Charisma.ToString();
        }


        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopulateCharacterSheet))
            {
                if(!(parameter is Entity companion))
                {
                    return;
                }

                Populate(companion);
            }
        }
    }
}

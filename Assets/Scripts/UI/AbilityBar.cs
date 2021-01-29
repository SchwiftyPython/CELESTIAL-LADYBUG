using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AbilityBar : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;

        private static Dictionary<string, Sprite> _abilityIcons;

        private Entity _activeEntity;

        #region AbilityIcons

        public Sprite SlashIcon;

        #endregion

        public Transform AbilityButtonParent;

        public GameObject AbilityButtonPrefab;

        private void Start()
        {
            _abilityIcons = new Dictionary<string, Sprite>
            {
                {"slash", SlashIcon}
            };

            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
        }

        public static void AssignAbilityToButton(Ability ability, GameObject buttonParent)
        {
            var buttonScript = buttonParent.GetComponent<Button>().GetComponent<UseAbilityButton>();

            var icon = GetIconForAbility(ability);

            buttonScript.AssignAbility(ability, icon);
        }

        public static void AssignAbilityToButton(Ability ability, Button button)
        {
            var buttonScript = button.GetComponent<UseAbilityButton>();

            var icon = GetIconForAbility(ability);

            buttonScript.AssignAbility(ability, icon);
        }

        private void Populate(Entity activeEntity)
        {
            _activeEntity = activeEntity;

            GlobalHelper.DestroyAllChildren(AbilityButtonParent.gameObject);

            //todo determine abilities from the entity
            //todo for each ability populate bar with an ability button prefab

            var testSlashAbility = new Ability("slash", 3, 1);

            var testInstance = Instantiate(AbilityButtonPrefab, new Vector3(0, 0), Quaternion.identity);

            AssignAbilityToButton(testSlashAbility, testInstance);

            testInstance.transform.SetParent(AbilityButtonParent);

            //todo check if there are valid targets and if there is enough ap for ability
            //todo make interactable if both are true 

        }

        private void Refresh()
        {
            //todo loop through all abilities in bar and determine if interactable
        }

        private bool AbilityIsUsable(Ability ability)
        {
            if (_activeEntity.Stats.CurrentActionPoints >= ability.ApCost)
            {
                return true;
            }

            //todo for each valid target see if any are in range

            var allEntities = CombatManager.Instance.TurnOrder.ToList();

            foreach (var entity in allEntities)
            {
                //todo use Chebyshev distance see gorogue article
            }

            //todo return true if any conditions met otherwise false
        }

        private static Sprite GetIconForAbility(Ability ability)
        {
            if (ability == null)
            {
                return null;
            }

            return !_abilityIcons.ContainsKey(ability.Name) ? null : _abilityIcons[ability.Name];
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                if (!(parameter is Entity activeEntity))
                {
                    return;
                }

                if (_activeEntity == null || _activeEntity != activeEntity)
                {
                    Populate(activeEntity);
                }
                else
                {
                    Refresh();
                }
            }
        }
    }
}

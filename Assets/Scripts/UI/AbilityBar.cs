using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
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

            var testSlashAbility = new Ability("slash", 3, 1, _activeEntity);

            var testInstance = Instantiate(AbilityButtonPrefab, new Vector3(0, 0), Quaternion.identity);

            AssignAbilityToButton(testSlashAbility, testInstance);

            testInstance.transform.SetParent(AbilityButtonParent);

            var buttonScript = testInstance.GetComponent<Button>().GetComponent<UseAbilityButton>();

            if (AbilityIsUsable(testSlashAbility))
            {
                buttonScript.EnableButton();
            }
            else
            {
                buttonScript.DisableButton();
            }

        }

        private void Refresh()
        {
            for (var i = 0; i < AbilityButtonParent.transform.childCount; i++)
            {
                var buttonScript = AbilityButtonParent.transform.GetChild(i).gameObject.GetComponent<Button>().GetComponent<UseAbilityButton>();

                var ability = buttonScript.Ability;

                if (_activeEntity.Stats.CurrentActionPoints >= ability.ApCost && AbilityIsUsable(ability))
                {
                    buttonScript.EnableButton();
                }
                else
                {
                    buttonScript.DisableButton();
                }
            }
        }

        private bool AbilityIsUsable(Ability ability)
        {
            if (_activeEntity.Stats.CurrentActionPoints < ability.ApCost)
            {
                return false;
            }

            var allEntities = CombatManager.Instance.TurnOrder.ToList();

            foreach (var entity in allEntities)
            {
                //todo need to determine if ability target type is hostile or friendly. -- assuming hostile here
                if (entity.IsPlayer() || entity.IsDerpus())
                {
                    continue;
                }

                var distance = Distance.CHEBYSHEV.Calculate(_activeEntity.Position, entity.Position);

                if (ability.Range >= distance)
                {
                    return true;
                }
            }

            return false;
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

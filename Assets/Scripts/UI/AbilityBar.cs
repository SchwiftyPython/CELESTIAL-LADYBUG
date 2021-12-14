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

        private Entity _activeEntity;

        public Transform AbilityButtonParent;

        public GameObject AbilityButtonPrefab;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(RefreshEvent, this);
        }

        private static void AssignAbilityToButton(Ability ability, GameObject buttonParent)
        {
            var buttonScript = buttonParent.GetComponent<Button>().GetComponent<UseAbilityButton>();

            var icon = GetIconForAbility(ability);

            if (icon == null)
            {
                return;
            }

            buttonScript.AssignAbility(ability, icon);
        }

        private void Populate(Entity activeEntity)
        {
            _activeEntity = activeEntity;

            GlobalHelper.DestroyAllChildren(AbilityButtonParent.gameObject);

            var abilities = activeEntity.Abilities;

            foreach (var ability in abilities.Values)
            {
                if (ability.IsPassive)
                {
                    continue;
                }

                var testInstance = Instantiate(AbilityButtonPrefab, new Vector3(0, 0), Quaternion.identity);

                AssignAbilityToButton(ability, testInstance);

                testInstance.transform.SetParent(AbilityButtonParent);

                testInstance.transform.localScale = new Vector3(1, 1, 1);

                var buttonScript = testInstance.GetComponent<Button>().GetComponent<UseAbilityButton>();

                if (AbilityIsUsable(ability))
                {
                    buttonScript.EnableButton();
                }
                else
                {
                    buttonScript.DisableButton();
                }
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

            var combatManager = FindObjectOfType<CombatManager>();

            var allEntities = combatManager.TurnOrder.ToList();

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
                    //todo going to have to check book slot too for spells
                    //return ability.Range <= 1 || ability.AbilityOwner.HasMissileWeaponEquipped();
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

            var spriteStore = FindObjectOfType<SpriteStore>();

            return spriteStore.GetAbilitySprite(ability);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                if (!(parameter is Entity activeEntity))
                {
                    return;
                }

                if (!activeEntity.IsPlayer())
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

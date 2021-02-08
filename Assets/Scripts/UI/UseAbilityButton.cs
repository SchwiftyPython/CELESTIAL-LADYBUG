using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UseAbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISubscriber
    {
        private Image _buttonIcon;
        private Button _button;

        private Queue<Entity> _targets;
        private Entity _selectedTarget;
        private Tile _highlightedTile;

        public Color HighlightedColor;

        public Ability Ability { get; private set; }

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(GlobalHelper.EndTurn, this);
            EventMediator.Instance.SubscribeToEvent(GlobalHelper.NextTarget, this);

            _buttonIcon = gameObject.GetComponent<Image>();
            _button = gameObject.GetComponent<Button>();

            _targets = new Queue<Entity>();
        }

        public void AssignAbility(Ability ability, Sprite icon)
        {
            if (ability == null)
            {
                return;
            }

            if (_button == null)
            {
                _button = gameObject.GetComponent<Button>();
            }

            if (_buttonIcon == null)
            {
                _buttonIcon = gameObject.GetComponent<Image>();
            }

            Ability = ability;

            EnableButton();

            SetIcon(icon);
        }

        public void EnableButton()
        {
            _button.interactable = true;
        }

        public void DisableButton()
        {
            _button.interactable = false; 
        }

        public void OnClick()
        {
            //todo targets thing would mean we'd want to select the target then an ability.
            //todo let's try select ability then select with mouse click
            //todo hit chance shows on hover and click
            //todo second click confirms action

            if (!_button.interactable)
            {
                return;
            }

            /*var allEntities = CombatManager.Instance.TurnOrder.ToList();

            var activeEntity = CombatManager.Instance.ActiveEntity;

            _targets = new Queue<Entity>();

            foreach (var entity in allEntities)
            {
                //todo need to determine if ability target type is hostile or friendly. -- assuming hostile here
                if (entity.IsPlayer() || entity.IsDerpus())
                {
                    continue;
                }

                var distance = Distance.CHEBYSHEV.Calculate(activeEntity.Position, entity.Position);

                if (Ability.Range >= distance)
                {
                    _targets.Enqueue(entity);
                }
            }*/

            EventMediator.Instance.Broadcast(GlobalHelper.HidePopup, this);

            CombatInputController.Instance.AbilityButtonClicked(Ability);
        }

        private void NextTarget()
        {
            if (_targets == null || _targets.Count < 2)
            {
                return;
            }

            var lastTarget = _targets.Dequeue();

            _targets.Enqueue(lastTarget);

            _selectedTarget = _targets.Peek();
        }

        private void SetIcon(Sprite icon)
        {
            _buttonIcon.sprite = icon;
        }

        private void HighlightTile(Tile tile)
        {
            if (tile == null)
            {
                return;
            }

            _highlightedTile = tile;
            tile.SpriteInstance.GetComponent<SpriteRenderer>().color = HighlightedColor;
        }

        private void ClearHighlight()
        {
            if (_highlightedTile == null)
            {
                return;
            }

            _highlightedTile.SpriteInstance.GetComponent<SpriteRenderer>().color = Color.white;

            _highlightedTile = null;
        }

        //todo this bugs out when the tootip appears under mouse. I took out any raycast target in the tooltip but no difference.
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventMediator.Instance.Broadcast(GlobalHelper.AbilityHovered, this, Ability);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventMediator.Instance.Broadcast(GlobalHelper.HidePopup, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.EndTurn))
            {
                ClearHighlight();
            }
            else if (eventName.Equals(GlobalHelper.NextTarget))
            {
                NextTarget();
            }
        }
    }
}

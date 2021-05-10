using System;
using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UI
{
    public class UseAbilityButton : MonoBehaviour, ISubscriber, IAbilityHolder
    {
        private Color EnabledColor = Color.white;
        private Color DisabledColor = new Color32((byte) 4F, (byte) 4F, (byte) 4F, byte.MaxValue);
        
        private Button _button;

        private Queue<Entity> _targets;
        private Entity _selectedTarget;
        private Tile _highlightedTile;

        public Color HighlightedColor;
        public GameObject IconImageParent;

        public Ability Ability { get; private set; }

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(GlobalHelper.EndTurn, this);

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

            Ability = ability;

            EnableButton();

            SetIcon(icon);
        }

        public void EnableButton()
        {
            _button.interactable = true;

            var iconImage = IconImageParent.GetComponent<Image>();

            iconImage.color = EnabledColor;
        }

        public void DisableButton()
        {
            _button.interactable = false;

            var iconImage = IconImageParent.GetComponent<Image>();

            iconImage.color = DisabledColor;
        }

        public void OnClick()
        {
            if (!_button.interactable)
            {
                return;
            }

            //eventMediator.Broadcast(GlobalHelper.HidePopup, this);

            var combatInputController = Object.FindObjectOfType<CombatInputController>();
            combatInputController.AbilityButtonClicked(Ability);
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
            var iconImage = IconImageParent.GetComponent<Image>();

            iconImage.sprite = icon;
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

        public Ability GetAbility()
        {
            return Ability;
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.EndTurn))
            {
                ClearHighlight();
            }
            // else if (eventName.Equals(GlobalHelper.NextTarget))
            // {
            //     NextTarget();
            // }
        }
    }
}

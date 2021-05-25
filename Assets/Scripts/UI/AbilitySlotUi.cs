﻿using Assets.Scripts.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AbilitySlotUi : MonoBehaviour, IAbilityHolder
    {
        private Ability _ability;

        public GameObject IconImageParent;

        public void SetAbility(Ability ability)
        {
            _ability = ability;

            var iconImage = IconImageParent.GetComponent<Image>();

            iconImage.sprite = ability.Icon;
        }

        public Ability GetAbility()
        {
            return _ability;
        }
    }
}
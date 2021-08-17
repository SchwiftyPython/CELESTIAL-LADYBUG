﻿using Assets.Scripts.Audio;
using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class PostCombatResultsPopup : MonoBehaviour
    {
        [SerializeField] private GameObject companionContainer;
        [SerializeField] private GameObject companionPrefab;
        [SerializeField] private GameObject popup;
        [SerializeField] private TextMeshProUGUI titleText;

        private CombatResult _result;

        private void Start()
        {
            Hide();
        }

        public void Show(CombatResult result)
        {
            Populate(result);

            popup.SetActive(true);
        }

        public void Leave()
        {
            if (_result == CombatResult.Defeat)
            {
                var eventMediator = FindObjectOfType<EventMediator>();

                eventMediator.Broadcast(GlobalHelper.GameOver, this);
            }
            else
            {
                var combatManager = FindObjectOfType<CombatManager>();

                var turnOrder = combatManager.TurnOrder;

                foreach (var combatant in turnOrder)
                {
                    combatant.ResetOneUseCombatAbilities();
                }

                if (!SceneManager.GetSceneByName(GlobalHelper.TravelScene).isLoaded)
                {
                    SceneManager.LoadScene(GlobalHelper.TravelScene);

                    var musicController = FindObjectOfType<MusicController>();

                    musicController.PlayTravelMusic();
                }
            }
        }

        private void Hide()
        {
            popup.SetActive(false);
        }

        private void Populate(CombatResult result)
        {
            _result = result;

            titleText.text = result.ToString();

            var combatManager = FindObjectOfType<CombatManager>();

            var allCompanions = combatManager.Companions;

            foreach (var companion in allCompanions)
            {
                var stats = Instantiate(companionPrefab, companionContainer.transform);

                stats.GetComponent<PostCombatCompanionStats>().Populate(companion);
            }
        }
    }
}
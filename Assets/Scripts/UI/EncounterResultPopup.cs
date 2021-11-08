using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Encounters;
using Assets.Scripts.Entities;
using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EncounterResultPopup : MonoBehaviour, ISubscriber
    {
        private const string NormalPopupEvent = GlobalHelper.EncounterResult;
        private const string CombatPreviewPopupEvent = GlobalHelper.ShowCombatPreview;
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string CampingEncounterFinished = GlobalHelper.CampingEncounterFinished;

        private TextWriter _textWriter;
        private EncounterType _encounterType;
        private bool _countsAsDayTraveled;
        private List<Entity> _enemies;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI ResultDescription;

        public GameObject OkayButton;
        public GameObject StartCombatButton;
        public GameObject ImageContainer;
        public Image Image;

        [FMODUnity.EventRef] public string popupSound;

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(NormalPopupEvent, this);
            eventMediator.SubscribeToEvent(CombatPreviewPopupEvent, this);
            eventMediator.SubscribeToEvent(GlobalHelper.WritingFinished, this);

            _textWriter = GetComponent<TextWriter>();

            GetComponent<Button_UI>().ClickFunc = _textWriter.DisplayMessageInstantly;

            Hide();
        }

        public void ShowCombatPreview(Encounter encounter, string result, List<Entity> enemies)
        {
            HideButtons();

            if (string.IsNullOrEmpty(encounter.ImageName))
            {
                ImageContainer.SetActive(false);
            }
            else
            {
                var spriteStore = FindObjectOfType<SpriteStore>();

                var image = spriteStore.GetEncounterSprite(encounter.ImageName); //todo get a combat preview image

                if (image == null)
                {
                    ImageContainer.SetActive(false);
                }
                else
                {
                    Image.sprite = image;
                    ImageContainer.SetActive(true);
                }
            }

            _encounterType = EncounterType.Combat;

            EncounterTitle.text = encounter.Title;

            _enemies = enemies;

            _textWriter.AddWriter(ResultDescription, result, GlobalHelper.DefaultTextSpeed, true);

            gameObject.SetActive(true);

            GameManager.Instance.AddActiveWindow(gameObject);

            var sound = FMODUnity.RuntimeManager.CreateInstance(popupSound);
            sound.start();
        }

        private void Show(Encounter encounter, List<string> result)
        {
            HideButtons();

            _encounterType = encounter.EncounterType;

            //todo we'll need to store result images in options for different outcomes

            if (string.IsNullOrEmpty(encounter.ImageResultName))
            {
                ImageContainer.SetActive(false);
            }
            else
            {
                var spriteStore = FindObjectOfType<SpriteStore>();

                var image = spriteStore.GetEncounterSprite(encounter.ImageResultName);

                if (image == null)
                {
                    ImageContainer.SetActive(false);
                }
                else
                {
                    Image.sprite = image;
                    ImageContainer.SetActive(true);
                }
            }

            if (_encounterType == EncounterType.Camping)
            {
                _countsAsDayTraveled = encounter.CountsAsDayTraveled;
            }

            EncounterTitle.text = encounter.Title;

            var resultText = string.Empty;
            foreach (var line in result)
            {
                resultText += '\n' + line;
            }

            _textWriter.AddWriter(ResultDescription, resultText, GlobalHelper.DefaultTextSpeed, true);

            gameObject.SetActive(true);

            GameManager.Instance.AddActiveWindow(gameObject);

            var sound = FMODUnity.RuntimeManager.CreateInstance(popupSound);
            sound.start();
        }

        private void ShowButtons()
        {
            if (_encounterType == EncounterType.Combat)
            {
                StartCombatButton.SetActive(true);
            }
            else
            {
                OkayButton.SetActive(true);
            }
        }

        private void HideButtons()
        {
            OkayButton.SetActive(false);
            StartCombatButton.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);

            var eventMediator = FindObjectOfType<EventMediator>();

            if (_encounterType == EncounterType.Camping)
            {
                eventMediator.Broadcast(CampingEncounterFinished, this, _countsAsDayTraveled);
            }

            eventMediator.Broadcast(EncounterFinished, this);
        }

        public void LoadCombatScene()
        {
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);

            SceneManager.LoadScene(GlobalHelper.CombatScene);

            var combatManager = FindObjectOfType<CombatManager>();

            combatManager.Enemies = _enemies;

            combatManager.LoadCombatScene();
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (eventMediator == null)
            {
                return;
            }

            eventMediator.UnsubscribeFromEvent(NormalPopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(NormalPopupEvent))
            {
                var encounter = broadcaster as Encounter;

                if (encounter == null)
                {
                    return;
                }

                var result = parameter as List<string>;

                if (result == null || result.Count < 1)
                {
                    return;
                }

                Show(encounter, result);
            }
            else if (eventName.Equals(CombatPreviewPopupEvent))
            {
                var encounter = broadcaster as Encounter;

                if (encounter == null)
                {
                    return;
                }

                var option = parameter as FightCombatOption;

                if (option == null || option.Enemies == null || option.Enemies.Count < 1)
                {
                    return;
                }

                ShowCombatPreview(encounter, option.ResultText, option.Enemies);
            }
            else if (eventName.Equals(GlobalHelper.WritingFinished))
            {
                ShowButtons();
            }
        }
    }
}

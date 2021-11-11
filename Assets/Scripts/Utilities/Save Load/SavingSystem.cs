using System;
using System.Globalization;
using Assets.Scripts.Audio;
using Assets.Scripts.Combat;
using Assets.Scripts.Encounters;
using Assets.Scripts.Travel;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Save_Load
{
    public class SavingSystem : MonoBehaviour
    {
        private const string SettingsFile = "settings.txt";

        private string _autoSaveFile;

        public void AutoSave()
        {
            Save(_autoSaveFile);
        }

        public void Save(string saveFile)
        {
            _autoSaveFile = saveFile;

            var gameManager = FindObjectOfType<GameManager>();

            ES3.Save("game manager", gameManager.CaptureState(), saveFile);

            if (gameManager.InCombat())
            {
                var combatManager = FindObjectOfType<CombatManager>();

                ES3.Save("combat manager", combatManager.CaptureState(), saveFile);
            }

            var travelManager = FindObjectOfType<TravelManager>();

            ES3.Save("travel manager", travelManager.CaptureState(), saveFile);

            ES3.Save("day info", $"DAY {travelManager.CurrentDayOfTravel}, {travelManager.Party.Size} Companions", saveFile);
            ES3.Save("datetime info", DateTime.Now.ToString(CultureInfo.CurrentCulture), saveFile);

            var encounterManager = FindObjectOfType<EncounterManager>();

            ES3.Save("encounter manager", encounterManager.CaptureState(), saveFile);
        }

        public void Load(string saveFile)
        {
            _autoSaveFile = saveFile;

            var gameManager = FindObjectOfType<GameManager>();

            var gmDto = (GameManager.GameManagerDto)ES3.Load("game manager", saveFile);

            var travelManager = FindObjectOfType<TravelManager>();

            var tmDto = ES3.Load("travel manager", saveFile);

            travelManager.RestoreState(tmDto);

            var encounterManager = FindObjectOfType<EncounterManager>();

            var emDto = ES3.Load("encounter manager", saveFile);

            encounterManager.RestoreState(emDto);

            if (string.Equals(gmDto.CurrentSceneName, GameManager.CombatSceneName, StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(SceneManager.GetActiveScene().name, GameManager.CombatSceneName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    LoadCombatManager();
                }
                else
                {
                    SceneManager.sceneLoaded += LoadCombatManager;
                }
            }

            gameManager.RestoreState(gmDto);
        }

        public void SaveSettings()
        {
            var settings = FindObjectOfType<SettingsWindow>();

            ES3.Save("settings", settings.CaptureState(), SettingsFile);
        }

        public void LoadSettings()
        {
            var settings = FindObjectOfType<SettingsWindow>();

            if (!ES3.FileExists(SettingsFile))
            {
                return;
            }

            var settingsDto = ES3.Load("settings", SettingsFile);

            settings.RestoreState(settingsDto);
        }

        public SaveSlot.SaveGameInfo? GetSaveGameInfo(string fileName)
        {
            if (!SaveExists(fileName))
            {
                return null;
            }

            var saveGameInfo = new SaveSlot.SaveGameInfo();

            saveGameInfo.DayInfo = ES3.Load("day info", fileName).ToString();
            saveGameInfo.DateTimeInfo = ES3.Load("datetime info", fileName).ToString();

            return saveGameInfo;
        }

        public bool SaveExists(string fileName)
        {
            return ES3.FileExists(fileName);
        }

        public void Delete(string saveFile)
        {
            ES3.DeleteFile(saveFile);
        }

        public void DeleteCurrentSave()
        {
            ES3.DeleteFile(_autoSaveFile);
        }

        private void LoadCombatManager(Scene arg0, LoadSceneMode loadSceneMode)
        {
            LoadCombatManager();
        }

        private void LoadCombatManager()
        {
            var music = FindObjectOfType<MusicController>();

            music.EndTravelMusic();

            var combatManager = FindObjectOfType<CombatManager>();

            var cmDto = ES3.Load("combat manager", _autoSaveFile);

            combatManager.RestoreState(cmDto);

            combatManager.LoadFromSave();

            SceneManager.sceneLoaded -= LoadCombatManager;
        }

        private void OnApplicationQuit()
        {
            if (string.Equals(GameManager.CurrentScene.name, "TitleScreen", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var combatTester = FindObjectOfType<CombatTester>();

            if (combatTester != null && combatTester.testingEnabled)
            {
                return;
            }

            AutoSave();
        }
    }
}
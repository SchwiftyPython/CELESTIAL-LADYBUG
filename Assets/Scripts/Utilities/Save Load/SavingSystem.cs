using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Combat;
using Assets.Scripts.Encounters;
using Assets.Scripts.Travel;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Save_Load
{
    public class SavingSystem : MonoBehaviour, ISubscriber
    {
        private const string CombatSceneLoaded = GlobalHelper.CombatSceneLoaded;

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            var gameManager = FindObjectOfType<GameManager>();

            ES3.Save("game manager", gameManager.CaptureState(), saveFile);

            if (gameManager.InCombat())
            {
                var combatManager = FindObjectOfType<CombatManager>();
                
                ES3.Save("combat manager", combatManager.CaptureState(), saveFile);
            }
            
            var travelManager = FindObjectOfType<TravelManager>();
            
            ES3.Save("travel manager", travelManager.CaptureState(), saveFile);

            var encounterManager = FindObjectOfType<EncounterManager>();

            ES3.Save("encounter manager", encounterManager.CaptureState(), saveFile);
        }

        public void Load(string saveFile)
        {
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
                SceneManager.sceneLoaded += LoadCombatManager;
            }

            gameManager.RestoreState(gmDto);
        }

        public SaveSlot.SaveGameInfo? GetSaveGameInfo(string fileName)
        {
            if (!ES3.FileExists($"{fileName}.es3"))
            {
                return null;
            }

            var saveGameInfo = new SaveSlot.SaveGameInfo();

            saveGameInfo.DayInfo = (string)ES3.Load("day info", fileName);
            saveGameInfo.DateTimeInfo = (string)ES3.Load("datetime info", fileName);

            return saveGameInfo;
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            //todo replace with es3

            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFile, object state)
        {
            //todo replace with es3

            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        private void LoadCombatManager(Scene arg0, LoadSceneMode loadSceneMode)
        {
            var combatManager = FindObjectOfType<CombatManager>();

            var cmDto = ES3.Load("combat manager");

            combatManager.RestoreState(cmDto);

            combatManager.LoadFromSave();

            SceneManager.sceneLoaded -= LoadCombatManager;
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (string.Equals(eventName, CombatSceneLoaded, StringComparison.OrdinalIgnoreCase))
            {
                var combatManager = FindObjectOfType<CombatManager>();

                var cmDto = ES3.Load("combat manager");

                combatManager.RestoreState(cmDto);

                combatManager.LoadFromSave();

                var eventMediator = FindObjectOfType<EventMediator>();

                eventMediator.UnsubscribeFromEvent(CombatSceneLoaded, this);
            }
        }
    }
}
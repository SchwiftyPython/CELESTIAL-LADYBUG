using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Combat;
using Assets.Scripts.Encounters;
using Assets.Scripts.Travel;
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
            //todo set save filename

            var gameManager = FindObjectOfType<GameManager>();

            ES3.Save("game manager", gameManager.CaptureState());

            if (gameManager.InCombat())
            {
                var combatManager = FindObjectOfType<CombatManager>();
                
                ES3.Save("combat manager", combatManager.CaptureState());
            }
            
            var travelManager = FindObjectOfType<TravelManager>();
            
            ES3.Save("travel manager", travelManager.CaptureState());

            var encounterManager = FindObjectOfType<EncounterManager>();

            ES3.Save("encounter manager", encounterManager.CaptureState());

        }

        public void Load(string saveFile)
        {
            //todo load save file

            var gameManager = FindObjectOfType<GameManager>();

            var gmDto = (GameManager.GameManagerDto)ES3.Load("game manager");

            if (string.Equals(gmDto.CurrentSceneName, GameManager.CombatSceneName, StringComparison.OrdinalIgnoreCase))
            {
                var eventMediator = FindObjectOfType<EventMediator>();

                eventMediator.SubscribeToEvent(CombatSceneLoaded, this);

                // var combatManager = FindObjectOfType<CombatManager>();
                //
                // var cmDto = ES3.Load("combat manager");
                //
                // combatManager.RestoreState(cmDto);
            }

            gameManager.RestoreState(gmDto);

            var travelManager = FindObjectOfType<TravelManager>();

            var tmDto = ES3.Load("travel manager");

            travelManager.RestoreState(tmDto);

            var encounterManager = FindObjectOfType<EncounterManager>();

            var emDto = ES3.Load("encounter manager");

            encounterManager.RestoreState(emDto);
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

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (string.Equals(eventName, CombatSceneLoaded, StringComparison.OrdinalIgnoreCase))
            {
                var combatManager = FindObjectOfType<CombatManager>();

                var cmDto = ES3.Load("combat manager");

                combatManager.RestoreState(cmDto);

                var eventMediator = FindObjectOfType<EventMediator>();

                eventMediator.UnsubscribeFromEvent(CombatSceneLoaded, this);
            }
        }
    }
}
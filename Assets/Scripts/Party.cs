using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class Party
    {
        private const int MaxSize = 10;
        private const int StartSize = 6;

        private Dictionary<string, Entity> _companions;
        public  Entity Derpus { get; set; }

        //todo need enums for these
        public int Food { get; set; }
        public int HealthPotions { get; set; }
        public int Gold { get; set; }

        public Party()
        {
            GenerateStartingParty();

            Food = 50;
            HealthPotions = 40;
            Gold = 550;
        }

        //todo refactor
        public void AddCompanion(Entity companion)
        {
            if (_companions == null)
            {
                _companions = new Dictionary<string, Entity>();
            }

            if (_companions.Count >= MaxSize)
            {
                return;
            }

            if (_companions.ContainsKey(companion.Name))
            {
                companion.Name += ", Jr";

                if (_companions.ContainsKey(companion.Name))
                {
                    //todo just generate a different name at this point or something
                    Debug.Log($"{companion.Name} already exists in party!");
                    return;
                }
            }

            _companions.Add(companion.Name, companion);
        }

        public void RemoveCompanion(Entity companion)
        {
            if (_companions == null)
            {
                return;
            }

            if (_companions.Count <= 0 || !_companions.ContainsKey(companion.Name))
            {
                return;
            }

            _companions.Remove(companion.Name);
        }

        public Entity GetCompanion(string companionName)
        {
            if (!_companions.ContainsKey(companionName))
            {
                return null;
            }

            return _companions[companionName];
        }

        public void Eat()
        {
            if (_companions == null)
            {
                return;
            }

            //todo choose random order to feed companions. This allows for some to eat and not others when food is low. Then subtract morale for the hungry.

            if (Food < _companions.Count)
            {
                Food = 0;

                foreach (var companion in _companions.Values)
                {
                    companion.SubtractMorale(25);
                }

                Debug.Log("Not enough food! Party morale drops!"); //todo message
            }
            else
            {
                Food -= _companions.Count;

                Debug.Log($"Party ate {_companions.Count} food!"); //todo message
            }

            //todo food amount changed event to update party status bar
        }

        public void Heal()
        {
            if (_companions == null)
            {
                return;
            }

            if (HealthPotions <= 0)
            {
                Debug.Log("No health potions! Can't heal!");
            }

            foreach (var companion in _companions.Values)
            {
                if (HealthPotions <= 0)
                {
                    return;
                }

                if (companion.Stats.CurrentHealth < companion.Stats.MaxHealth)
                {
                    companion.UseHealthPotion();

                    HealthPotions--;
                }
            }
        }

        public void Rest()
        {
            Derpus.Rest();

            if (_companions == null)
            {
                return;
            }

            foreach (var companion in _companions.Values)
            {
                companion.Rest();
            }
        }

        private void GenerateStartingParty()
        {
            Derpus = new Entity(true);

            _companions = new Dictionary<string, Entity>();

            for (var i = 0; i < StartSize; i++)
            {
                var companion = new Entity();

                AddCompanion(companion);
            }
        }
    }
}

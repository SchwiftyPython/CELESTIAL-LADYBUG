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

        private int _food;
        private int _healthPotions;
        private int _gold;

        public Party()
        {
            GenerateStartingParty();

            _food = 50;
            _healthPotions = 40;
            _gold = 550;
        }

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

        public void Eat()
        {
            if (_companions == null)
            {
                return;
            }

            //todo choose random order to feed companions. This allows for some to eat and not others when food is low. Then subtract morale for the hungry.

            if (_food < _companions.Count)
            {
                _food = 0;

                foreach (var companion in _companions.Values)
                {
                    companion.SubtractMorale(25);
                }

                Debug.Log("Not enough food! Party morale drops!"); //todo message
            }
            else
            {
                _food -= _companions.Count;

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

            if (_healthPotions <= 0)
            {
                Debug.Log("No health potions! Can't heal!");
            }

            foreach (var companion in _companions.Values)
            {
                if (_healthPotions <= 0)
                {
                    return;
                }

                if (companion.Stats.CurrentHealth < companion.Stats.MaxHealth)
                {
                    companion.UseHealthPotion();

                    _healthPotions--;
                }
            }
        }

        public void Rest()
        {
            _derpus.Rest();

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
            _derpus = new Entity(true);

            _companions = new Dictionary<string, Entity>();

            for (var i = 0; i < StartSize; i++)
            {
                var companion = new Entity();

                AddCompanion(companion);
            }
        }
    }
}

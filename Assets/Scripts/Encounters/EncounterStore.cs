using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Encounters.Camping;
using Assets.Scripts.Encounters.Combat;
using Assets.Scripts.Encounters.DerpusStopWagon;
using Assets.Scripts.Encounters.MentalBreak;
using Assets.Scripts.Encounters.Normal;
using Assets.Scripts.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Encounters
{
    public class EncounterStore : MonoBehaviour
    {
        private readonly Dictionary<string, Func<Encounter>> _normalEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"stay in school", () => new StayInSchool()},
            {"fight or flight", () => new FightOrFlight()},
            {"genie in a bottle", () => new GenieInABottle()},
            {"sweetroll robbery", () => new SweetrollRobbery()},
            {"disabled wagon", () => new DisabledWagon()},
            {"nasty storm", () => new NastyStorm()},
            {"rock skipping contest", () => new RockSkippingContest()},
            {"cauldron roulette", () => new CauldronRoulette()},
            {"squeaky wheel", () => new SqueakyWheel()},
            {"potion master", () => new PotionMaster()},
            {"creepy shop", () => new CreepyShop()},
            {"crustacean laceration", () => new CrustaceanLaceration()},
            {"how rude", () => new HowRude()},
            {"extreme food challenge", () => new ExtremeFoodChallenge()},
            {"positively disgusting", () => new PositivelyDisgusting()}
        };

        private readonly Dictionary<string, Func<Encounter>> _campingEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"camp mosquito", () => new CampMosquito()},
            {"comfy inn", () => new ComfyInn()},
            {"holy inferno", () => new HolyInferno()},
            {"star blanket", () => new StarBlanket()},
            {"conk out", () => new ConkOut()},
            {"peaceful village", () => new PeacefulVillage()},
            {"arachnid archery", () => new ArachnidArchery()},
            {"lending a hand", () => new LendingAHand()},
        };

        private readonly Dictionary<string, Func<Encounter>> _combatEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"bandit attack", () => new BanditAttack()},
            {"looting bandits", () => new LootingBandits()},
            {"trail of the dead", () => new TrailOfTheDead()},
        };

        private readonly Dictionary<string, Func<Entity, Encounter>> _mentalBreakEncounters = new Dictionary<string, Func<Entity, Encounter>>
        {
            {"dazed mugging", dazedCompanion => new DazedMugging(dazedCompanion)}
        };

        private readonly Dictionary<string, Func<Encounter>> _derpusStopWagonEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"no energy", () => new DerpusNoEnergy()},
            {"no morale", () => new DerpusNoMorale()}
        };

        private readonly Dictionary<string, Func<Encounter>> _testEncounters = new Dictionary<string, Func<Encounter>>
        {
            
        };

        public List<Encounter> GetNormalEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _normalEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public List<Encounter> GetCampingEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _campingEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public List<Encounter> GetCombatEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _combatEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public Encounter GetRandomMentalBreakEncounter(Entity companion)
        {
            var index = Random.Range(0, _mentalBreakEncounters.Count);

            var key = _mentalBreakEncounters.ElementAt(index).Key;

            return _mentalBreakEncounters[key].Invoke(companion);
        }

        public Encounter GetDerpusNoEnergyEncounter()
        {
            return _derpusStopWagonEncounters["no energy"].Invoke();
        }

        public Encounter GetDerpusNoMoraleEncounter()
        {
            return _derpusStopWagonEncounters["no morale"].Invoke();
        }

        public List<Encounter> GetTestEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _testEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }
    }
}

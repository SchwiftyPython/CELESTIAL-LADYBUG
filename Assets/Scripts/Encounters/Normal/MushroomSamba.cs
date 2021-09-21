using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class MushroomSamba : Encounter
    {
        public MushroomSamba()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Mushroom Samba";
        }

        public override void Run()
        {
            var forager = Party.GetCompanionWithHighestSurvivalSkill();

            Description = $"{forager.FirstName()} gets bored and wanders off the trail a bit. They come back with an armful of mushrooms.\n\n\"Of course they're safe! See?\"\n\nThey pop one in their mouth and swallow.";

            const int success = 15;

            var survivalRoll = Dice.Roll($"{forager.Skills.Survival - 1}d6");

            var wildRoll = GlobalHelper.RollWildDie();

            survivalRoll += wildRoll;

            if (survivalRoll > success)
            {
                Description += $"A short while passes on the trail and {forager.FirstName()} hasn't keeled over so they must be okay!";

                Reward = new Reward();

                Reward.AddPartyGain(PartySupplyTypes.Food, 6);
            }
            else
            {
                Description += $"A short while passes on the trail and {forager.FirstName()} gets horribly sick! Derpus chucks the mushrooms as far as he can.";

                Penalty = new Penalty();

                Penalty.AddEntityLoss(forager, EntityStatTypes.CurrentHealth, 5);
                Penalty.AddEntityLoss(forager, EntityStatTypes.CurrentMorale, 5);
            }

            var travelManager = Object.FindObjectOfType<TravelManager>();

            if (Reward != null)
            {
                travelManager.ApplyEncounterReward(Reward);
            }

            if (Penalty != null)
            {
                travelManager.ApplyEncounterPenalty(Penalty);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}

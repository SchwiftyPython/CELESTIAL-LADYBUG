using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class DisabledWagon : Encounter
    {
        public DisabledWagon()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Disabled Wagon";
            Description = "A broken down wagon appears on the trail. A man is sitting beside it, dirty and frustrated.";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var optionTitle = "Help fix wagon";
            string optionResultText;

            Reward optionReward = null;
            Penalty optionPenalty = null;
            EncounterType targetEncounterType;

            const int fixChance = 51;

            //todo diceroller
            var roll = Random.Range(1, 101);

            Debug.Log($"Fix Value Needed: {fixChance}");
            Debug.Log($"Rolled: {roll}");

            const int potionChance = 50;
            const int goldChance = 30;
            if (roll < fixChance)
            {
                targetEncounterType = EncounterType;

                optionResultText = "No one here is a wagon expert, but the group puts their heads together and manages to get the wagon repaired enough to get into the next town! The man gives some supplies as a reward!";

                optionReward = new Reward();

                optionReward.AddPartyGain(PartySupplyTypes.Food, 10);

                //todo diceroller
                roll = Random.Range(1, 101);

                Debug.Log($"Potion Value Needed: {potionChance}");
                Debug.Log($"Rolled: {roll}");

                if (roll <= potionChance)
                {
                    optionReward.AddPartyGain(PartySupplyTypes.HealthPotions, 3);
                }

                //todo diceroller
                roll = Random.Range(1, 101);

                Debug.Log($"Potion Value Needed: {goldChance}");
                Debug.Log($"Rolled: {roll}");

                if (roll <= goldChance)
                {
                    optionReward.AddPartyGain(PartySupplyTypes.Gold, 40);
                }
            }
            else if (roll == fixChance)
            {
                targetEncounterType = EncounterType.Camping;

                optionResultText = "Try as they might, no one can figure out how to fix the wagon. It's sundown by the time the group calls it quits so they make camp.";
            }
            else
            {
                targetEncounterType = EncounterType.Camping;

                optionResultText = "No one has a clue how to help and they spend most of the day arguing. It's sundown by the time the group calls it quits so they make camp.";

                optionPenalty = new Penalty();

                var travelManager = Object.FindObjectOfType<TravelManager>();

                optionPenalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    optionPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentMorale, 10);
                }
            }

            var optionOne = new Option(optionTitle, optionResultText, optionReward, optionPenalty, targetEncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Rob him";
            optionResultText = "Unarmed and outnumbered, the man gives up his supplies and sits helpless by the wagon as the group vanishes over the horizon.";

            //todo entity penalty for companions with "good guy" traits
            //todo entity reward for companions with "bad guy" traits

            optionReward = new Reward();

            optionReward.AddPartyGain(PartySupplyTypes.Food, 7);

            //todo diceroller
            roll = Random.Range(1, 101);

            Debug.Log($"Potion Value Needed: {potionChance}");
            Debug.Log($"Rolled: {roll}");

            if (roll <= potionChance)
            {
                optionReward.AddPartyGain(PartySupplyTypes.HealthPotions, 9);
            }

            //todo diceroller
            roll = Random.Range(1, 101);

            Debug.Log($"Potion Value Needed: {goldChance}");
            Debug.Log($"Rolled: {roll}");

            if (roll <= goldChance)
            {
                optionReward.AddPartyGain(PartySupplyTypes.Gold, 40);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            optionTitle = "Pass'm by";

            var optionThree = new Option(optionTitle, "The man sits helpless by the wagon as the group vanishes over the horizon.", EncounterType.Normal);

            Options.Add(optionTitle, optionThree);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

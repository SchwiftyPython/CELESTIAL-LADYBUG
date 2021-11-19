using System.Collections.Generic;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class UnleashPower : Encounter
    {
        private readonly List<string> _companyNames = new List<string>
        {
            "Gnome Depot",
            "Bards and Nobles",
            "Bloodbath & Beyond",
            "Dungeon Donuts"
        };

        public UnleashPower()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Unleash the Power Within!";
        }

        public override void Run()
        {
            var companyName = _companyNames[Random.Range(0, _companyNames.Count)];

            Description =
                $"The party spots a large gathering and decides to check it out.\n\n'WELCOME TO THE {companyName.ToUpper()} COMPANY RETREAT!' a huge banner exclaims.\n\nYou are about to tell everyone break's over when an ENTHUSIASTIC gentleman with a ponytail challenges your party to 'unleash their inner power'. His arm is extended towards a bed of hot coals surrounded by totally thrilled employees.\n\nWho will take the challenge?";

            Options = new Dictionary<string, Option>();

            var sacrifices = Party.GetRandomCompanions(3);

            const int success = 15;

            string optionResultText;
            foreach (var sacrifice in sacrifices)
            {
                var optionReward = new Reward();
                var optionPenalty = new Penalty();

                var coordCheck = Dice.Roll($"{sacrifice.Attributes.Coordination - 1}d6");

                var wildRoll = GlobalHelper.RollWildDie();

                coordCheck += wildRoll;

                if (coordCheck > success)
                {
                    optionResultText = $"{sacrifice.FirstName()} marches across the coals. They indeed feel a burning sensation -- not on their feet, but within. Just like the pamphlet said!";

                    optionReward.AddEntityGain(sacrifice, EntityStatTypes.CurrentMorale, 10);
                    optionReward.AddEntityGain(sacrifice, EntitySkillTypes.Endurance, 1);
                }
                else
                {
                    optionResultText = $"{sacrifice.FirstName()} tries walking on the sides of their feet to keep from burning their soles, but ends up slipping and slams onto the hot coals!";

                    optionPenalty.AddEntityLoss(sacrifice, EntityStatTypes.CurrentMorale, 5);
                    optionPenalty.AddEntityLoss(sacrifice, EntityStatTypes.CurrentHealth, 5);
                }

                var option = new Option(sacrifice.Name, optionResultText, optionReward, optionPenalty, EncounterType.Normal);

                Options.Add(sacrifice.Name, option);
            }

            var optionTitle = "Get out of there";

            optionResultText = $"The man whips his ponytail round and round in a rage!\n\n\"You'll never know true enlightenment!\"";

            var optionFour = new Option(optionTitle, optionResultText, null, null, EncounterType.Normal);

            Options.Add(optionTitle, optionFour);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

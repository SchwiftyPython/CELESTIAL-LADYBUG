using System.CodeDom;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class DevilsLunch : Encounter
    {
        public DevilsLunch()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Devil's Lunch";
            Description =
                "The wagon stops for lunch. Everyone is just chilling when the smell of brimstone and candy fills the air.\n\n\"Who's making s'mores?\"\n\nSuddenly, blinding light envelopes the group. The light fades and the demon Seva is now menacingly standing in front of you. Everything is still until Seva erupts into a coughing fit!\n\n\"Got any cough syrup?\"";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            string optionTitle;
            string optionResultText;

            var sacrifice = Party.GetRandomCompanion();
            if (Party.HealthPotions > 0)
            {
                optionTitle = "Offer them a health potion";

                optionResultText = "Not quite cough syrup, but close enough!\n\n";

                const int sacChance = 13;

                var roll = Dice.Roll("1d100");

                var optionOnePenalty = new Penalty();

                optionOnePenalty.AddPartyLoss(PartySupplyTypes.HealthPotions, 1);

                var optionOneReward = new Reward();

                if (roll <= sacChance)
                {
                    optionResultText += "\"Still gonna take one of you as a sacrifice though.\"\n\n";

                    optionResultText += $"{sacrifice.Name} vanishes in a flash of fire and black smoke! Seva shrugs.\n\n\"Okay, see ya.\"";

                    optionOnePenalty.RemoveFromParty(sacrifice);

                    optionOnePenalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 5);

                    optionOneReward.EveryoneGain(Party, EntityAttributeTypes.Physique, 1);
                    optionOneReward.EveryoneGain(Party, EntitySkillTypes.Endurance, 1);
                }
                else
                {
                    var chosen = Party.GetRandomCompanion();

                    optionResultText += $"Seva points at {chosen.FirstName()}.\n\n\"You seem cool. Here.\"\n\n{chosen.FirstName()} glows red briefly and Seva vanishes.";

                    optionOneReward.AddEntityGain(chosen, EntityAttributeTypes.Physique, 1);
                    optionOneReward.AddEntityGain(chosen, EntitySkillTypes.Endurance, 1);

                    //todo maybe change their skin color to red idk
                }

                var optionOne = new Option(optionTitle, optionResultText, optionOneReward, optionOnePenalty,
                    EncounterType.Normal);

                Options.Add(optionTitle, optionOne);
            }

            optionTitle = "PANIC!";

            optionResultText = $"Everyone screams and scatters in all directions! Seva alternates between screaming and coughing before pointing at {sacrifice.FirstName()}. {sacrifice.FirstName()} stops in their tracks, glows red briefly, then vanishes in a flash of fire and black smoke! Seva shrugs.\n\n\"Okay, see ya.\"";

            var optionTwoPenalty = new Penalty();

            optionTwoPenalty.RemoveFromParty(sacrifice);

            var optionTwo = new Option(optionTitle, optionResultText, null, optionTwoPenalty, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class PositivelyDisgusting : Encounter
    {
        public PositivelyDisgusting()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Positively Disgusting";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var chosenCompanions = travelManager.Party.GetRandomCompanions(3);

            Description = $"While doing some spring cleaning in the wagon, {chosenCompanions.First().FirstName()} finds an old chest completely covered with some kind of pale green slime. It's super gross and there doesn't seem to be a way to get around touching the slime to open it!";

            Options = new Dictionary<string, Option>();

            string optionResult;
            foreach (var companion in chosenCompanions)
            {
                Reward optionReward = new Reward();

                optionReward.AddPartyGain(PartySupplyTypes.Food, 15);

                Penalty optionPenalty = null;

                if (companion.EntityClass == EntityClass.Wizard)
                {
                    optionResult =
                        $"{companion.FirstName()} waggles their fingers and opens the chest without touching the goo. There's a bunch of supplies inside!";
                }
                else
                {
                    optionResult =
                        $"{companion.FirstName()} tries to use a cloth to open the chest, but it goes horribly wrong! They are super disgusted! They'll never get over this!";

                    optionPenalty = new Penalty();

                    optionPenalty.AddEntityLoss(companion, EntityStatTypes.MaxMorale, 10);
                }

                var option = new Option(companion.Name, optionResult, optionReward, optionPenalty,
                    EncounterType.Normal);

                Options.Add(companion.Name, option);
            }

            var optionTitle = "Leave it";

            optionResult = "Everyone decides it's best to just pretend it's not there and move on with their lives.";

            var ignoreOption = new Option(optionTitle, optionResult, null, null, EncounterType.Normal);

            Options.Add(optionTitle, ignoreOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

﻿using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters
{
    public class SweetrollRobbery : Encounter
    {
        public SweetrollRobbery()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Sweetroll Robbery";
            Description = "The group finds an overturned wagon on the side of the trail. It's unclear what happened here, but the group manages to find some baked goods!";

            Reward = new Reward();

            Reward.AddPartyGain(PartySupplyTypes.Food, 8);

            //todo need a method for giving entire party the same reward or penalty
            Reward.AddEntityGain(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                Reward.AddEntityGain(companion, EntityStatTypes.CurrentMorale, 10);
            }
        }

        public override void Run()
        {
            var fullResultDescription = new List<string>();

            fullResultDescription.Add(Description + "\n");

            var rewardsText = TravelManager.Instance.ApplyEncounterReward(Reward);

            fullResultDescription.AddRange(rewardsText);

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
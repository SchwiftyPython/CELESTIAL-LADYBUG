﻿using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class RockSkippingContest : Encounter
    {
        private const int BetAmount = 60;

        public RockSkippingContest()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Rock Skipping Contest";
            Description = $"The wagon comes across a young man skipping stones across a pond. He consistently gets about eight or ten skips with each stone he throws.\n\n \"HEY! I'll bet you wimps {BetAmount} gold I can skip a rock more times than your best arm!\"";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            string optionTitle;
            string optionResultText;
            Reward optionReward = null;
            Penalty optionPenalty = null;
            if (TravelManager.Instance.Party.Gold >= BetAmount)
            {
                var bestArm = TravelManager.Instance.Party.GetCompanionWithHighestRangedSkill();

                optionTitle = $"{bestArm.Name} takes the bet";
                optionResultText = $"\"Best two out of three?\" says {bestArm.FirstName()}. \n\n";

                var opponentRoll = 50 + Random.Range(1, 21);

                var companionRoll = bestArm.Stats.RangedSkill + Random.Range(1, 21);

                if (companionRoll >= opponentRoll)
                {
                    optionResultText += $"{bestArm.FirstName()} crushes the challenger. Easy money!";

                    optionReward = new Reward();
                    optionReward.AddPartyGain(PartySupplyTypes.Gold, BetAmount);
                }
                else
                {
                    optionResultText += $"{bestArm.FirstName()} tries his best, but can't quite beat the young man's skipping ability.";

                    optionPenalty = new Penalty();
                    optionPenalty.AddPartyLoss(PartySupplyTypes.Gold, BetAmount);
                }

                var optionOne = new Option(optionTitle, optionResultText, optionReward, optionPenalty, EncounterType);

                Options.Add(optionTitle, optionOne);
            }

            optionTitle = "Keep moving";
            optionResultText = "The group starts moving again down the trail. \n\n \"HA! Yee that's what I thought! Don't stand a chance against me!\"";

            var optionTwo = new Option(optionTitle, optionResultText, null, null, EncounterType);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

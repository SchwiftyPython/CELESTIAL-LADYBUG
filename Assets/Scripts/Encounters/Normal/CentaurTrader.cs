using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class CentaurTrader : Encounter
    {
        public CentaurTrader()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Odd Trader";
            Description = "Just another day on the trail when the party arrives at a small trading stall. The young man watching the stall gives you a wave. As you approach, you realize that he is a horse from the waist down! He chuckles at your astonishment.\n\n\"Never cheap out on magic! You know how those teleport spells mess up sometimes? Well...\"\n\nHe spreads his arms wide as if to say 'surprise!'\n\n\"Anyways, what can I get ya?\"";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            string optionTitle;
            string optionResultText = "\"Pleasure doing business with you!\"";

            var itemStore = Object.FindObjectOfType<ItemStore>();

            var weapon = itemStore.GetRandomEquipableItem(EquipLocation.Weapon);

            if (Party.Gold >= weapon.GetPrice())
            {
                optionTitle = $"{weapon.GetDisplayName()} ({weapon.GetPrice()})";

                var weaponReward = new Reward();

                weaponReward.AddToInventory(weapon);

                var weaponPenalty = new Penalty();

                weaponPenalty.AddPartyLoss(PartySupplyTypes.Gold, weapon.GetPrice());

                var weaponOption = new Option(optionTitle, optionResultText, weaponReward, weaponPenalty,
                    EncounterType.Normal);

                Options.Add(optionTitle, weaponOption);
            }

            var slot = GlobalHelper.GetRandomEnumValue<EquipLocation>();

            while (slot == EquipLocation.Weapon || slot == EquipLocation.Book)
            {
                slot = GlobalHelper.GetRandomEnumValue<EquipLocation>();
            }

            var armor = itemStore.GetRandomEquipableItem(slot);

            if (Party.Gold >= armor.GetPrice())
            {
                optionTitle = $"{armor.GetDisplayName()} ({armor.GetPrice()})";

                var armorReward = new Reward();

                armorReward.AddToInventory(weapon);

                var armorPenalty = new Penalty();

                armorPenalty.AddPartyLoss(PartySupplyTypes.Gold, weapon.GetPrice());

                var armorOption = new Option(optionTitle, optionResultText, armorReward, armorPenalty,
                    EncounterType.Normal);

                Options.Add(optionTitle, armorOption);
            }

            const int supplyCost = 30;

            if (Party.Gold >= supplyCost)
            {
                optionTitle = $"Some supplies {supplyCost}";

                var supplyReward = new Reward();

                supplyReward.AddPartyGain(PartySupplyTypes.Food, 5);
                supplyReward.AddPartyGain(PartySupplyTypes.HealthPotions, 1);

                var supplyPenalty = new Penalty();

                supplyPenalty.AddPartyLoss(PartySupplyTypes.Gold, supplyCost);

                var supplyOption = new Option(optionTitle, optionResultText, supplyReward, supplyPenalty,
                    EncounterType.Normal);

                Options.Add(optionTitle, supplyOption);
            }

            optionTitle = "Nothing";

            optionResultText = "You continue down the trail.";

            var nothingOption = new Option(optionTitle, optionResultText, null, null, EncounterType.Normal);

            Options.Add(optionTitle, nothingOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}

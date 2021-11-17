using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Encounters;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.SceneManagement;
using ISaveable = Assets.Scripts.Saving.ISaveable;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Travel
{
    public class TravelManager : MonoBehaviour, ISubscriber, ISaveable
    {
        private const int DemoDaysToDestination = 5;
        private const int FullGameDaysToDestination = 15;

        private const int DemoBiomeChangeFrequency = 2;
        private const int BiomeChangeFrequency = 3;

        private const BiomeType StartingBiome = BiomeType.Forest;
        //private const BiomeType EndBiome = BiomeType.Evil; todo

        private struct TravelManagerDto
        {
            public Queue<BiomeType> BiomeQueue;
            public int CurrentDayOfTravel;
            public int DaysTilNextBiome;
            public int TravelDaysTilDestination;
            public object Party;
            public BiomeType CurrentBiome;
            public Inventory.InventorySlotRecord[] Inventory;
        }

        private Queue<BiomeType> _biomeQueue;

        private int _daysTilNextBiome;

        private MusicController _musicController;
        private TravelMessenger _travelMessenger;

        public int TravelDaysToDestination { get; private set; }
        public int CurrentDayOfTravel { get; private set; }

        public Party Party { get; private set; }

        public BiomeType CurrentBiome { get; set; }

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (!SceneManager.GetActiveScene().name.Equals(GlobalHelper.CombatScene))
            {
                eventMediator.SubscribeToEvent(GlobalHelper.CampingEncounterFinished, this);
            }

            eventMediator.SubscribeToEvent(GlobalHelper.EntityDead, this);

            BuildBiomeQueue();

            CurrentDayOfTravel = 1;

            ResetTravelDays();

            ResetDaysTilNextBiome();

            _musicController = FindObjectOfType<MusicController>();

            _travelMessenger = FindObjectOfType<TravelMessenger>();
        }

        public void ResetTravelDays()
        {
            if (GameManager.Instance.DemoMode)
            {
                TravelDaysToDestination = DemoDaysToDestination;
            }
            else
            {
                TravelDaysToDestination = FullGameDaysToDestination;
            }
        }

        private void ResetDaysTilNextBiome()
        {
            if (GameManager.Instance.DemoMode)
            {
                _daysTilNextBiome = DemoBiomeChangeFrequency;
            }
            else
            {
                _daysTilNextBiome = BiomeChangeFrequency;
            }
        }

        public void NewParty()
        {
            Party = new Party();
        }

        public void NewInventory()
        {
            var inventory = Inventory.GetPartyInventory();
            inventory.GenerateStartingItems();
        }

        public void StartNewDay()
        {
            var encounterManager = FindObjectOfType<EncounterManager>();
            encounterManager.BuildDecksForNewDay();

            PlayTravelMusic();
        }

        public void PlayTravelMusic()
        {
            _musicController.PlayTravelMusic();
        }

        public string BuildPartyRewardTextItem(int value, PartySupplyTypes gainType)
        {
            return $"Gained {value} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        public string BuildPartyLossTextItem(int value, PartySupplyTypes lossType)
        {
            return $"Lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public string BuildCompanionRewardTextItem(Entity companion, int value, EntityStatTypes gainType) //todo can gain type be an enum?
        {
            return $"{companion.Name} gained {value} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        private string BuildCompanionRewardTextItem(Entity companion, int attributeGainValue, EntityAttributeTypes gainType)
        {
            return $"{companion.Name} gained {attributeGainValue} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        private string BuildCompanionRewardTextItem(Entity companion, int attributeGainValue, EntitySkillTypes gainType)
        {
            return $"{companion.Name} gained {attributeGainValue} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        private string BuildPartyAdditionTextItem(Entity companion)
        {
            return $"{companion.Name} joins the party!";
        }

        private string BuildItemAdditionTextItem(EquipableItem item)
        {
            return $"{item.GetDisplayName()} added to inventory!";
        }

        private string BuildPartyRemovalTextItem(Entity companion)
        {
            return $"{companion.Name} leaves the party!";
        }

        public string BuildCompanionLossTextItem(Entity companion, int value, EntityStatTypes lossType)
        {
            return $"{companion.Name} lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public string BuildCompanionLossTextItem(Entity companion, int value, EntityAttributeTypes lossType)
        {
            return $"{companion.Name} lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public string BuildCompanionLossTextItem(Entity companion, int value, EntitySkillTypes lossType)
        {
            return $"{companion.Name} lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public void ApplyEncounterReward(Reward reward)
        {
            if (reward.Effects != null && reward.Effects.Count > 0)
            {
                //todo apply effects
            }

            ApplyPartyReward(reward);
            ApplyEntityReward(reward);
            ApplyPartyAdditions(reward);
            ApplyInventoryAdditions(reward);
        }

        public void ApplyPartyReward(Reward partyReward)
        {
            if (_travelMessenger == null)
            {
                _travelMessenger = FindObjectOfType<TravelMessenger>();
            }

            var rewardsText = new List<TravelMessenger.PartyMessageDto>();

            if (partyReward.PartyGains != null && partyReward.PartyGains.Count > 0)
            {
                foreach (var partyGain in partyReward.PartyGains)
                {
                    var gainType = (PartySupplyTypes)partyGain.Key;

                    switch (gainType)
                    {
                        case PartySupplyTypes.Food:
                            Party.Food += partyGain.Value;
                            break;
                        case PartySupplyTypes.HealthPotions:
                            Party.HealthPotions += partyGain.Value;
                            break;
                        case PartySupplyTypes.Gold:
                            Party.Gold += partyGain.Value;
                            break;
                        default:
                            Debug.Log($"Invalid gain type! {gainType}");
                            break;
                    }

                    var partyDto = new TravelMessenger.PartyMessageDto
                    {
                        Message = BuildPartyRewardTextItem(partyGain.Value, gainType),
                        TextColor = _travelMessenger.rewardColor
                    };

                    rewardsText.Add(partyDto);
                }
            }

            _travelMessenger.QueuePartyMessages(rewardsText);
        }

        public void ApplyEntityReward(Reward entityReward)
        {
            if (entityReward.EntityStatGains != null && entityReward.EntityStatGains.Count > 0)
            {
                foreach (var entityGain in entityReward.EntityStatGains)
                {
                    var targetEntity = entityGain.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var statGain in entityGain.Value)
                        {
                            var gainType = statGain.Key;

                            var moddedGain = 0;
                            switch (gainType)
                            {
                                case EntityStatTypes.CurrentMorale:
                                    moddedGain = companion.AddMorale(statGain.Value); //modded gain was used to reflect actual gains, but kinda confusing in practice
                                    break;
                                case EntityStatTypes.CurrentHealth:
                                    moddedGain = companion.AddHealth(statGain.Value);
                                    break;
                                case EntityStatTypes.CurrentEnergy:
                                    moddedGain = companion.AddEnergy(statGain.Value);
                                    break;
                                case EntityStatTypes.MaxMorale:
                                    companion.Stats.MaxMorale += statGain.Value;
                                    break;
                                default:
                                    Debug.Log($"Invalid gain type! {gainType}");
                                    break;
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionRewardTextItem(companion, statGain.Value, gainType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }

            if (entityReward.EntityAttributeGains != null && entityReward.EntityAttributeGains.Count > 0)
            {
                foreach (var entityGain in entityReward.EntityAttributeGains)
                {
                    var targetEntity = entityGain.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var attributeGain in entityGain.Value)
                        {
                            var gainType = attributeGain.Key;

                            switch (gainType)
                            {
                                case EntityAttributeTypes.Agility:
                                    companion.Attributes.Agility += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Coordination:
                                    companion.Attributes.Coordination += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Physique:
                                    companion.Attributes.Physique += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Intellect:
                                    companion.Attributes.Intellect += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Acumen:
                                    companion.Attributes.Acumen += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Charisma:
                                    companion.Attributes.Charisma += attributeGain.Value;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionRewardTextItem(companion, attributeGain.Value, gainType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }

            if (entityReward.EntitySkillGains != null && entityReward.EntitySkillGains.Count > 0)
            {
                foreach (var entityGain in entityReward.EntitySkillGains)
                {
                    var targetEntity = entityGain.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var skillGain in entityGain.Value)
                        {
                            var gainType = skillGain.Key;

                            switch (gainType)
                            {
                                case EntitySkillTypes.Melee:
                                    companion.Skills.Melee += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Ranged:
                                    companion.Skills.Ranged += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Sneak:
                                    companion.Skills.Sneak += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Endurance:
                                    companion.Skills.Endurance += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Healing:
                                    companion.Skills.Healing += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Survival:
                                    companion.Skills.Survival += skillGain.Value;
                                    break;
                                case EntitySkillTypes.Persuasion:
                                    companion.Skills.Persuasion += skillGain.Value;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionRewardTextItem(companion, skillGain.Value, gainType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }
        }

        private void ApplyPartyAdditions(Reward reward)
        {
            if (reward.PartyAdditions == null || reward.PartyAdditions.Count < 1)
            {
                return;
            }

            foreach (var companion in reward.PartyAdditions)
            {
                Party.AddCompanion(companion);

                var entityDto = new TravelMessenger.EntityMessageDto
                {
                    Message = BuildPartyAdditionTextItem(companion),
                    Portrait = companion.Portrait,
                    TextColor = _travelMessenger.rewardColor
                };

                _travelMessenger.QueueEntityMessage(entityDto);
            }
        }

        private void ApplyInventoryAdditions(Reward reward)
        {
            if (reward.InventoryGains == null || reward.InventoryGains.Count < 1)
            {
                return;
            }

            var inventory = Inventory.GetPartyInventory();

            foreach (var item in reward.InventoryGains)
            {
                inventory.AddToFirstEmptySlot(item, 1);

                var partyDto = new TravelMessenger.PartyMessageDto
                {
                    Message = BuildItemAdditionTextItem(item),
                    TextColor = _travelMessenger.rewardColor
                };

                _travelMessenger.QueuePartyMessages(new List<TravelMessenger.PartyMessageDto> { partyDto });
            }
        }

        private void ApplyPartyRemovals(Penalty penalty)
        {
            if (penalty.PartyRemovals == null || penalty.PartyRemovals.Count < 1)
            {
                return;
            }

            foreach (var companion in penalty.PartyRemovals)
            {
                Party.RemoveCompanion(companion);

                var entityDto = new TravelMessenger.EntityMessageDto
                {
                    Message = BuildPartyRemovalTextItem(companion),
                    Portrait = companion.Portrait,
                    TextColor = _travelMessenger.penaltyColor
                };

                _travelMessenger.QueueEntityMessage(entityDto);
            }
        }

        public void ApplyEncounterPenalty(Penalty penalty)
        {
            if (penalty.Effects != null && penalty.Effects.Count > 0)
            {
                //todo apply effects
            }

            ApplyPartyPenalty(penalty);
            ApplyEntityPenalty(penalty);
            ApplyPartyRemovals(penalty);
        }

        private void ApplyPartyPenalty(Penalty partyPenalty)
        {
            if (_travelMessenger == null)
            {
                _travelMessenger = FindObjectOfType<TravelMessenger>();
            }

            var penaltiesText = new List<TravelMessenger.PartyMessageDto>();

            if (partyPenalty.PartyLosses != null && partyPenalty.PartyLosses.Count > 0)
            {
                foreach (var partyLoss in partyPenalty.PartyLosses)
                {
                    var lossType = partyLoss.Key;

                    switch (lossType)
                    {
                        case PartySupplyTypes.Food:
                            Party.Food -= partyLoss.Value;
                            break;
                        case PartySupplyTypes.HealthPotions:
                            Party.HealthPotions -= partyLoss.Value;
                            break;
                        case PartySupplyTypes.Gold:
                            Party.Gold -= partyLoss.Value;
                            break;
                        default:
                            Debug.Log($"Invalid loss type! {lossType}");
                            break;
                    }

                    var partyDto = new TravelMessenger.PartyMessageDto
                    {
                        Message = BuildPartyLossTextItem(partyLoss.Value, lossType),
                        TextColor = _travelMessenger.penaltyColor
                    };

                    penaltiesText.Add(partyDto);
                }
            }

            _travelMessenger.QueuePartyMessages(penaltiesText);
        }

        private void ApplyEntityPenalty(Penalty entityPenalty)
        {
            if (entityPenalty.EntityStatLosses != null && entityPenalty.EntityStatLosses.Count > 0)
            {
                foreach (var entityLoss in entityPenalty.EntityStatLosses)
                {
                    var targetEntity = entityLoss.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var statLoss in entityLoss.Value)
                        {
                            var lossType = statLoss.Key;

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionLossTextItem(companion, statLoss.Value, lossType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.penaltyColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);

                            switch (lossType)
                            {
                                case EntityStatTypes.CurrentMorale:
                                    companion.SubtractMorale(statLoss.Value);
                                    break;
                                case EntityStatTypes.CurrentHealth:
                                    companion.SubtractHealth(statLoss.Value);
                                    break;
                                case EntityStatTypes.CurrentEnergy:
                                    companion.SubtractEnergy(statLoss.Value);
                                    break;
                                case EntityStatTypes.MaxMorale:
                                    companion.Stats.MaxMorale -= statLoss.Value;
                                    break;
                                default:
                                    Debug.Log($"Invalid loss type! {lossType}");
                                    break;
                            }
                        }
                    }
                }
            }

            if (entityPenalty.EntityAttributeLosses != null && entityPenalty.EntityAttributeLosses.Count > 0)
            {
                foreach (var entityLoss in entityPenalty.EntityAttributeLosses)
                {
                    var targetEntity = entityLoss.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var attributeLoss in entityLoss.Value)
                        {
                            var lossType = attributeLoss.Key;

                            switch (lossType)
                            {
                                case EntityAttributeTypes.Agility:
                                    companion.Attributes.Agility -= attributeLoss.Value;
                                    break;
                                case EntityAttributeTypes.Coordination:
                                    companion.Attributes.Coordination -= attributeLoss.Value;
                                    break;
                                case EntityAttributeTypes.Physique:
                                    companion.Attributes.Physique -= attributeLoss.Value;
                                    break;
                                case EntityAttributeTypes.Intellect:
                                    companion.Attributes.Intellect -= attributeLoss.Value;
                                    break;
                                case EntityAttributeTypes.Acumen:
                                    companion.Attributes.Acumen -= attributeLoss.Value;
                                    break;
                                case EntityAttributeTypes.Charisma:
                                    companion.Attributes.Charisma -= attributeLoss.Value;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionLossTextItem(companion, attributeLoss.Value, lossType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.penaltyColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }

            if (entityPenalty.EntitySkillLosses != null && entityPenalty.EntitySkillLosses.Count > 0)
            {
                foreach (var entityLoss in entityPenalty.EntitySkillLosses)
                {
                    var targetEntity = entityLoss.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanionByName(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var skillLoss in entityLoss.Value)
                        {
                            var lossType = skillLoss.Key;

                            switch (lossType)
                            {
                                case EntitySkillTypes.Melee:
                                    companion.Skills.Melee -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Ranged:
                                    companion.Skills.Ranged -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Sneak:
                                    companion.Skills.Sneak -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Endurance:
                                    companion.Skills.Endurance -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Healing:
                                    companion.Skills.Healing -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Survival:
                                    companion.Skills.Survival -= skillLoss.Value;
                                    break;
                                case EntitySkillTypes.Persuasion:
                                    companion.Skills.Persuasion -= skillLoss.Value;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionLossTextItem(companion, skillLoss.Value, lossType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }
        }

        private void BuildBiomeQueue()
        {
            CurrentBiome = StartingBiome;

            _biomeQueue = new Queue<BiomeType>();

            var biomes = Enum.GetValues(typeof(BiomeType)).Cast<BiomeType>().ToList();

            biomes.Remove(StartingBiome);
            //biomes.Remove(EndBiome); todo

            while (biomes.Count > 0)
            {
                var index = Random.Range(0, biomes.Count);

                _biomeQueue.Enqueue(biomes[index]);

                biomes.RemoveAt(index);
            }

            //todo add end biome to end of queue
        }

        private void MoveToNextBiome()
        {
            if (_biomeQueue == null)
            {
                return;
            }

            ResetDaysTilNextBiome();

            CurrentBiome = _biomeQueue.Dequeue();

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.BiomeChanged, this);
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator?.UnsubscribeFromAllEvents(this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.CampingEncounterFinished))
            {
                //todo need to fade screen or possibly start swapping out the sprites up ahead

                Party.EatAndHeal();

                CurrentDayOfTravel++;

                _daysTilNextBiome--;

                if (_daysTilNextBiome <= 0)
                {
                    MoveToNextBiome();
                }

                var countsAsDayTraveled = parameter != null && (bool) parameter;

                if (countsAsDayTraveled)
                {
                    TravelDaysToDestination--;
                }

                if (TravelDaysToDestination <= 0)
                {
                    var eventMediator = FindObjectOfType<EventMediator>();
                    eventMediator.Broadcast(GlobalHelper.YouWon, this);
                }
                else
                {
                    StartNewDay();

                    var saveSystem = FindObjectOfType<SavingSystem>();

                    saveSystem.AutoSave();
                }
            }
            else if (eventName.Equals(GlobalHelper.EntityDead))
            {
                if (!(broadcaster is Entity deadGuy) || !deadGuy.IsPlayer())
                {
                    return;
                }

                Party.RemoveCompanion(deadGuy);

                var entityDto = new TravelMessenger.EntityMessageDto
                {
                    Message = $"{deadGuy.Name} died!",
                    TextColor = _travelMessenger.penaltyColor,
                    Portrait = deadGuy.Portrait
                };

                _travelMessenger.QueueEntityMessage(entityDto);
            }
        }

        public object CaptureState()
        {
            var dto = new TravelManagerDto();

            dto.BiomeQueue = _biomeQueue;
            dto.CurrentBiome = CurrentBiome;
            dto.CurrentDayOfTravel = CurrentDayOfTravel;
            dto.DaysTilNextBiome = _daysTilNextBiome;
            dto.TravelDaysTilDestination = TravelDaysToDestination;
            dto.Party = Party.CaptureState();
            dto.Inventory = (Inventory.InventorySlotRecord[])Inventory.GetPartyInventory().CaptureState();

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            TravelManagerDto dto = (TravelManagerDto)state;

            _biomeQueue = dto.BiomeQueue;
            CurrentBiome = dto.CurrentBiome;
            CurrentDayOfTravel = dto.CurrentDayOfTravel;
            _daysTilNextBiome = dto.DaysTilNextBiome;
            TravelDaysToDestination = dto.TravelDaysTilDestination;
            Inventory.GetPartyInventory().RestoreState(dto.Inventory);

            Party = new Party();

            Party.RestoreState(dto.Party);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Abilities;
using Assets.Scripts.AI;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Effects.Args;
using Assets.Scripts.Entities.Names;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using GoRogue;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Entity : GameObject
    {
        private int _level;
        private int _xp;
        private bool _isPlayer;

        private Equipment _equipment;

        //private Armor _equippedArmor;

        //todo use this instead of get component if possible
        private AiController _aiController;

        private int _lastTurnMoved;

        public string Name { get; set; }
        public Sex Sex { get; }
        public Race Race { get; }
        public EntityClass EntityClass { get; }
        public Attributes Attributes { get; }
        public Stats Stats { get; }
        public Skills Skills { get; }
        public Dictionary<Portrait.Slot, string> Portrait { get; private set; }
        //public Weapon EquippedWeapon { get; private set; }
        public Dictionary<Type, Ability> Abilities { get; private set; }

        public List<Effect> Effects { get; set; }

        public EffectTrigger<EffectArgs> EffectTriggers { get; set; }

        public UnityEngine.GameObject CombatSpritePrefab { get; private set; }
        public UnityEngine.GameObject CombatSpriteInstance { get; private set; }
        public Sprite UiSprite { get; private set; } //todo setup a sprite store that can give us the combat and ui sprites needed


        public Entity(bool isPlayer, bool isDerpus = false) : base((-1, -1), 1, null, false, false, true)
        {
            if (isDerpus)
            {
                Race = Race.Derpus;
                EntityClass = EntityClass.Derpus;
                Sex = Sex.Male;
                Name = "Derpus";
                CombatSpritePrefab = EntityPrefabStore.Instance.DerpusPrototypePrefab;
            }
            else
            {
                Race = PickRace();

                while (Race == Race.Derpus)
                {
                    Race = PickRace();
                }

                //EntityClass = PickEntityClass();
                EntityClass = EntityClass.Crossbowman; //todo testing

                while (EntityClass == EntityClass.Derpus)
                {
                    EntityClass = PickEntityClass();
                }

                Sex = PickSex();
                Name = GenerateName(null, Sex);

                //todo pick prefab based on race and/or class
                if (isPlayer)
                {
                    CombatSpritePrefab = EntityPrefabStore.Instance.CompanionPrototypePrefab;
                    _isPlayer = true;
                }
                else
                {
                    CombatSpritePrefab = EntityPrefabStore.Instance.EnemyPrototypePrefab;
                    _isPlayer = false;
                }
            }

            Attributes = new Attributes(this);
            Skills = new Skills(this);

            Abilities = new Dictionary<Type, Ability>();

            GenerateStartingEquipment();

            Stats = new Stats(this, Attributes, Skills);

            //Effects = new List<Effect>();

            EffectTriggers = new EffectTrigger<EffectArgs>();

            GeneratePortrait();

            _level = 1;
            _xp = 0;
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            CombatSpriteInstance = instance;
        }

        public void SetSpritePosition(Vector3 newPosition)
        {
            if (CombatSpriteInstance == null)
            {
                return;
            }

            CombatSpriteInstance.transform.position = newPosition;
        }

        public bool IsPlayer()
        {
            return _isPlayer;
        }

        public bool IsDead()
        {
            return Stats.CurrentHealth <= 0;
        }

        public bool IsDerpus()
        {
            return EntityClass == EntityClass.Derpus || Race == Race.Derpus;
        }

        public string FirstName()
        {
            return Regex.Replace(Name.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
        }

        public bool MovedLastTurn(int currentTurn)
        {
            return currentTurn - _lastTurnMoved <= 1;
        }

        public void RefillActionPoints()
        {
            Stats.CurrentActionPoints = Stats.MaxActionPoints;
        }

        //todo refactor this so the sprite moves through each square and doesn't just teleport
        public void MoveTo(Tile tile, int apMovementCost)
        {
            if (apMovementCost > Stats.CurrentActionPoints)
            {
                Debug.Log("AP movement cost greater than current AP!");
                return;
            }

            Stats.CurrentActionPoints -= apMovementCost;

            var currentTile = ((CombatMap) CurrentMap).GetTileAt(Position);

            currentTile.SpriteInstance.GetComponent<TerrainSlotUi>().SetEntity(null);

            //todo this will not update the position if blocked
            //we could make a method to encapsulate this and check if position was updated to make it more clear
            Position = tile.Position;

            CombatSpriteInstance.transform.position = new Vector3(Position.X, Position.Y);

            tile.SpriteInstance.GetComponent<TerrainSlotUi>().SetEntity(this);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.ActiveEntityMoved, this);

            var tileEffects = tile.GetEffects();

            /*if (Effects.Count > 0)
            {
                foreach (var effect in Effects.ToArray())
                {
                    if (!effect.IsLocationDependent())
                    {
                        continue;
                    }

                    if (tileEffects != null && tileEffects.Any())
                    {
                        foreach (var tileEffect in tileEffects)
                        {
                            if (ReferenceEquals(tileEffect, effect))
                            {
                                continue;
                            }

                            RemoveEffect(effect);
                        }
                    }
                    else
                    {
                        RemoveEffect(effect);
                    }
                }
            }*/

            if (EffectTriggers.Effects.Count > 0)
            {
                foreach (var effect in EffectTriggers.Effects.ToArray())
                {
                    if (!((Effect)effect).IsLocationDependent())
                    {
                        continue;
                    }

                    if (tileEffects != null && tileEffects.Any())
                    {
                        foreach (var tileEffect in tileEffects)
                        {
                            if (ReferenceEquals(tileEffect, effect))
                            {
                                continue;
                            }

                            RemoveEffect((Effect) effect);
                        }
                    }
                    else
                    {
                        RemoveEffect((Effect) effect);
                    }
                }
            }

            if (tileEffects != null && tileEffects.Any())
            {
                foreach (var effect in tileEffects)
                {
                    ApplyEffect(effect);
                }
            }
        }

        public void GenerateStartingEquipment()
        {
            var itemStore = Object.FindObjectOfType<ItemStore>();

            //todo not implemented - temp for testing

            _equipment = new Equipment(EntityClass);

            var testWeapon = itemStore.GetRandomEquipableItem(EquipLocation.Weapon);

            Equip(testWeapon);

            var testArmor = itemStore.GetRandomEquipableItem(EquipLocation.Body);

            Equip(testArmor);

            var testHelmet = itemStore.GetRandomEquipableItem(EquipLocation.Helmet);

            Equip(testHelmet);

            var testBoots = itemStore.GetRandomEquipableItem(EquipLocation.Boots);

            Equip(testBoots);

            var testGloves = itemStore.GetRandomEquipableItem(EquipLocation.Gloves);

            Equip(testGloves);

            var testShield = itemStore.GetRandomEquipableItem(EquipLocation.Shield);

            Equip(testShield);

            var testRing = itemStore.GetItemTypeByName("Skull Ring");

            Equip((EquipableItem)testRing.NewItem());
        }

        public void Equip(EquipableItem item)
        {
            if (!_equipment.ItemValidForEntityClass(item))
            {
                return;
            }

            if (Abilities == null)
            {
                Abilities = new Dictionary<Type, Ability>();
            }

            foreach (var ability in item.GetAbilities(this))
            {
                if (!Abilities.ContainsKey(ability.GetType()))
                {
                    Abilities.Add(ability.GetType(), ability);
                }
            }

            _equipment.AddItem(item.GetAllowedEquipLocation(), item);
        }

        public void UnEquip(EquipLocation slot, bool swapAttempt)
        {
            var item = _equipment.GetItemInSlot(slot);

            _equipment.RemoveItem(slot);

            foreach (var ability in item.GetAbilities(this))
            {
                if (!_equipment.AbilityEquipped(ability))
                {
                    Abilities.Remove(ability.GetType());
                }
            }

            if (swapAttempt)
            {
                return;
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        public bool HasAbility(Type abilityType)
        {
            foreach (var ability in Abilities)
            {
                if (abilityType == ability.Key)
                {
                    return true;
                }
            }

            return false;
        }

        public void ResetOneUseCombatAbilities()
        {
            if (HasAbility(typeof(EndangeredEndurance)) &&
                !((EndangeredEndurance)Abilities[typeof(EndangeredEndurance)])
                    .SavedFromDeathThisBattle())
            {
                ((EndangeredEndurance)Abilities[typeof(EndangeredEndurance)]).Reset();
            }
        }

        public void MeleeAttack(Entity target)
        {
            var hitChance = CalculateChanceToHitMelee(target);

            if (AttackHit(hitChance, target)) 
            {
                ApplyDamage(target, false);

                if (target.IsDead())
                {
                    //todo sound for dying peep

                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility(typeof(Riposte))) //todo hail mary not sure if this will work
                    {
                        target.Abilities[typeof(Riposte)].Use(this);
                    }

                }
            }
        }

        public void MeleeAttackWithSlot(Entity target, EquipLocation slot)
        {
            var hitChance = CalculateChanceToHitMelee(target);

            if (AttackHit(hitChance, target))
            {
                ApplyDamage(target, false, slot);

                if (target.IsDead())
                {
                    //todo sound for dying peep

                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility(typeof(Riposte))) //todo hail mary not sure if this will work
                    {
                        target.Abilities[typeof(Riposte)].Use(this);
                    }

                }
            }
        }

        public void RangedAttack(Entity target)
        {
            var hitChance = CalculateChanceToHitRanged(target);

            if (AttackHit(hitChance, target))
            {
                ApplyDamage(target, true);

                if (target.IsDead())
                {
                    //todo sound for dying peep

                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility(typeof(Riposte))) //todo hail mary not sure if this will work
                    {
                        target.Abilities[typeof(Riposte)].Use(this);
                    }

                }
            }
        }

        public void ApplyDamage(Entity target, bool ranged, EquipLocation slot = EquipLocation.Weapon)
        {
            var equippedItem = _equipment.GetItemInSlot(slot);

            int minDamage = 0;
            int maxDamage = 0;

            if (equippedItem != null)
            {
                if (ranged)
                {
                    (minDamage, maxDamage) = equippedItem.GetRangedDamageRange();
                }
                else
                {
                    (minDamage, maxDamage) = equippedItem.GetMeleeDamageRange();
                }
            }

            var damage = Random.Range(minDamage, maxDamage + 1) + Stats.Attack;

            damage = GlobalHelper.ModifyNewValueForStat(this, CombatModifierTypes.Damage, damage);

            var targetArmor = target.GetTotalArmorToughness();

            var damageReduction = GetDamageReduction(damage, targetArmor);

            damage -= (int)(damage * damageReduction);

            target.SubtractHealth(damage);

            var message = $"{Name} dealt {damage} damage to {target.Name}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            if (!target.HasAbility(typeof(DemonicIntervention)))
            {
                return;
            }

            if (!DemonicIntervention.Intervened())
            {
                return;
            }

            SubtractHealth(damage);

            message = $"Demonic Intervention! {target.Name} dealt {damage} damage to {Name}!";

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
        }

        public bool HasMissileWeaponEquipped()
        {
            var equippedWeapon = _equipment.GetItemInSlot(EquipLocation.Weapon);

            if (equippedWeapon == null)
            {
                return false;
            }

            var (min, max) = equippedWeapon.GetRangedDamageRange();

            return min > 0 && max > 0;
        }

        private int GetTotalArmorToughness()
        {
            var toughnessTotal = 0;

            foreach (EquipLocation location in Enum.GetValues(typeof(EquipLocation)))
            {
                if(location == EquipLocation.Weapon)
                {
                    continue;
                }

                var  equippedArmor = _equipment.GetItemInSlot(location);

                if (equippedArmor == null)
                {
                    continue;
                }

                toughnessTotal += equippedArmor.GetToughness();
            }

            return toughnessTotal;
        }

        public EquipableItem GetEquippedWeapon()
        {
            return _equipment.GetItemInSlot(EquipLocation.Weapon);
        }

        public EquipableItem GetEquippedItemInSlot(EquipLocation slot)
        {
            return _equipment.GetItemInSlot(slot);
        }

        public Equipment GetEquipment()
        {
            return _equipment;
        }

        public bool TargetInRange(Entity target)
        {
            //todo prob not needed since everything is ability based
            throw new NotImplementedException();
        }

        public int CalculateChanceToHitMelee(Entity target)
        {
            var total = CalculateBaseChanceToHit(target);

            total += (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.MeleeToHit);

            return total;
        }

        public int CalculateChanceToHitRanged(Entity target)
        {
            var total = CalculateBaseChanceToHit(target);

            total += (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.RangedToHit);

            return total;
        }

        private int CalculateBaseChanceToHit(Entity target)
        {
            Debug.Log($"Attacker Melee Skill: {Stats.MeleeSkill}");
            Debug.Log($"Defender Melee Skill: {target.Stats.MeleeSkill}");

            return Stats.MeleeSkill - target.Stats.MeleeSkill / 10;
        }

        private bool AttackHit(int chanceToHit, Entity target)
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();

            //todo diceroller
            var roll = Random.Range(1, 101);

            string message;
            if (roll <= chanceToHit)
            {
                if (target.HasAbility(typeof(DivineIntervention)))
                {
                    if (DivineIntervention.Intervened())
                    {
                        message = $"Divine Intervention! Attack missed!";

                        eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                        return false;
                    }
                }

                message = $"Attack hit!";

                eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                eventMediator.Broadcast(GlobalHelper.TargetHit, this, target);

                return true;
            }

            message = $"Attack missed!";

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.Broadcast(GlobalHelper.TargetMiss, this);

            return false;
        }

        public int AddHealth(int amount)
        {
            var startingHealth = Stats.CurrentHealth;

            Stats.CurrentHealth += amount;

            return Stats.CurrentHealth - startingHealth;
        }

        public void SubtractHealth(int amount)
        {
            Stats.CurrentHealth -= amount;
        }

        public int AddEnergy(int amount)
        {
            var startingEnergy = Stats.CurrentEnergy;

            Stats.CurrentEnergy += amount;

            return Stats.CurrentEnergy - startingEnergy;
        }

        public void SubtractEnergy(int amount)
        {
            Stats.CurrentEnergy -= amount;
        }

        public int AddMorale(int amount)
        {
            var startingMorale = Stats.CurrentMorale;

            Stats.CurrentMorale += amount;

            return Stats.CurrentMorale - startingMorale;
        }

        public void SubtractMorale(int amount)
        {
            Stats.CurrentMorale -= amount;
        }

        public void AddActionPoints(int amount)
        {
            Stats.CurrentActionPoints += amount;
        }

        public void SubtractActionPoints(int amount)
        {
            Stats.CurrentActionPoints -= amount;
        }

        public void AddMight(int amount)
        {
            Attributes.Physique += amount;
        }

        public void SubtractMight(int amount)
        {
            Attributes.Physique -= amount;
        }

        public void UseHealthPotion()
        {
            Heal(40); //todo make a constant somewhere
        }

        public void Heal(int amount)
        {
            AddHealth(amount);
        }

        public void ApplyEffect(Effect effect)
        {
            // if (Effects == null)
            // {
            //     Effects = new List<Effect>();
            // }

            if (EffectTriggers == null)
            {
                EffectTriggers = new EffectTrigger<EffectArgs>();
            }

            // if (!effect.CanStack())
            // {
            //     foreach (var existingEffect in Effects)
            //     {
            //         if (existingEffect.GetType() == effect.GetType())
            //         {
            //             return;
            //         }
            //     }
            // }

            if (!effect.CanStack())
            {
                foreach (var existingEffect in EffectTriggers.Effects)
                {
                    if (existingEffect.GetType() == effect.GetType())
                    {
                        return;
                    }
                }
            }

            //Effects.Add(effect);

            EffectTriggers.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            // if (Effects == null || Effects.Count < 1)
            // {
            //     return;
            // }

            if (EffectTriggers == null)
            {
                return;
            }

            //Effects.Remove(effect);

            EffectTriggers.Remove(effect);
        }

        public void TriggerEffects()
        {
            if (EffectTriggers == null)
            {
                return;
            }

            //todo getting an enumeration modified error from this sometimes
            EffectTriggers.TriggerEffects(new BasicEffectArgs(this));
        }

        public int RollForInitiative()
        {
            //todo
            return -1;
        }

        public void Rest()
        {
            AddEnergy(45); //todo make a constant somewhere
        }

        private float GetDamageReduction(int damage, int armor)
        {
            var reduction = (damage - armor) * (400f / (400f + armor));

            Debug.Log($"Damage reduction = {reduction}%");

            return reduction / 100;
        }

        private string GenerateName(List<string> possibleNameFiles, Sex sex)
        {
            if (NameStore.Instance == null || !NameStore.Instance.FilesLoaded)
            {
                return GlobalHelper.RandomString(6);
            }

            return NameStore.Instance.GenerateFullName(possibleNameFiles, sex);
        }

        private void GeneratePortrait()
        {
            Portrait = new Dictionary<Portrait.Slot, string>();

            //todo base off of equipped items

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            foreach (Portrait.Slot slot in Enum.GetValues(typeof(Portrait.Slot)))
            {
                Portrait.Add(slot, spriteStore.GetRandomSpriteKeyForSlot(slot));

                //Portrait[slot] = SpriteStore.Instance.GetRandomSpriteKeyForSlot(slot);
            }
        }

        private Sex PickSex()
        {
            return GlobalHelper.GetRandomEnumValue<Sex>();
        }

        private Race PickRace()
        {
            return GlobalHelper.GetRandomEnumValue<Race>();
        }

        private EntityClass PickEntityClass()
        {
            return GlobalHelper.GetRandomEnumValue<EntityClass>();
        }
    }
}

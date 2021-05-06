using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Abilities;
using Assets.Scripts.AI;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities.Names;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Entity : GameObject
    {
        private int _level;
        private int _xp;
        private bool _isPlayer;

        private List<Effect> _effects;

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

                EntityClass = PickEntityClass();

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

            Attributes = new Attributes();
            Skills = new Skills(this);

            Abilities = new Dictionary<Type, Ability>();

            GenerateStartingEquipment();

            Stats = new Stats(this, Attributes, Skills);

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

        //todo refactor this so the sprite moves through each square and doesn't just teleport
        public void MoveTo(Tile tile, int apMovementCost)
        {
            if (apMovementCost > Stats.CurrentActionPoints)
            {
                Debug.Log("AP movement cost greater than current AP!");
                return;
            }

            Stats.CurrentActionPoints -= apMovementCost;

            //todo this will not update the position if blocked
            //we could make a method to encapsulate this and check if position was updated to make it more clear
            Position = tile.Position;

            CombatSpriteInstance.transform.position = new Vector3(Position.X, Position.Y);

            EventMediator.Instance.Broadcast(GlobalHelper.ActiveEntityMoved, this);
        }

        public void GenerateStartingEquipment()
        {
            //todo not implemented - temp for testing

            _equipment = new Equipment(EntityClass);

            var testWeapon = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Weapon);
            
            Equip(testWeapon);

            var testArmor = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Body);

            Equip(testArmor);

            var testHelmet = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Helmet);

            var stoneFaceType = ItemStore.Instance.GetItemTypeByName("Unicorn Helmet"); //todo testing

            Equip((EquipableItem) stoneFaceType.NewItem());

            var testBoots = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Boots);

            Equip(testBoots);

            var testGloves = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Gloves);

            Equip(testGloves);

            var testShield = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Shield);

            Equip(testShield);

            var testRing = ItemStore.Instance.GetRandomEquipableItem(EquipLocation.Ring);

            Equip(testRing);
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

            EventMediator.Instance.Broadcast(GlobalHelper.EquipmentUpdated, this);
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
                ApplyDamage(target);

                if (target.IsDead())
                {
                    //todo sound for dying peep

                    var message = $"{Name} killed {target.Name}!";

                    EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
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
            //todo attack with item in slot
        }

        public void RangedAttack(Entity target)
        {
            throw new NotImplementedException();
        }

        public void ApplyDamage(Entity target)
        {
            var equippedWeapon = _equipment.GetItemInSlot(EquipLocation.Weapon);

            var (minDamage, maxDamage) = equippedWeapon.GetMeleeDamageRange();

            var damage = Random.Range(minDamage, maxDamage + 1) + Stats.Attack;

            var targetArmor = target.GetTotalArmorToughness();

            var damageReduction = GetDamageReduction(damage, targetArmor);

            damage -= (int)(damage * damageReduction);

            target.SubtractHealth(damage);

            var message = $"{Name} dealt {damage} damage to {target.Name}!";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
        }

        public bool HasMissileWeaponEquipped()
        {
            //todo
            return true;
        }

        public int GetTotalArmorToughness()
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
            //todo might need enums for to hit and damage etc so we can use modifier providers.

            var total = CalculateBaseChanceToHit(target);

            if (HasAbility(typeof(GuidedStrikes)))
            {
                total += GuidedStrikes.GetToHitBonus();
            }

            return total;
        }

        public int CalculateChanceToHitRanged(Entity target)
        {
            //todo might need enums for to hit and damage etc so we can use modifier providers.

            var total = CalculateBaseChanceToHit(target);

            if (HasAbility(typeof(Calculated)))
            {
                total += Calculated.GetToHitBonus();
            }

            return total;
        }

        public int CalculateBaseChanceToHit(Entity target)
        {
            Debug.Log($"Attacker Melee Skill: {Stats.MeleeSkill}");
            Debug.Log($"Defender Melee Skill: {target.Stats.MeleeSkill}");

            return Stats.MeleeSkill - target.Stats.MeleeSkill / 10;
        }

        public bool AttackHit(int chanceToHit, Entity target)
        {
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

                        EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                        return false;
                    }
                }

                message = $"Attack hit!";

                EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                EventMediator.Instance.Broadcast(GlobalHelper.MeleeHit, this);

                return true;
            }

            message = $"Attack missed!";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            EventMediator.Instance.Broadcast(GlobalHelper.MeleeMiss, this);

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
            //todo
        }

        public void RemoveEffect(Effect effect)
        {
            //todo
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

            foreach (Portrait.Slot slot in Enum.GetValues(typeof(Portrait.Slot)))
            {
                Portrait.Add(slot, SpriteStore.Instance.GetRandomSpriteKeyForSlot(slot));

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

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Abilities;
using Assets.Scripts.AI;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities.Names;
using Assets.Scripts.Items;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Entity : GameObject
    {
        private Race _race;
        private EntityClass _entityClass;
        private int _level;
        private int _xp;
        private bool _isPlayer;

        private List<Effect> _effects;

        
        private Armor _equippedArmor;

        //todo use this instead of get component if possible
        private AiController _aiController;

        private int _lastTurnMoved;

        public string Name { get; set; }
        public Sex Sex { get; }
        public Attributes Attributes { get; }
        public Stats Stats { get; }
        public Sprite Portrait { get; private set; }
        public Weapon EquippedWeapon { get; private set; }
        public List<Ability> Abilities { get; private set; }

        public UnityEngine.GameObject CombatSpritePrefab { get; private set; }
        public UnityEngine.GameObject CombatSpriteInstance { get; private set; }
        public Sprite UiSprite { get; private set; } //todo setup a sprite store that can give us the combat and ui sprites needed


        public Entity(bool isPlayer, bool isDerpus = false) : base((-1, -1), 1, null, false, false, true)
        {
            if (isDerpus)
            {
                _race = Race.Derpus;
                _entityClass = EntityClass.Derpus;
                Sex = Sex.Male;
                Name = "Derpus";
                CombatSpritePrefab = EntityPrefabStore.Instance.DerpusPrototypePrefab;
            }
            else
            {
                _race = PickRace();

                while (_race == Race.Derpus)
                {
                    _race = PickRace();
                }

                _entityClass = PickEntityClass();

                while (_entityClass == EntityClass.Derpus)
                {
                    _entityClass = PickEntityClass();
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
            Stats = new Stats(Attributes);

            _level = 1;
            _xp = 0;

            Abilities = new List<Ability>();

            //todo for testing need to remove
            var testSlashAbility = new Ability("slash", 3, 1);

            Abilities.Add(testSlashAbility);
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
            return _entityClass == EntityClass.Derpus || _race == Race.Derpus;
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
        }

        public void GenerateStartingEquipment()
        {
            //todo not implemented - temp for testing

            var testSword = new Weapon("Sword", Item.ItemType.Sword, (35, 40), 1);

            EquipWeapon(testSword);

            var testArmor = new Armor("Leather Armor", Item.ItemType.LeatherArmor, 3);

            EquipArmor(testArmor);
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            if (EquippedWeapon != null)
            {
                UnEquipWeapon();
            }

            EquippedWeapon = newWeapon;
        }

        public void UnEquipWeapon()
        {
            //todo
        }

        public void EquipArmor(Armor newArmor)
        {
            if (_equippedArmor != null)
            {
                UnEquipArmor();
            }

            _equippedArmor = newArmor;
        }

        public void UnEquipArmor()
        {
            //todo
        }

        public void MeleeAttack(Entity target)
        {
            var hitChance = CalculateChanceToHit(target);

            if (AttackHit(hitChance)) //todo handle damage in its own method since ranged will use this too
            {
                var (minDamage, maxDamage) = EquippedWeapon.DamageRange;

                var damage = Random.Range(minDamage, maxDamage + 1);

                var targetArmor = target.GetArmorToughness();

                var damageReduction = GetDamageReduction(damage, targetArmor);

                damage -= (int) (damage * damageReduction);

                target.SubtractHealth(damage);

                var message = $"{Name} dealt {damage} damage to {target.Name}!";

                EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                if (target.IsDead())
                {
                    //todo sound for dying peep

                    message = $"{Name} killed {target.Name}!";

                    EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                    EventMediator.Instance.Broadcast(GlobalHelper.EntityDead, this, target);
                }
            }
        }

        public void RangedAttack(Entity target)
        {
            throw new NotImplementedException();
        }

        public bool HasMissileWeaponEquipped()
        {
            //todo
            return true;
        }

        public int GetArmorToughness()
        {
            if (_equippedArmor == null)
            {
                return 0;
            }

            return _equippedArmor.Toughness;
        }

        public bool TargetInRange(Entity target)
        {
            //todo prob not needed since everything is ability based
            throw new NotImplementedException();
        }

        public int CalculateChanceToHit(Entity target)
        {
            //todo need to add modifiers

            return CalculateBaseChanceToHit(target);
        }

        public int CalculateBaseChanceToHit(Entity target)
        {
            Debug.Log($"Attacker Melee Skill: {Stats.MeleeSkill}");
            Debug.Log($"Defender Melee Skill: {target.Stats.MeleeSkill}");

            return Stats.MeleeSkill - target.Stats.MeleeSkill / 10;
        }

        public bool AttackHit(int chanceToHit)
        {
            //todo diceroller
            var roll = Random.Range(1, 101);

            string message;
            if (roll <= chanceToHit)
            {
                message = $"Attack hit! Rolled: {roll} Needed: {chanceToHit}";

                EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                EventMediator.Instance.Broadcast(GlobalHelper.MeleeHit, this);

                return true;
            }

            //todo sound for missed attack

            message = $"Attack missed! Rolled: {roll} Needed: {chanceToHit}";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            return false;
        }

        public void AddHealth(int amount)
        {
            Stats.CurrentHealth += amount;
        }

        public void SubtractHealth(int amount)
        {
            Stats.CurrentHealth -= amount;
        }

        public void AddEnergy(int amount)
        {
            Stats.CurrentEnergy += amount;
        }

        public void SubtractEnergy(int amount)
        {
            Stats.CurrentEnergy -= amount;
        }

        public void AddMorale(int amount)
        {
            Stats.CurrentMorale += amount;
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

        public void UseHealthPotion()
        {
            AddHealth(40); //todo make a constant somewhere
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

using System.Collections.Generic;
using Assets.Scripts.Entities.Names;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;

namespace Assets.Scripts.Entities
{
    public class Entity : GameObject
    {
        private Attributes _attributes;
        private Race _race;
        private EntityClass _entityClass;
        private int _level;
        private int _xp;

        private List<Effect> _effects;

        private Weapon _equippedWeapon;
        private Armor _equippedArmor;

        private AiController _aiController;

        private List<Ability> _abilities;
        
        private int _lastTurnMoved;

        public string Name { get; set; }
        public Sex Sex { get; }
        public Stats Stats { get; }
        public Sprite Portrait { get; private set; }

        public UnityEngine.GameObject SpritePrefab { get; private set; }
        public UnityEngine.GameObject SpriteInstance { get; private set; }

        public Entity(bool isDerpus = false) : base((-1, -1), 1, null, false, false, true)
        {
            if (isDerpus)
            {
                _race = Race.Derpus;
                _entityClass = EntityClass.Derpus;
                Sex = Sex.Male;
                Name = "Derpus";
            }
            else
            {
                _race = PickRace();
                _entityClass = PickEntityClass();
                Sex = PickSex();
                Name = GenerateName(null, Sex);
            }

            _level = 1;
            _xp = 0;
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            SpriteInstance = instance;
        }

        public void SetSpritePosition(Vector3 newPosition)
        {
            if (SpriteInstance == null)
            {
                return;
            }

            SpriteInstance.transform.position = newPosition;
        }

        public bool IsPlayer()
        {
            return _aiController == null;
        }

        public bool IsDead()
        {
            return Stats.CurrentHealth <= 0;
        }

        public bool IsDerpus()
        {
            return _entityClass == EntityClass.Derpus || _race == Race.Derpus;
        }

        public bool MovedLastTurn(int currentTurn)
        {
            return currentTurn - _lastTurnMoved <= 1;
        }

        public void GenerateStartingEquipment()
        {
            //todo
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            //todo
        }

        public void UnEquipWeapon()
        {
            //todo
        }

        public void EquipArmor(Armor newArmor)
        {
            //todo
        }

        public void UnEquipArmor()
        {
            //todo
        }

        public void MeleeAttack()
        {

        }

        public void RangedAttack()
        {

        }

        public bool HasMissileWeaponEquipped()
        {
            //todo
            return true;
        }

        public bool TargetInRange(Entity target)
        {
            //todo
            return true;
        }

        public int CalculateChanceToHit(Entity target)
        {
            //todo
            return -1;
        }

        public int CalculateBaseChanceToHit(Entity target)
        {
            //todo
            return -1;
        }

        public bool AttackHit(int chanceToHit)
        {
            //todo
            return true;
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
            AddHealth(40);
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
            AddEnergy(45);
        }

        private string GenerateName(List<string> possibleNameFiles, Sex sex)
        {
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

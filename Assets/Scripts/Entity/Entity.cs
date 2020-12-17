using System.Collections.Generic;
using GoRogue;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;

namespace Assets.Scripts.Entity
{
    public class Entity : GameObject
    {
        private string _name;
        private Attributes _attributes;
        private Stats _stats;
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

        public Sprite Portrait { get; private set; }

        public UnityEngine.GameObject SpritePrefab { get; private set; }
        public UnityEngine.GameObject SpriteInstance { get; private set; }

        public Entity(string name) : base((-1, -1), 1, null, false, false, true)
        {
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            SpriteInstance = instance;
        }

        public void SetSpritePosition(Vector3 newPosition)
        {
            //todo
        }

        public bool IsPlayer()
        {
            return _aiController == null;
        }

        public bool IsDead()
        {
            return _stats.CurrentHealth <= 0;
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

        public void AddHp(int amount)
        {
            _stats.CurrentHealth += amount;
        }

        public void SubtractHp(int amount)
        {
            _stats.CurrentHealth -= amount;
        }

        public void AddEnergy(int amount)
        {
            _stats.CurrentEnergy += amount;
        }

        public void SubtractEnergy(int amount)
        {
            _stats.CurrentEnergy -= amount;
        }

        public void AddMorale(int amount)
        {
            _stats.CurrentMorale += amount;
        }

        public void SubtractMorale(int amount)
        {
            _stats.CurrentMorale -= amount;
        }

        public void AddAp(int amount)
        {
            _stats.CurrentActionPoints += amount;
        }

        public void SubtractAp(int amount)
        {
            _stats.CurrentActionPoints -= amount;
        }

        public void UseHealthPotion()
        {
            //todo
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
    }
}

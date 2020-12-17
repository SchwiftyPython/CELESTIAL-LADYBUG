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

        public bool IsDead()
        {
            return _stats.CurrentHealth <= 0;
        }

        public bool IsDerpus()
        {
            return _entityClass == EntityClass.Derpus;
        }

        public bool MovedLastTurn(int currentTurn)
        {
            return currentTurn - _lastTurnMoved <= 1;
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
    }
}

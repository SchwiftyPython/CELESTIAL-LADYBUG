using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using GoRogue.GameFramework;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public abstract class Ability 
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ApCost { get; private set; }
        public int Range { get; private set; }
        public Entity AbilityOwner { get; private set; }
        public bool HostileTargetsOnly { get; private set; }
        public bool IsPassive { get; private set; }
        public Sprite Icon { get; protected set; }
        public bool UsesEquipment { get; protected set; }

        protected Ability(string name, string description, int apCost, int range, Entity abilityOwner, bool hostileTargetsOnly, bool passive, bool usesEquipment = true)
        {
            Name = name;
            Description = description;
            ApCost = apCost;

            if (range < 0 && !passive)
            {
                var equippedWeapon = abilityOwner.GetEquippedWeapon();

                if (equippedWeapon == null)
                {
                    Range = -1;
                }
                else
                {
                    Range = equippedWeapon.GetRange();
                }
            }
            else
            {
                Range = range;
            }

            AbilityOwner = abilityOwner;
            HostileTargetsOnly = hostileTargetsOnly;
            IsPassive = passive;
            UsesEquipment = usesEquipment;

            GetIconForAbility(this);
        }

        public virtual void Use(Entity target)
        {
            //todo testing for prototype - assumes combat ability

            //todo message assumes combat ability
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            if (Range < 2)
            {
                AbilityOwner.AttackWithAbility(target, this);
            }
            else
            {
                AbilityOwner.RangedAttack(target);
            }

            AbilityOwner.SubtractActionPoints(ApCost);
        }

        public virtual int Use()
        {
            return 0;
        }

        public virtual void SetupForCombat()
        {
        }

        public bool TargetInRange(Entity target)
        {
            var distance = Distance.CHEBYSHEV.Calculate(AbilityOwner.Position, target.Position);

            return Range >= distance;
        }

        public bool TargetValid(Entity target)
        {
            //todo might need to have a boolean for if ability uses line of sight

            if (IsRanged() && !HasLineOfSight(target))
            {
                return false;
            }

            if (HostileTargetsOnly)
            {
                return AbilityOwner.IsPlayer() != target.IsPlayer();
            }

            return AbilityOwner.IsPlayer() == target.IsPlayer();
        }

        public bool IsRanged()
        {
            return Range > 1;
        }

        public virtual (int, int) GetAbilityDamageRange()
        {
            var combatManager = Object.FindObjectOfType<CombatManager>();

            int damageMin;
            int damageMax;
            if (IsRanged())
            {
                (damageMin, damageMax) = AbilityOwner.GetEquippedWeapon().GetRangedDamageRange();
            }
            else
            {
                (damageMin, damageMax) = AbilityOwner.GetEquippedWeapon().GetMeleeDamageRange();
            }

            return (damageMin, damageMax);
        }

        public virtual void Terminate()
        {
        }

        private bool HasLineOfSight(IGameObject target)
        {
            //todo need to allow for some objects to be shot around at a penalty, but some objects should completely block los

            var line = Lines.Get(AbilityOwner.Position, target.Position).ToList();

            var combatManager = Object.FindObjectOfType<CombatManager>();

            var map = combatManager.Map;

            foreach (var coord in line)
            {
                if (coord == line.First() || coord == line.Last())
                {
                    continue;
                }

                if (!map.GetTileAt(coord).IsWalkable)
                {
                    return false;
                }
            }

            return true;
        }

        private void GetIconForAbility(Ability ability)
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            Icon = spriteStore.GetAbilitySprite(ability);
        }
    }
}

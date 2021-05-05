using Assets.Scripts.Entities;
using GoRogue;
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

        protected Ability(string name, string description, int apCost, int range, Entity abilityOwner, bool hostileTargetsOnly, bool passive)
        {
            Name = name;
            Description = description;
            ApCost = apCost;
            Range = range;
            AbilityOwner = abilityOwner;
            HostileTargetsOnly = hostileTargetsOnly;
            IsPassive = passive;

            GetIconForAbility(this);
        }

        public virtual void Use(Entity target)
        {
            //todo testing for prototype - assumes combat ability

            //todo message assumes combat ability
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            //todo if range < 0 then use equipped item's range
            if (Range < 2)
            {
                AbilityOwner.MeleeAttack(target);
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

        public bool TargetInRange(Entity target)
        {
            var distance = Distance.CHEBYSHEV.Calculate(AbilityOwner.Position, target.Position);

            return Range >= distance;
        }

        public bool TargetValid(Entity target)
        {
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

        protected void GetIconForAbility(Ability ability)
        {
            Icon = SpriteStore.Instance.GetAbilitySprite(ability);
        }
    }
}

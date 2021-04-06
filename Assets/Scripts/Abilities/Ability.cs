using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Abilities
{
    public class Ability 
    {
        public string Name { get; private set; }
        public int ApCost { get; private set; }
        public int Range { get; private set; }
        public Entity AbilityOwner { get; private set; }
        public bool HostileTargetsOnly { get; private set; }

        public Ability(string name, int apCost, int range, Entity abilityOwner, bool hostileTargetsOnly)
        {
            Name = name;
            ApCost = apCost;
            Range = range;
            AbilityOwner = abilityOwner;
            HostileTargetsOnly = hostileTargetsOnly;
        }

        public void Use(Entity abilityOwner, Entity target)
        {
            //todo testing for prototype - assumes combat ability

            //todo message assumes combat ability
            var message = $"{abilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            //todo either we change this check or we give a weapon a "reach" property and check for that
            //for spears for example that could melee attack 2 squares away
            if (Range < 2)
            {
                abilityOwner.MeleeAttack(target);
            }
            else
            {
                abilityOwner.RangedAttack(target);
            }

            abilityOwner.SubtractActionPoints(ApCost);
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
    }
}

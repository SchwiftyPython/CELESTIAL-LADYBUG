using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Abilities
{
    public class Ability 
    {
        //todo might need ability owner property

        public string Name { get; private set; }
        public int ApCost { get; private set; }
        public int Range { get; private set; }

        public Ability(string name, int apCost, int range)
        {
            Name = name;
            ApCost = apCost;
            Range = range;
        }

        public void Use(Entity abilityOwner, Entity target)
        {
            //todo testing for prototype - assumes combat ability

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

        public bool TargetInRange(Entity abilityOwner, Entity target)
        {
            var distance = Distance.CHEBYSHEV.Calculate(abilityOwner.Position, target.Position);

            return Range >= distance;
        }
    }
}

namespace Assets.Scripts.Abilities
{
    public class Ability 
    {
        public string Name { get; private set; }
        public int ApCost { get; private set; }
        public int Range { get; private set; }

        public Ability(string name, int apCost, int range)
        {
            Name = name;
            ApCost = apCost;
            Range = range;
        }
    }
}

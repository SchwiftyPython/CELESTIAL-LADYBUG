namespace Assets.Scripts.Items
{
    public class Weapon : Item
    {
        public (int, int) DamageRange { get; private set; }
        public int Range { get; private set; }
        public bool IsRanged { get; private set; }

        public Weapon(string itemName, ItemType type, (int, int) damageRange, int range) : base(itemName, type)
        {
            DamageRange = damageRange;
            Range = range;
            IsRanged = Range > 1;
        }
    }
}

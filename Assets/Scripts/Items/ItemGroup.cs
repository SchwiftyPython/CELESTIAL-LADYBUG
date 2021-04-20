using System.ComponentModel;

namespace Assets.Scripts.Items
{
    public enum ItemGroup 
    {
        [Description("Robe")] Robe,
        [Description("Armor")] Armor,
        [Description("Feet")] Feet,
        [Description("Glove")] Glove,
        [Description("Helmet")] Helmet,
        [Description("Ring")] Ring,
        [Description("Shield")] Shield,
        [Description("Axe")] Axe,
        [Description("Crossbow")] Crossbow,
        [Description("Dagger")] Dagger,
        [Description("Spear")] OneHandedSpear,
        [Description("Spear")] TwoHandedSpear,
        [Description("Book")] Book,
        [Description("Sword")] OneHandedSword,
        [Description("Sword")] TwoHandedSword
    }
}

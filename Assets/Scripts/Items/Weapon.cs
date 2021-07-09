using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Weapon
    {
        public (int, int) DamageRange { get; private set; }
        public int Range { get; private set; }
        public bool IsRanged { get; private set; }

        
    }
}

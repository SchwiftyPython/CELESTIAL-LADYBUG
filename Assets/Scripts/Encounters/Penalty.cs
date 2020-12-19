using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Encounters
{
    public class Penalty
    {
        public Dictionary<Entity, KeyValuePair<object, int>> EntityLosses;
        public Dictionary<object, int> PartyLosses;
        public List<Effect> Effects;

        public Penalty(Dictionary<Entity, KeyValuePair<object, int>> entityLosses, Dictionary<object, int> partyLosses, List<Effect> effects)
        {
            EntityLosses = entityLosses;
            PartyLosses = partyLosses;
            Effects = effects;
        }

        public Penalty(Dictionary<Entity, KeyValuePair<object, int>> entityLosses)
        {
            EntityLosses = entityLosses;
        }
    }
}

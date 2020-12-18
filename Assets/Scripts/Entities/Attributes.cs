using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Attributes
    {
        private const int AttributeMin = 1;
        private const int AttributeMax = 10;

        public int Might { get; set; }
        public int Speed { get; set; }
        public int Intellect { get; set; }


        public Attributes()
        {
            GenerateAttributeValues();
        }

        public Attributes(int might, int speed, int intellect)
        {
            Might = might;
            Speed = speed;
            Intellect = intellect;
        }

        private void GenerateAttributeValues()
        {
            Might = GenerateAttributeValue();
            Speed = GenerateAttributeValue();
            Intellect = GenerateAttributeValue();
        }

        private int GenerateAttributeValue()
        {
            return Random.Range(AttributeMin, AttributeMax + 1);
        }
    }
}

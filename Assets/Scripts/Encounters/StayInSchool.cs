namespace Assets.Scripts.Encounters
{
    public class StayInSchool : Encounter
    {
        public StayInSchool()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Stay In School Kids";
            Description =
                "The party takes a break on the side of the trail. A rather handsome woman approaches them and explains she is a teacher at the local school. She wonders if they could speak to the children and encourage them to stay in school. In return, she offers a little food.";

            //todo options
        }

        public override void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}

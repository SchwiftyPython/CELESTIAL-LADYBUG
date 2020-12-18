namespace Assets.Scripts.Encounters
{
    public enum EncounterType 
    {
        Normal,
        Crossroads,
        Trading,
        Camping,
        Combat,
        MentalBreak, //Morale < 0, add to top of deck
        Continuity
    }
}

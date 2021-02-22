namespace Assets.Scripts.Encounters
{
    public class Option
    {
        public string Name { get; set; }
        public string ResultText { get; set; }
        public Reward Reward { get; set; }
        public Penalty Penalty { get; set; }
        public EncounterType TargetEncounterType { get; set; }

        public Option() {}

        public Option(string name, string resultText, Reward reward, Penalty penalty, EncounterType targetEncounterType)
        {
            Name = name;
            ResultText = resultText;
            Reward = reward;
            Penalty = penalty;
            TargetEncounterType = targetEncounterType;
        }

        public Option(string optionTitle, string optionResultText, EncounterType targetEncounterType)
        {
            Name = optionTitle;
            ResultText = optionResultText;
            TargetEncounterType = targetEncounterType;
        }

        public bool HasReward()
        {
            return Reward != null;
        }

        public bool HasPenalty()
        {
            return Penalty != null;
        }
    }
}

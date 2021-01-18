namespace Assets.Scripts.Encounters
{
    public class Option
    {
        public string Name { get; set; }
        public string ResultText { get; set; }
        public Reward Reward { get; set; }
        public Penalty Penalty { get; set; }

        public Option() {}

        public Option(string name, string resultText, Reward reward, Penalty penalty)
        {
            Name = name;
            ResultText = resultText;
            Reward = reward;
            Penalty = penalty;
        }

        public Option(string optionTitle, string optionResultText)
        {
            Name = optionTitle;
            ResultText = optionResultText;
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

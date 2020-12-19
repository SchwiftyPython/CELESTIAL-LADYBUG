namespace Assets.Scripts.Encounters
{
    public class Option
    {
        public string Name { get; set; }
        public string ResultText { get; set; }
        public Reward Reward { get; set; }
        public Penalty Penalty { get; set; }

        public Option(string name, string resultText, Reward reward, Penalty penalty)
        {
            Name = name;
            ResultText = resultText;
            Reward = reward;
            Penalty = penalty;
        }
    }
}

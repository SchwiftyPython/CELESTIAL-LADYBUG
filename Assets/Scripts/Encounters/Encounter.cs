using System.Collections.Generic;

namespace Assets.Scripts.Encounters
{
    public abstract class Encounter: ISubscriber
    {
        private const string OptionSelectedEvent = GlobalHelper.EncounterOptionSelected;

        public Rarity Rarity;

        public static EncounterType EncounterType;

        public string Title;

        public string Description;

        public Dictionary<string, Option> Options;

        //public abstract void Run();

        //todo figure out optional params in abstract methods so resharper fucks off
        public abstract void Run(Option selectedOption = null);

        public bool HasOptions()
        {
            return Options != null && Options.Count > 0;
        }

        protected void SubscribeToOptionSelectedEvent()
        {
            EventMediator.Instance.SubscribeToEvent(OptionSelectedEvent, this);
        }

        protected void UnsubscribeFromOptionSelectedEvent()
        {
            EventMediator.Instance.UnsubscribeFromEvent(OptionSelectedEvent, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(OptionSelectedEvent))
            {
                if (!HasOptions())
                {
                    return;
                }

                var optionName = parameter.ToString();

                if (!Options.ContainsKey(optionName))
                {
                    //todo maybe throw exception
                    return;
                }

                Run(Options[optionName]);
            }
        }
    }
}

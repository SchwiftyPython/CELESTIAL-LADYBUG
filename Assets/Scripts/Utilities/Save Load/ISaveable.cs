namespace Assets.Scripts.Utilities.Save_Load
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
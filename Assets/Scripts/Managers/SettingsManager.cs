using Zenject;

namespace Dices.UIConnection // Class for contein settings
{
    public class SettingsManager : IInitializable 
    {
        public int DicesAmount;
        public int StopTimer;
        public int CurrentTimerValue;
        public bool IsAnimated;
        public bool DetailsShow;
        public bool IsVoited;
        public bool IsHistoryShow;
        public enum StopMode { Manual, Automatic, OnTimer };
        public StopMode CurrentStop;

        public void Initialize()
        {
        }
    }
}

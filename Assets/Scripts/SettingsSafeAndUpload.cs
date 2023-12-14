using UnityEngine;
using Zenject;
using Doozy.Engine.Soundy;

namespace Dices.UIConnection
{
    public class SettingsSafeAndUpload : MonoBehaviour // Class for save and get settings from PlayerPrefs in register
    {
        [Inject]
        private SettingsManager _settinsManager;

        [SerializeField]
        private string IsAnimated;
        [SerializeField]
        private string DetailsShow;
        [SerializeField]
        private string IsVoited;
        [SerializeField]
        private string IsHistoryShow;
        [SerializeField]
        private string CurrentStop;
        [SerializeField]
        private SoundyData _soundData;

        public void SaveSettings()
        {
            #region PlayerPrefs.Set***
            PlayerPrefs.SetInt("DicesAmount", _settinsManager.DicesAmount);
            PlayerPrefs.SetInt("StopTimer", _settinsManager.StopTimer);
            PlayerPrefs.SetInt("CurrentTimerValue", _settinsManager.CurrentTimerValue);

            ConvertForSaveSettings();
            PlayerPrefs.SetString("IsAnimated", IsAnimated);
            PlayerPrefs.SetString("DetailsShow", DetailsShow);
            PlayerPrefs.SetString("IsVoited", IsVoited);
            PlayerPrefs.SetString("IsHistoryShow", IsHistoryShow);

            PlayerPrefs.SetString("CurrentStop", CurrentStop);
            #endregion
//            Debug.Log("Settings uploaded to Windows Registry");
        }


        void Awake()
        {
            SoundyManager.Play(_soundData);
            DownloadSettings();
        }

        void DownloadSettings()
        {
            #region PlayerPrefs.Get***
            _settinsManager.DicesAmount = PlayerPrefs.GetInt("DicesAmount");
            _settinsManager.StopTimer = PlayerPrefs.GetInt("StopTimer");
            _settinsManager.CurrentTimerValue = PlayerPrefs.GetInt("CurrentTimerValue");

            IsAnimated = PlayerPrefs.GetString("IsAnimated");
            DetailsShow = PlayerPrefs.GetString("DetailsShow");
            IsVoited = PlayerPrefs.GetString("IsVoited");
            IsHistoryShow = PlayerPrefs.GetString("IsHistoryShow");

            CurrentStop = PlayerPrefs.GetString("CurrentStop");

            ConvertFromDownloadSettings();
            #endregion
//            Debug.Log("Settings downloaded from Windows Registry");
        }

        void ConvertForSaveSettings()
        {
        IsAnimated = _settinsManager.IsAnimated.ToString();
        DetailsShow = _settinsManager.DetailsShow.ToString();
        IsVoited = _settinsManager.IsVoited.ToString();
        IsHistoryShow = _settinsManager.IsHistoryShow.ToString();

        CurrentStop = _settinsManager.CurrentStop.ToString();
        }

        void ConvertFromDownloadSettings()
        {
            _settinsManager.IsAnimated = bool.Parse(IsAnimated);
            _settinsManager.DetailsShow = bool.Parse(DetailsShow);
            _settinsManager.IsVoited = bool.Parse(IsVoited);
            _settinsManager.IsHistoryShow = bool.Parse(IsHistoryShow);

            _settinsManager.CurrentStop = (SettingsManager.StopMode)System.Enum.Parse(typeof(SettingsManager.StopMode), CurrentStop, true);
        }


    }
}

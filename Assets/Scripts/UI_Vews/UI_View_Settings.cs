using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Dices.UIConnection;
using Zenject;
using Doozy.Engine;
using Doozy.Engine.Soundy;


namespace Dices.UserInterface // Class for settings canvas
{
    public class UI_View_Settings : MonoBehaviour
    {
        [Inject]
        SettingsManager _settingsManager;

        [Inject]
        ScoreManager _scoreManager;

        [SerializeField]
        private Slider _diceAmountSlider;
        [SerializeField]
        private InputField _diceAmountField;
        [SerializeField]
        private TMP_Dropdown _stopModeSelect;
        [SerializeField]
        private GameObject _stopModePanel;
        [SerializeField]
        private Toggle _useAnimation;
        [SerializeField]
        private Toggle _historyShow;
        [SerializeField]
        private Toggle _scoreDetails;
        [SerializeField]
        private Slider _timerSlider;
        [SerializeField]
        private InputField _timerField;
        [SerializeField]
        private GameObject _timerPanel;
        [SerializeField]
        private bool _isVoited;
        [SerializeField]
        private GameObject _soundCyrcle;
        [SerializeField]
        private SoundyManager AudioMixer;
        [SerializeField]
        private GridLayoutGroup _middleGridLayoutGroup;


        void Start()
        {
            ApplySavedSettings();
            _scoreManager.SpawnBySwipe = false;
        }

        private void OnEnable()
        {
            ChangeCanvasOrientation();
        }

        void ApplySavedSettings()
        {
            _diceAmountSlider.value = _settingsManager.DicesAmount;
            _diceAmountField.text = _settingsManager.DicesAmount.ToString();
            SwichMode();
            _useAnimation.isOn = _settingsManager.IsAnimated;
            _historyShow.isOn = _settingsManager.IsHistoryShow;
            _scoreDetails.isOn = _settingsManager.DetailsShow;
            _timerSlider.value = _settingsManager.StopTimer;
            _isVoited = !(_settingsManager.IsVoited);
            SwichSound();
        }

        void SwichMode()
        {
            SettingsManager.StopMode _currentStopMode = _settingsManager.CurrentStop;
            switch (_settingsManager.CurrentStop)
            {
                case SettingsManager.StopMode.Manual:
                    _stopModeSelect.value = 0;
                    _timerPanel.SetActive(false);
                    break;
                case SettingsManager.StopMode.Automatic:
                    _stopModeSelect.value = 1;
                    _timerPanel.SetActive(false);
                    break;
                case SettingsManager.StopMode.OnTimer:
                    _stopModeSelect.value = 2;
                    _timerPanel.SetActive(true);
                    break;
                default:
                    _stopModeSelect.value = 1;
                    _timerPanel.SetActive(false);

                    break;
            }
        }

        public void SwichSound()
        {
            if (_isVoited == false)
                {
                GameEventMessage.SendEvent(EventsLibrary.SoundON);
                _isVoited = true;
                AudioMixer.enabled = true;
            }
            else
            {
                GameEventMessage.SendEvent(EventsLibrary.SoundOff);
                _isVoited = false;
            }
            AudioMixer.gameObject.SetActive(_isVoited);
            _soundCyrcle.SetActive(!_isVoited);
            _settingsManager.IsVoited = _isVoited;
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }
        public void ChangeStopMode()
        {
            switch (_stopModeSelect.value)
            {
                case 0:
                    _settingsManager.CurrentStop = SettingsManager.StopMode.Manual;
                    _timerPanel.SetActive(false);
                    break;
                case 1:
                    _settingsManager.CurrentStop = SettingsManager.StopMode.Automatic;
                    _timerPanel.SetActive(false);
                    break;
                case 2:
                    _settingsManager.CurrentStop = SettingsManager.StopMode.OnTimer;
                    _timerPanel.SetActive(true);
        break;
                default:
                    _settingsManager.CurrentStop = SettingsManager.StopMode.Automatic;
                    _timerPanel.SetActive(false);

                    break;
            }
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeDetailShow()
        {
            _settingsManager.DetailsShow = _scoreDetails.isOn;
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeDicesAmountByField()
        {
            _diceAmountField.text = _diceAmountSlider.value.ToString();
            _settingsManager.DicesAmount = Convert.ToInt32(_diceAmountSlider.value);
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeSlider()
        {
            if (Input.GetKeyDown(KeyCode.Backspace) != true)
            {
                _diceAmountSlider.value = Int32.Parse(_diceAmountField.text.ToString());
                _settingsManager.DicesAmount = Int32.Parse(_diceAmountField.text.ToString());
                GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
            }
        }

        public void LessDiceAmount()
        {
            if (_settingsManager.DicesAmount > 1)
            {
                _settingsManager.DicesAmount--;
                ChangeDiceAmount();
            }
        }

        public void MoreDiceAmount()
        {
            if (_settingsManager.DicesAmount < 10)
            {
                _settingsManager.DicesAmount++;
                ChangeDiceAmount();
            }
        }

        void ChangeDiceAmount()
        {
            _diceAmountSlider.value = _settingsManager.DicesAmount;
            _diceAmountField.text = _settingsManager.DicesAmount.ToString();
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeAnimUsing()
        {
            _settingsManager.IsAnimated = _useAnimation.isOn;
            _stopModePanel.SetActive(_useAnimation.isOn);
            TimePanelShow();
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }
        public void ChangeHistoryShow()
        {
            _settingsManager.IsHistoryShow = _historyShow.isOn;
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
            GameEventMessage.SendEvent(EventsLibrary.ScoreHistoryWritten);
            int i = 0;
            foreach (int _scoreHistory in _scoreManager.ScoreHistory)
            {
                _scoreManager.ScoreHistory[i] = 0;
                i++;
            }
        }

    void TimePanelShow()
        {
            if ((_useAnimation.isOn == true) & (_settingsManager.CurrentStop == SettingsManager.StopMode.OnTimer))
            {
                _timerPanel.SetActive(true);
            }
            else
            {
                _timerPanel.SetActive(false);
            }
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void StartGame()
        {
            GameEventMessage.SendEvent(EventsLibrary.Spawn);
        }


        public void ChangeTimerField()
        {
            _timerField.text = _timerSlider.value.ToString();
            _settingsManager.StopTimer = Convert.ToInt32(_timerSlider.value);
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeTimerSlider()
        {
            if (Input.GetKeyDown(KeyCode.Backspace) != true)
            {
                _timerSlider.value = Int32.Parse(_timerField.text.ToString());
                _settingsManager.StopTimer = Int32.Parse(_timerField.text.ToString());
            }
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void LessTimeAmount()
        {
            if (_settingsManager.StopTimer > 0)
            {
                _settingsManager.StopTimer--;
                ChangeTimer();
            }
        }

        public void MoreTimeAmount()
        {
             if (_settingsManager.StopTimer < 60)
            {
                _settingsManager.StopTimer++;
                ChangeTimer();
            }
        }

        void ChangeTimer()
        {
            _timerSlider.value = _settingsManager.StopTimer;
            _timerField.text = _settingsManager.StopTimer.ToString();
            GameEventMessage.SendEvent(EventsLibrary.SettingsChanged);
        }

        public void ChangeCanvasOrientation()
        {
            bool _isOrientationPortret = (Screen.currentResolution.height > Screen.currentResolution.width);

            if (_isOrientationPortret == false)
            {
                _middleGridLayoutGroup.cellSize = new Vector2(300, 300);
                _middleGridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                _middleGridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
            }
            else
            {
                _middleGridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                _middleGridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                _middleGridLayoutGroup.constraintCount = 1;
            }
        }
    }
}

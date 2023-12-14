using UnityEngine;
using Zenject;
using Dices.UIConnection;
using TMPro;
using Doozy.Engine;
using UniRx;

namespace Dices.UserInterface // Class for conteiner on full play canvas
{
    public class UI_Drawer_Container : MonoBehaviour
    {
        [Inject]
        SettingsManager _settingsManager;
        [Inject]
        ScoreManager _scoreManager;
        [SerializeField]
        private GameObject _rerollButton;
        [SerializeField]
        private GameObject _stopeRotateButton;
        [SerializeField]
        private TMP_Text[] _scoreDetails;
        [SerializeField]
        private GameObject _scoreDetail;
        [SerializeField]
        private TMP_Text _scoreLabel;
        [SerializeField]
        private TMP_Text _timeLabel;
        [SerializeField]
        private bool _stopRotateButtonActive;
        [SerializeField]
        private GameObject _timerLabel;
        [SerializeField]
        private GameObject[] _scoreHistorySlot;
        [SerializeField]
        private TMP_Text[] _scoreHistoryLabels;
        [SerializeField]
        private GameObject _scoreHistory;
        [SerializeField]
        private int _timer;
        private CompositeDisposable _disposable = new CompositeDisposable();


        private void OnEnable()
        {
            _scoreManager.SpawnBySwipe = false;
            _scoreDetail.SetActive(_settingsManager.DetailsShow);
            _scoreHistory.SetActive(_settingsManager.IsHistoryShow);
            ShowScoreHistory();
            GetSettingsForButton();
            bool y;
                if ((_stopRotateButtonActive == true) && (_scoreManager.IsSpawned == true))
            {
                y = true;
            }
                else
            {
                y = false;
            }
            ScoreShow();
            SwichShowTimer();
            SetActiveButtons(y);
            GamePlay.DiceSpawnCS.CurrentTimer.Subscribe(
                _ => ShowCurrentTimer()).AddTo(_disposable);
        }

        void OnDisable()
        {
            _disposable.Clear();
        }

        void ShowCurrentTimer()
        {
            _timeLabel.text = GamePlay.DiceSpawnCS.CurrentTimer.Value.ToString();
        }

        public void ShowScoreHistory()
        {
            int i = 0;
            foreach (TMP_Text ScoreHistoryLabel in _scoreHistoryLabels)
            {
                int _score = _scoreManager.ScoreHistory[i];
                if (_score != 0)
                {
                    _scoreHistorySlot[i].SetActive(true);
                    ScoreHistoryLabel.text = _score.ToString();
                }
                else
                {
                    _scoreHistorySlot[i].SetActive(false);
                }
                i++;
            }
        }

        public void AddDiceButtonHendler()
        {
            if (_settingsManager.DicesAmount < 10)
            {
                Time.timeScale = 5;
                _settingsManager.DicesAmount++;
                GameEventMessage.SendEvent(EventsLibrary.AddDice);
                SetActiveButtons(_stopRotateButtonActive);
            }
        }

        void SwichShowTimer()
        {
            bool _isOnTimer;
            if ((_settingsManager.IsAnimated == true) & (_settingsManager.CurrentStop == SettingsManager.StopMode.OnTimer))
            {
                _timerLabel.SetActive(true);
                _timer = _settingsManager.StopTimer;
                _isOnTimer  = true;
                _timeLabel.text = _timer.ToString();
            }
            else
            {
                _isOnTimer = false;
            }
            _timerLabel.SetActive(_isOnTimer);
        }

        public void SwipeReroll()
        {
            GetSettingsForButton();
            if ((_scoreManager.IsSpawned == true)&(_stopRotateButtonActive == true))
            {
                StopRotate();
            }
            else
            {
                _scoreManager.SpawnBySwipe = true;
                Reroll();
            }
            SetActiveButtons(_stopRotateButtonActive);
        }

        public void Reroll()
        {

            if (_settingsManager.CurrentStop == SettingsManager.StopMode.OnTimer)
            {
                int _timer = _settingsManager.StopTimer;
            }

            GameEventMessage.SendEvent(EventsLibrary.Spawn);
            _scoreManager.IsSpawned = true;
            _scoreManager.SpawnBySwipe = false;
            SetActiveButtons(_stopRotateButtonActive);
        }

        public void StopRotate()
        {
            _scoreManager.SpawnBySwipe = false;
            GameEventMessage.SendEvent(EventsLibrary.StopRotate);
            _scoreManager.IsSpawned = false;
            SetActiveButtons(!_stopRotateButtonActive);
            Debug.Log("Rotate stoped!!!");
        }

        void GetSettingsForButton()
        {
            if ((_settingsManager.CurrentStop == SettingsManager.StopMode.Manual) & (_settingsManager.IsAnimated == true))
            {
                _stopRotateButtonActive = true;
            }
            else
            {
                _stopRotateButtonActive = false;

            }
        }

        void SetActiveButtons(bool Swichable)
        {
            _rerollButton.SetActive(!Swichable);
            _stopeRotateButton.SetActive(Swichable);
        }

        public void ScoreShow()
        {
            _scoreLabel.text = _scoreManager.Score.ToString();
            ShowDetails();
        }

        void ShowDetails()
        {
            int i = 0;
            foreach (TMP_Text _detail in _scoreDetails)
            {
               _detail.text = _scoreManager.ScoreDetales[i].ToString();
               i++;
            }
        }
    }
}

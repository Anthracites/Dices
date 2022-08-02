using UnityEngine;
using Doozy.Engine;
using Zenject;
using Dices.UIConnection;

namespace Dices.UserInterface //Class for short canvas play
{
    public class BigRerollButton : MonoBehaviour
    {
        [Inject]
        SettingsManager _settingsManager;
        [Inject]
        ScoreManager _scoreManager;

        [SerializeField]
        private bool _stopRotateButtonActiveS;

        private void OnEnable()
        {
            if ((_settingsManager.CurrentStop == SettingsManager.StopMode.Manual) & (_settingsManager.IsAnimated == true))
            {
                _stopRotateButtonActiveS = true;
            }
            else
            {
                _stopRotateButtonActiveS = false;

            }
        }
        public void OnClick()
        {
            if ((_scoreManager.IsSpawned == true) & (_stopRotateButtonActiveS == true))
            {
                StopRotate();
            }
            else
            {
                Reroll();
            }
        }

        void StopRotate()
        {
            GameEventMessage.SendEvent(EventsLibrary.StopRotate);
            _scoreManager.IsSpawned = false; 
        }

        void Reroll()
        {
            GameEventMessage.SendEvent(EventsLibrary.Spawn);
            _scoreManager.IsSpawned = true;
        }
    }
}

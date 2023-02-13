using System.Collections;
using UnityEngine;
using Zenject;
using Dices.UIConnection;
using Doozy.Engine;
using Doozy.Engine.Soundy;
using UniRx;

namespace Dices.GamePlay
{
    public class DiceSpawnCS : MonoBehaviour // Dices spawner on scene
    {

        [Inject]
        private SettingsManager _settingsManager;
        [Inject]
        private ScoreManager _scoreManager;

        [SerializeField]
        private int _dicesNumber;
        [SerializeField]
        private float _firstSpawnPoint;
        [SerializeField]
        private float _distance;
        [SerializeField]
        private GameObject _dicePref;
        [SerializeField]
        private bool _isAnimated;
        [SerializeField]
        private SettingsManager.StopMode _stopMode;
        public static ReactiveProperty<int> CurrentTimer = new ReactiveProperty<int>();
        [SerializeField]
        private SoundyData _soundData;

        public void SpawnDice()
        {
            _distance = _dicePref.transform.localScale.x;
            GetSettings();

            if (_isAnimated == true)
            {
                AnimSpawnDice();
            }
            else
            {
                NoAnimSpawnDice();
            }
            _scoreManager.IsSpawned = true;
            GameEventMessage.SendEvent(EventsLibrary.DisecSpawned);

            if (_stopMode == SettingsManager.StopMode.OnTimer)
            {
                StartCoroutine(StopByTimer());
            }

        }

        public IEnumerator StopByTimer()
        {
            CurrentTimer.Value = _settingsManager.StopTimer;
            while (CurrentTimer.Value > 0)
            {
                yield return new WaitForSeconds(1);
                CurrentTimer.Value--;
            }

            SoundyManager.Play(_soundData);
            GameEventMessage.SendEvent(EventsLibrary.StopRotate);

        }


        void AnimSpawnDice()
        {
            StopAllCoroutines();
            MadeFirstSpawnPoint();
            int i = 0;
            while (i < _dicesNumber)
            {
                float coordX = (_firstSpawnPoint + i) * _distance * (Mathf.Pow(-1, i + 1));
                float coordY = 10f;
                float coordZ = (-(_firstSpawnPoint * (Mathf.Pow(-1, i)) * (_distance / _dicesNumber) * _distance));
                Vector3 SpawnPosition = new Vector3(coordZ, coordY, coordX);
                Quaternion spawnRotation = new Quaternion(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f));
                GameObject inst_obj = Instantiate(_dicePref, SpawnPosition, spawnRotation);
                inst_obj.name += i.ToString();

                if (_stopMode == SettingsManager.StopMode.OnTimer)
                {
                    inst_obj.GetComponent<DiceRotation>().IsStopByTimer = true;
                }
                inst_obj.transform.parent = gameObject.transform;
                i++;
            }
            if (_stopMode == SettingsManager.StopMode.Automatic)
            {
                GameEventMessage.SendEvent(EventsLibrary.StopRotate);
            }
        }

        void GetSettings()
        {
            _dicesNumber = _settingsManager.DicesAmount;
            _isAnimated = _settingsManager.IsAnimated;
            _stopMode = _settingsManager.CurrentStop;
        }

        void NoAnimSpawnDice()
        {
            MadeFirstSpawnPoint();
            int i = 0;
            while (i < _dicesNumber)
            {
                float coordX = (_firstSpawnPoint + i) * _distance * (Mathf.Pow(-1, i + 1));
                float coordY = 1f;
                float coordZ = (-(_firstSpawnPoint * (Mathf.Pow(-1, i)) * (_distance / _dicesNumber) * _distance));
                Vector3 SpawnPosition = new Vector3(coordZ, coordY, coordX);
                float[] _angles = { -180, -90, 0f, 90, 180};
                int A = Random.Range(0, _angles.Length - 1);
                int B = Random.Range(0, _angles.Length - 1);
                int C = Random.Range(0, _angles.Length - 1);
                Quaternion spawnRotation = Quaternion.Euler(_angles[A], _angles[B], _angles[C]);
                GameObject inst_obj = Instantiate(_dicePref, SpawnPosition, spawnRotation);
                inst_obj.GetComponent<DiceRotation>().IsStoded = true;
                inst_obj.transform.parent = gameObject.transform;
                i++;
            }
        }

        void MadeFirstSpawnPoint()

        {
            _firstSpawnPoint = -((_dicesNumber / 2));
        }

    }
}

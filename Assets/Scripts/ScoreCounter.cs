using System;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Dices.UIConnection;

namespace Dices.UserInterface
{
    public class ScoreCounter : MonoBehaviour // Class for count score of dices
    {
        [Inject]
        ScoreManager _scoreManager;
        [Inject]
        SettingsManager _settingsManager;

        [SerializeField]
        public int _score = 0;
        [SerializeField]
        private int dicesAmount, falledDicesAmount;
        [SerializeField]
        private GameObject[] _countCubes;


        //void OnTriggerEnter(Collider other)
        //{
        //    if (other.tag == "ScoreCube")
        //    {
        //        int m = (Int32.Parse(other.name));
        //        _score += m;
        //        _scoreManager.Score = _score;
        //        _scoreManager.ScoreDetales[m - 1]++;
        //    }
        //    GameEventMessage.SendEvent(EventsLibrary.ScoreChanged);
        //}

        //void OnTriggerExit(Collider other)
        //{
        //    if (other.tag == "ScoreCube")
        //    {
        //        int m = (Int32.Parse(other.name));
        //        _score -= m;
        //        _scoreManager.Score = _score;
        //        _scoreManager.ScoreDetales[m - 1]--;
        //    }
        //    GameEventMessage.SendEvent(EventsLibrary.ScoreChanged);
        //}

        public void CountFalledDices()
        {
            falledDicesAmount++;

            if (dicesAmount == falledDicesAmount)
            {
                NoAnimCubeCount();
            }
        }

         void NoAnimCubeCount()
        {
            _countCubes = GameObject.FindGameObjectsWithTag("ScoreCube");
            foreach (GameObject CountCube in _countCubes)
            {
                if (gameObject.GetComponent<Collider>().bounds.Intersects(CountCube.GetComponent<Collider>().bounds))
                {
                    _score += Int32.Parse(CountCube.name);
                    int m = Int32.Parse(CountCube.name);
                    _scoreManager.Score = _score;
                    _scoreManager.ScoreDetales[m - 1]++;
                }
                Destroy(CountCube);
            }
            GameEventMessage.SendEvent(EventsLibrary.ScoreChanged);
        }


        public void ScoreCleaner()
        {
            falledDicesAmount = 0;
            _score = 0;
            _scoreManager.Score = 0;
            int i = 0;
            foreach (int scoreDetail in _scoreManager.ScoreDetales)
            {
                _scoreManager.ScoreDetales[i] = 0;
                i++;
            }
//            Debug.Log("ScoreCleared");
            if (_settingsManager.IsAnimated == false)
            {
                NoAnimCubeCount();
            }
            GameEventMessage.SendEvent(EventsLibrary.ScoreChanged);
            dicesAmount = _settingsManager.DicesAmount;
        }

        public void WriteHistory()
        {
            if (_score != 0)
            {
                int[] _exScores = _scoreManager.ScoreHistory;
                int[] _currScores = new int[10];
                int i = 0;
                foreach (int exScore in _exScores)
                {
                    if (i == 0)
                    {
                        _currScores[i] = _score;
                    }
                    else
                    {
                        _currScores[i] = _exScores[i - 1];
                    }
                    i++;
                }
                _scoreManager.ScoreHistory = _currScores;
                GameEventMessage.SendEvent(EventsLibrary.ScoreHistoryWritten);
                ScoreCleaner();
//                Debug.Log("HistoryWritten");
            }

        }
    }
}

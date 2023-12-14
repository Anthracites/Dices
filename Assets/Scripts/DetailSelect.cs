using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Doozy.Engine;


namespace Dices.UserInterface
{
    public class DetailSelect : MonoBehaviour, IPointerClickHandler
    {
        [Inject]
        UIConnection.ScoreManager _scoreManager;

        [SerializeField]
        private GameObject detailFrame;
        [SerializeField]
        private int _detailNumber;

        private bool isSelected;

        void Start() // прописать загрузку из настроек
        {
            isSelected = false;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            SwichDetail();
        }

        void SwichDetail()
        {
            isSelected = !isSelected;
            _scoreManager.SelectedScores[_detailNumber-1] = isSelected;
            detailFrame.SetActive(isSelected);
            GameEventMessage.SendEvent(EventsLibrary.DetailSelected);

        }
    }
}

using System.Collections;
using UnityEngine;
using Doozy.Engine;
using UnityEngine.UI;
using Zenject;
using Dices.UIConnection;


namespace Dices.UserInterface // Class for settings canvas
{
    public class UIView_SuccessfulDonation : MonoBehaviour
    {
        [Inject]
        DonationManager _donationManager;

        [SerializeField]
        private GameObject _tapText, _thankYouText;
        [SerializeField]
        private RenderTexture _renderTexture;
        [SerializeField]
        private RawImage _rawImage;
        [SerializeField]
        private int _donationAmount;


        public void ContinueAfterDonationShow()
        {
            GameEventMessage.SendEvent(EventsLibrary.DonationShowStoped);
        }

        private void OnEnable()
        {
            float _currentTime = 0;
            _donationAmount = _donationManager.DonationAmount;
            if (_donationAmount > 4)
            {
               _currentTime = 4.6f;
            }
            StartCoroutine(TYShow(_currentTime));

        }

        public void ResizeTexture()
        {
            int _height = Screen.currentResolution.height;
            int _width = Screen.currentResolution.width;

            _renderTexture.height = _height;
            _renderTexture.width = _width;
        }

        IEnumerator Flash()
        {
            bool _isActive = true;
            while (true)
            {
                _tapText.SetActive(_isActive);
                _isActive = !_isActive;
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator TYShow(float _time)
        {
            yield return new WaitForSeconds(_time);
            _thankYouText.SetActive(true);
            _tapText.SetActive(true);
            StartCoroutine(Flash());
        }
        private void OnDisable()
        {
            _thankYouText.SetActive(false);
            StopAllCoroutines();
        }
    }
}


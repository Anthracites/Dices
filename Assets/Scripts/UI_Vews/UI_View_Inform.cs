using UnityEngine;
using Doozy.Engine.UI;
using UnityPackages.UI;

namespace Dices.UserInterface
{
    public class UI_View_Inform : MonoBehaviour
    {
        [SerializeField]
        private string _emailLink = "mailto:dicesapp@gmail.com", _storeLink = "https://play.google.com/store/apps/details?id=com.AnthraciteStrixdev.Dices", _privacyPolicy = "https://dicesapp.blogspot.com/2022/07/privacy-policy-for-dices.html";
        [SerializeField]
        private UIFlexbox _middleFlexBox;
        [SerializeField]
        private UIPopup _myPopUp;

        private void OnEnable()
        {
            ChangeCanvasOrientation();
        }

        public void OpenEmail()
        {
            Application.OpenURL(_emailLink);
        }

        public void OpenStore()
        {
            Application.OpenURL(_storeLink);
        }

        public void OpenPP()
        {
            Application.OpenURL(_privacyPolicy);
        }

        public void ShowDonationPopUp()
        {
            _myPopUp.Show();
        }

        public void ChangeCanvasOrientation()
        {
            bool _isOrientationPortret = (Screen.currentResolution.height > Screen.currentResolution.width);

            if (_isOrientationPortret == false)
            {
                _middleFlexBox.flexDirection = FlexDirection.Row;
            }
            else
            {
                _middleFlexBox.flexDirection = FlexDirection.Column;
            }
        }
    }
}

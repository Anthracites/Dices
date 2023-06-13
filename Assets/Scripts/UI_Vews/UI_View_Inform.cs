using UnityEngine;
using Doozy.Engine.UI;
using UnityPackages.UI;

namespace Dices.UserInterface
{
    public class UI_View_Inform : MonoBehaviour
    {
        [SerializeField]
        private string _emailLink = "mailto:dicesapp@gmail.com", _storeLink = "https://play.google.com/store/apps/details?id=com.AnthraciteStrixdev.Dices", _privacyPolicy = "https://sites.google.com/view/dices-privacy-policy/%D0%B3%D0%BB%D0%B0%D0%B2%D0%BD%D0%B0%D1%8F-%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%86%D0%B0";
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

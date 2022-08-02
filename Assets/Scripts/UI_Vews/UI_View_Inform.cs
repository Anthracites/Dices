using UnityEngine;
using Doozy.Engine.UI;
using UnityPackages.UI;

namespace Dices.UserInterface
{
    public class UI_View_Inform : MonoBehaviour
    {
        [SerializeField]
        private string _emailLink = "mailto:dicesapp@gmail.com";
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

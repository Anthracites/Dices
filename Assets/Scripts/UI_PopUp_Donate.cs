using UnityEngine;
using Doozy.Engine.UI;
using Zenject;
using Dices.UIConnection;

namespace Dices.UserInterface
{
    public class UI_PopUp_Donate : MonoBehaviour // Class for control popup for donation
    {
        [Inject]
        DonationManager _donationManager;

        [SerializeField]
        private UIPopup _thisPopUp;


        private void Start()
        {
            _donationManager.DonationAmount = 10;
            _thisPopUp.Show();
            ClosePopUp();
        }

        public void ClosePopUp()
        {
            _thisPopUp.Hide();
        }

        public void ChooseDonationAmount(int _amount)
        {
            _donationManager.DonationAmount = _amount;

        }
    }
}

using UnityEngine;
using Doozy.Engine.UI;
using Zenject;
using Dices.UIConnection;
using UnityEngine.Purchasing;
using Doozy.Engine;

namespace Dices.UserInterface
{
    public class UI_PopUp_Donate : MonoBehaviour // Class for control popup for donation
    {
        [Inject]
        DonationManager _donationManager;

        [SerializeField]
        private UIPopup _thisPopUp;
        [SerializeField]
        private IAPButton _submitButton;


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

        public void GetSuccesfulPurchase()
        {
            GameEventMessage.SendEvent(EventsLibrary.OnSuccessConsumable);
        }
        
        public void ChooseDonationAmount(int _amount)
        {
            _donationManager.DonationAmount = _amount;

            string _productID;

            switch (_amount)
            {
                case 1:
                    _productID = "0";
                    break;
                case 5:
                    _productID = "10";
                    break;
                case 10:
                    _productID = "20";
                    break;
                default:
                    _productID = "20";
                    break;
            }

            _submitButton.productId = _productID;
        }
    }
}

using Zenject;

namespace Dices.UIConnection
{
public class DonationManager : IInitializable // Class for contein information about donation
    {
        public int DonationAmount = 1;
        public int TransactionNumber = 0;
        public string ProductID = "20";

        public void Initialize()
        {

        }
    }
}

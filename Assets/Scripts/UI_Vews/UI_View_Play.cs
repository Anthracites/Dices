using UnityEngine;
using UnityEngine.UI;

namespace Dices.UserInterface
{
    public class UI_View_Play : MonoBehaviour // Class for Show/Hide elemenst on canvas play
    {
        [SerializeField]
        private Doozy.Engine.UI.UIDrawer _playFull;
        [SerializeField]
        private Button SwichViewButton;

        private void OnEnable()
        {
            ShowFullView();
            SwichViewButton.onClick.AddListener(HideFullView);
        }

        public void HideFullView()
        {
            _playFull.Close();
            SwichViewButton.onClick.RemoveAllListeners();
            SwichViewButton.onClick.AddListener(ShowFullView);
        }

        public void ShowFullView()
        {
            _playFull.Open();
            SwichViewButton.onClick.RemoveAllListeners();
            SwichViewButton.onClick.AddListener(HideFullView);
        }

        private void OnDisable()
        {
            SwichViewButton.onClick.RemoveAllListeners();
        }
    }
}

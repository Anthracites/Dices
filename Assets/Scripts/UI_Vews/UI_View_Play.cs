using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dices.UserInterface
{
    public class UI_View_Play : MonoBehaviour // Class for Show/Hide elemenst on canvas play
    {
        [Inject]
        UIConnection.ScoreManager scoreManager;

        [SerializeField]
        private Doozy.Engine.UI.UIDrawer _playFull;
        [SerializeField]
        private Button SwichViewButton;
        [SerializeField]
        private Material[] DetailMarkers;

        private void SendToManager()
        {
            scoreManager.DetailMarkerMaterials.Clear();
            scoreManager.DetailMarkerMaterials.RemoveAll(x => x == null);

            foreach (Material m in DetailMarkers)
            {
                scoreManager.DetailMarkerMaterials.Add(m);
            }
//            Debug.Log("Material sended to manager. List count: " + scoreManager.DetailMarkerMaterials.Count.ToString());
        }

        private void OnEnable()
        {
            ShowFullView();
            SwichViewButton.onClick.AddListener(HideFullView);
            SendToManager();
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

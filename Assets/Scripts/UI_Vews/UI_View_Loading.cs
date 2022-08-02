using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Dices.UserInterface
{
    public class UI_View_Loading : MonoBehaviour // Canvas to loading screen for show applucation loadin progress
    {
        [SerializeField]
        private Image _loadingImage;
        [SerializeField]
        private TMP_Text _loadingLabel;

        private void Start()
        {
            StartCoroutine(LoadingShow());
        }
        IEnumerator LoadingShow()
        {
            AsyncOperation _operation = SceneManager.LoadSceneAsync(1);
            while (!_operation.isDone)
            {
                float _progress = _operation.progress;
                _loadingImage.fillAmount = _progress;
                _loadingLabel.text = (_progress * 100).ToString();
                yield return null;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Dices.UserInterface
{
    public class FlashText : MonoBehaviour // Class to make text flash
    {
        private void OnEnable()
        {
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            bool _isActive = true;
            while (true)
            {
                gameObject.SetActive(_isActive);
                _isActive = !_isActive;
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}

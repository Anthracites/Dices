using UnityEngine;

namespace Dices.UserInterface // Class for full play canvas
{
    public class CameraRottion : MonoBehaviour
{
        [SerializeField]
        private Camera _mainCamera;

        private void Awake()
        {
            RotateCamera();
        }
        public void RotateCamera()
        {

            bool _isOrientationPortret = (Screen.currentResolution.height > Screen.currentResolution.width);

            if (_isOrientationPortret == false)
            {
                gameObject.transform.eulerAngles = new Vector3(90, 0, 90);
                _mainCamera.fieldOfView = 19;
            }
            else
            {
                gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
                _mainCamera.fieldOfView = 40;
            }
        }
    }
}

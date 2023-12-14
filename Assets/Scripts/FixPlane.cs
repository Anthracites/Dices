using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Zenject;

namespace Dices
{
    public class FixPlane : MonoBehaviour
    {
        [Inject]
        UIConnection.SettingsManager _settingsManager;

        private Rigidbody rigidbody;
        private Vector3 upPosition;
        private IEnumerator speedControl;
        [SerializeField]
        private int cubeAmount, falledCubeAmount;
        [SerializeField]
        private float verticalSpeed;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
            upPosition = new Vector3(0, 20, 0);
        }

        public void SwichFalledCount()
        {
            cubeAmount = _settingsManager.DicesAmount;
            falledCubeAmount++;
            if (cubeAmount == falledCubeAmount)
            {
                Time.timeScale = 100;
                DropPlane();
            }
        }

        void DropPlane()
        {
            speedControl = SpeedConrol();
            rigidbody.useGravity = true;
            StartCoroutine(speedControl);
            Debug.Log("Dropped!!!");

        }

       public void RerollOneDice()
        {
            UpPlane();
            falledCubeAmount = cubeAmount - 1;
        }

        public void UpPlane()
        {
            cubeAmount = _settingsManager.DicesAmount;

            rigidbody.useGravity = false;
            gameObject.transform.position = upPosition;
            falledCubeAmount = 0;
            //cubeAmount = 0;
        }

        public IEnumerator SpeedConrol()
        {
            while (rigidbody.velocity.y == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            while (rigidbody.velocity.y != 0)
            {
                yield return new WaitForEndOfFrame();
                verticalSpeed = rigidbody.velocity.y;
            }
            StopCoroutine(speedControl);
            GameEventMessage.SendEvent(EventsLibrary.FixPanelFalled);
            Debug.Log("Falled!!!");
        }
    }
}
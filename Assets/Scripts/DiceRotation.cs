using UnityEngine;
using System.Collections;
using Doozy.Engine;

namespace Dices.GamePlay
{
    public class DiceRotation : MonoBehaviour // Class for move dices
    {

        [SerializeField]
        private float angle = 20; // скорость поворота в градусах
        [SerializeField]
        private float angle1; // скорость поворота в кватернионах
        [SerializeField]
        private Vector3 rotationAxis; // ось вращения

        [SerializeField]
        private float A, B, C;
        [SerializeField]
        private Quaternion Q;
        [SerializeField]
        private IEnumerator speedControl;
        private Rigidbody rb;
        private BoxCollider[] scores;

        public bool IsStoded = false;
        public bool IsStopByTimer = false;

        void Start()
        {
            GetRotationParameters();
            StartCoroutine(Rotation());
        }

        public IEnumerator Rotation()
        {
           while (gameObject.transform.position.y > 2)
                {
                yield return new WaitForSeconds(0);
                DiceRotationfunc();
            }

        }

        public IEnumerator SpeedConrol()
        {
            Debug.Log("Coroutine started for " + gameObject.name);
            while (rb.velocity.y == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            while (rb.velocity.y != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            GameEventMessage.SendEvent(EventsLibrary.CubeFalled);
            StopCoroutine(speedControl);
        }


        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        void DiceRotationfunc()
        {
            angle1 = angle * (Mathf.PI / 180);
            Q = new Quaternion(Mathf.Sin(angle1 / 2) * rotationAxis.x, Mathf.Sin(angle1 / 2) * rotationAxis.y, Mathf.Sin(angle1 / 2) * rotationAxis.z, Mathf.Cos(angle1 / 2));
            transform.rotation = transform.rotation * Q;
        }

        public void StopRotation()
        {
            IsStoded = true;
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().mass = 100;
            rb = gameObject.GetComponent<Rigidbody>();
            speedControl = SpeedConrol();


            if (IsStopByTimer == true)
            {
                StopAllCoroutines();
            }

            StartCoroutine(speedControl);
        }
        void GetRotationParameters()
        {
            A = Random.Range(-1.00f, 1.00f);
            B = Random.Range(-1.00f, 1.00f);
            C = Random.Range(-1.00f, 1.00f);
            rotationAxis = new Vector3(A, B, C).normalized;
        }    
    }
}

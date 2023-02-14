using UnityEngine;
using System.Collections;
using Doozy.Engine;
using Dices.UIConnection;
using Zenject;

namespace Dices.GamePlay
{
    public class DiceRotation : MonoBehaviour // Class for move dices
    {
        [Inject]
        ScoreManager _scoreManager;

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

        void Awake()
        {
            GetRotationParameters();
            StartCoroutine(Rotation());
        }

        public IEnumerator Rotation()
        {
            Debug.Log("Start rotate");

            while (gameObject.transform.position.y > 2)
                {
                yield return new WaitForSeconds(0);
                DiceRotationfunc();
            }

        }

        public IEnumerator SpeedConrol()
        {
//            Debug.Log("Coroutine started for " + gameObject.name);
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

        public void OnStopRotationMessage()
        {
            if (IsStoded == true)
            {
                DestroySelf();
            }

            else
            {
                StopRotation();
            }
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

         void StopRotation()
        {
            IsStoded = true;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 100;

            if (_scoreManager.SpawnBySwipe == true)
            {
                rb.AddForce(transform.forward * 100000);
            }
            speedControl = SpeedConrol();

            if (IsStopByTimer == true)
            {
                StopAllCoroutines();
            }

            StartCoroutine(speedControl);
            Debug.Log("Stope rotate!!!");
        }
        void GetRotationParameters()
        {
            A = Random.Range(-1.00f, 1.00f);
            B = Random.Range(-1.00f, 1.00f);
            C = Random.Range(-1.00f, 1.00f);
            rotationAxis = new Vector3(A, B, C).normalized;
        }


        public class Factory : PlaceholderFactory<Object, DiceRotation>
        {
        }


        //public class Factory : PlaceholderFactory<string, DiceRotation>
        //{

        //} // префаб по пути 
    }
}

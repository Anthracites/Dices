using UnityEngine;
using System.Collections;
using Doozy.Engine;
using Dices.UIConnection;
using Zenject;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace Dices.GamePlay
{
    public class DiceRotation : MonoBehaviour // Class for move dices
    {
        [Inject]
        ScoreManager _scoreManager;
        [Inject]
        SettingsManager _settingsManager;

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
        [SerializeField]
        private GameObject[] scoreCubes;
        [SerializeField]
        private Collider thisCollider;

        public bool IsStoded = false;
        public bool IsStopByTimer = false;
        private bool isRerolled, isAnimated;

        void Awake()
        {
            isRerolled = false;
            isAnimated = _settingsManager.IsAnimated;
            if (isAnimated == true)
            {
                GetRotationParameters();
                StartCoroutine(Rotation());
            }
            gameObject.transform.position = new Vector3(1, 1, 1);
        }
        private void Start()
        {
            if (isAnimated == false)
            {
                StartCoroutine(Fall(new WaitForFixedUpdate()));
            }
        }

        void ChangeCurrentScore()
        {
            GameObject scorePlane = _scoreManager.ScoreCountPlane;
            foreach(GameObject scoreCube in scoreCubes)
            {
                if (scorePlane.GetComponent<Collider>().bounds.Intersects(scoreCube.GetComponent<Collider>().bounds))
                {
                    int m = Int32.Parse(scoreCube.name);
                    _scoreManager.Score -= m;
                    _scoreManager.ScoreDetales[m - 1]--;
                }
            }
            GameEventMessage.SendEvent(EventsLibrary.ScoreChanged);
        }

        public IEnumerator Rotation()
        {
            while (gameObject.transform.position.y > 2)
                {
                yield return new WaitForSeconds(0);
                DiceRotationfunc();
            }

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == thisCollider)
                    {
                        OnTap();
                    }
                }
            }
        }

        public void OnTap()
        {
            foreach (GameObject scoreCube in scoreCubes)
            {
                scoreCube.SetActive(true);
            }
            ChangeCurrentScore();
            GameEventMessage.SendEvent(EventsLibrary.RerollOneDice);
            if (isAnimated == true)
            {
                isRerolled = true;
                rb = gameObject.GetComponent<Rigidbody>();
                gameObject.transform.position = new Vector3(transform.position.x, 10, transform.position.z);
                DiceRotationfunc();
                float _force = UnityEngine.Random.Range(50000, 100000);
                rb.AddForce(transform.forward * _force);
                StopRotation();
            }
            else
            {
                float[] _angles = { -180, -90, 0f, 90, 180 };
                int A = UnityEngine.Random.Range(0, _angles.Length - 1);
                int B = UnityEngine.Random.Range(0, _angles.Length - 1);
                int C = UnityEngine.Random.Range(0, _angles.Length - 1);
                Quaternion spawnRotation = Quaternion.Euler(_angles[A], _angles[B], _angles[C]);
                gameObject.transform.rotation = spawnRotation;
                StartCoroutine(Fall(new WaitForFixedUpdate()));
            }
        }

        public IEnumerator Fall(dynamic a)
        {
            yield return a;
            GameEventMessage.SendEvent(EventsLibrary.CubeFalled);
            Debug.Log("Falled");
        }

        public IEnumerator SpeedConrol()
        {
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

            if (isRerolled == false)
            {
                rb = gameObject.AddComponent<Rigidbody>();

                rb.mass = 100;

                if (_scoreManager.SpawnBySwipe == true)
                {
                    rb.AddForce(transform.forward * 100000);
                }
            }

            speedControl = SpeedConrol();

            if (IsStopByTimer == true)
            {
                StopAllCoroutines();
            }

            StartCoroutine(speedControl);
        }
        void GetRotationParameters()
        {
            A = UnityEngine.Random.Range(-1.00f, 1.00f);
            B = UnityEngine.Random.Range(-1.00f, 1.00f);
            C = UnityEngine.Random.Range(-1.00f, 1.00f);
            rotationAxis = new Vector3(A, B, C).normalized;
        }


        public class Factory : PlaceholderFactory<UnityEngine.Object, DiceRotation>
        {

        }


        //public class Factory : PlaceholderFactory<string, DiceRotation>
        //{

        //} // префаб по пути 
    }
}

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
        private IEnumerator speedControl;
        private Rigidbody rb;
        [SerializeField]
        private GameObject[] scoreCubes;
        [SerializeField]
        private Collider thisCollider;
        [SerializeField]
        private GameObject detailMarker;
        [SerializeField]
        private int score;
        private Material defauiltMaterial;

        public bool IsStoded = false;
        public bool IsStopByTimer = false;
        private bool isRerolled, isAnimated;

        void Awake()
        {
            defauiltMaterial = detailMarker.GetComponent<MeshRenderer>().material;
            isRerolled = false;
            isAnimated = _settingsManager.IsAnimated;
            if (isAnimated == true)
            {
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

        public void SwichDetailMarker()
        {
            GameObject scorePlane = _scoreManager.ScoreCountPlane;
            foreach (GameObject scoreCube in scoreCubes)
            {
                if (scorePlane.GetComponent<Collider>().bounds.Intersects(scoreCube.GetComponent<Collider>().bounds))
                {
                    score = Int32.Parse(scoreCube.name);
                }
            }
            if (_scoreManager.SelectedScores[score - 1] == true)
            {
                detailMarker.GetComponent<MeshRenderer>().sharedMaterial = _scoreManager.DetailMarkerMaterials[score - 1];
            }
            else
            {
                detailMarker.GetComponent<MeshRenderer>().sharedMaterial = defauiltMaterial;

            }
            detailMarker.SetActive(_scoreManager.SelectedScores[score - 1]);
        }

        void ChangeCurrentScore()
        {
            GameObject scorePlane = _scoreManager.ScoreCountPlane;
            foreach (GameObject scoreCube in scoreCubes)
            {
                if (scorePlane.GetComponent<Collider>().bounds.Intersects(scoreCube.GetComponent<Collider>().bounds))
                {
                    score = Int32.Parse(scoreCube.name);
                    _scoreManager.Score -= score;
                    _scoreManager.ScoreDetales[score - 1]--;

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
            detailMarker.GetComponent<MeshRenderer>().sharedMaterial = defauiltMaterial;
            ChangeCurrentScore();
            GameEventMessage.SendEvent(EventsLibrary.RerollOneDice);
            if (isAnimated == true)
            {
                speedControl = SpeedConrol();
                GameEventMessage.SendEvent(EventsLibrary.RerollOneDice);
                Time.timeScale = 5;
                isRerolled = true;
                rb = gameObject.GetComponent<Rigidbody>();
                gameObject.transform.position = new Vector3(transform.position.x, 10, transform.position.z);
                DiceRotationfunc();
                float _force = UnityEngine.Random.Range(50000, 100000);
                rb.AddForce(transform.forward * _force);
                StopRotation();
                StartCoroutine(speedControl);
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
            rb.AddForce(Vector3.down * 0.05f, ForceMode.Impulse);
            GameEventMessage.SendEvent(EventsLibrary.FixPanelFalled);
            SwichDetailMarker();
        }

        public IEnumerator SpeedConrol()
        {
            while (rb.linearVelocity.y == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            while (rb.linearVelocity.y != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            // Кость перестала падать по вертикали, но могла остановиться на ребре или углу.
            // Проверяем это ОДИН раз и, если нужно, мгновенно доворачиваем до ближайшей
            // грани — без физических толчков и без ожидания, пока физика "сама
            // успокоится". Это важно: CubeFalled ниже — это сигнал, от которого зависит
            // общий подсчёт очков (ScoreCounter ждёt его от КАЖДОЙ кости, строго). Любая
            // корутина здесь, которая теоретически может не завершиться (например, если
            // rb.velocity никогда не уйдёт точно в ноль), означает, что подсчёт очков
            // зависнет целиком — поэтому тут сознательно нет циклов ожидания.
            if (GetBestFaceAlignment() < faceAlignmentThreshold)
            {
                SnapToNearestFace();
                // Поворот подменили мгновенно — гасим остаточную скорость/вращение,
                // чтобы кость не продолжила с того места, где её "прервали", и не
                // съехала с подмененной грани на следующем физическом шаге.
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            GameEventMessage.SendEvent(EventsLibrary.CubeFalled);
            StopCoroutine(speedControl);
        }

        // --- Защита от приземления на ребро/угол ---

        [SerializeField]
        [Tooltip("Насколько точно нормаль грани должна совпадать с мировым 'вверх' (1 = идеально), чтобы считать, что кость легла на грань.")]
        private float faceAlignmentThreshold = 0.97f;

        private static readonly Vector3[] LocalFaceNormals =
        {
            Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right
        };

        // Возвращает, насколько хорошо текущая ориентация кости совпадает с "лежит на грани"
        // (1 — идеально лежит на грани, меньше — балансирует на ребре/углу)
        private float GetBestFaceAlignment()
        {
            float best = -1f;
            foreach (Vector3 localNormal in LocalFaceNormals)
            {
                Vector3 worldNormal = transform.TransformDirection(localNormal);
                float dot = Vector3.Dot(worldNormal, Vector3.up);
                if (dot > best)
                {
                    best = dot;
                }
            }
            return best;
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
            float angle1;
            Quaternion Q;
            Vector3 rotationAxis = RotationAxis();
            angle1 = 20 * (Mathf.PI / 180);
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
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.ResetCenterOfMass();
                rb.angularDamping = 0.5f;
            }

            speedControl = SpeedConrol();

            if (IsStopByTimer == true)
            {
                StopAllCoroutines();
            }

            StartCoroutine(speedControl);
        }

        private Vector3 RotationAxis()
        {
            float A, B, C;
            A = UnityEngine.Random.Range(-1.00f, 1.00f);
            B = UnityEngine.Random.Range(-1.00f, 1.00f);
            C = UnityEngine.Random.Range(-1.00f, 1.00f);
            Vector3 rotationAxis = new Vector3(A, B, C).normalized;
            return rotationAxis;
        }

        private void SnapToNearestFace()
        {
            Quaternion current = transform.rotation;
            float minAngle = float.MaxValue;
            Quaternion bestRotation = current;

            // Возможные ориентации куба (6 граней)
            Quaternion[] orientations = {
        Quaternion.LookRotation(Vector3.forward, Vector3.up),
        Quaternion.LookRotation(Vector3.back, Vector3.up),
        Quaternion.LookRotation(Vector3.left, Vector3.up),
        Quaternion.LookRotation(Vector3.right, Vector3.up),
        Quaternion.LookRotation(Vector3.up, Vector3.back),
        Quaternion.LookRotation(Vector3.down, Vector3.forward)
    };

            foreach (var q in orientations)
            {
                float angle = Quaternion.Angle(current, q);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestRotation = q;
                }
            }

            transform.rotation = bestRotation;
        }



        public class Factory : PlaceholderFactory<UnityEngine.Object, DiceRotation>
        {

        }
    }
}
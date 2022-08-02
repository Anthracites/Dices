using UnityEngine;
using System.Collections;

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
            if (IsStopByTimer == true)
            {
                StopAllCoroutines();
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotation : MonoBehaviour
{
    public float angle = 20; // скорость поворота в градусах
    public float angle1; // скорость поворота в кватернионах
    public Vector3 V; // ось вращения
    public float A, B, C;
    public Quaternion Q;
    public Quaternion FormSpawner;
    public bool IsStoded = false;

    void Start()
    {
        A = Random.Range(-1.00f, 1.00f);
        B = Random.Range(-1.00f, 1.00f);
        C = Random.Range(-1.00f, 1.00f);
        V = new Vector3(A, B, C).normalized;
    }
    void Update()
    {
        if (IsStoded == false)
        {
            DiceRotationfunc();
        }
    }

    void DiceRotationfunc()
    {
        angle1 = angle * (Mathf.PI / 180);
        Q = new Quaternion(Mathf.Sin(angle1 / 2) * V.x, Mathf.Sin(angle1 / 2) * V.y, Mathf.Sin(angle1 / 2) * V.z, Mathf.Cos(angle1 / 2));
        transform.rotation = transform.rotation * Q;
    }

    public void StopRotation()
    {
        IsStoded = true;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = 40;
    }
}

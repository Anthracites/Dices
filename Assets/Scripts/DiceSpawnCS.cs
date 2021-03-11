using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSpawnCS : MonoBehaviour
{
    public GameObject CanvasPlayObj;
    public GameObject CanvasSetObj;
    public GameObject ScoreCounterObj;
    public int DisecNumber;
    public int i = 0;
    public int A, B, C;
    public int k;
    public float timer;
    public float[] Angles = {-180, -90, 0, 90, 180};
    public float FirstSpawnPoint;
    public float Distance;
    public Text ScoreShowText;
    public GameObject GameControllerObj;
    public GameObject[] DicePref;
    public GameObject[] Dices;
    public GameObject Reroll;
    public GameObject StopRotate;
    public bool IsAnimated = false;
    public bool IsAmountEven;
    private GameObject inst_obj;
    public Vector3 DiceRandomRotate;

    void Start()
    {
    }


    public void SpawnDice()
    {
        Distance = DicePref[0].transform.localScale.x * 10;
        CanvasPlayObj.SetActive(true);
        DestroyDices();
        IsAnimated = GameControllerObj.GetComponent<ControlCS>().UseAnim;
        DisecNumber = GameControllerObj.GetComponent<ControlCS>().DiceAmount;
        ChekcAmount();
        i = 0;

        if (IsAnimated == true)
        {
            AnimSpawnDice();
        }
        else
        {
            NoAnimSpawnDice();
        }
        GameControllerObj.SetActive(false);
    }

    void AnimSpawnDice()
    {
        MadeFirstSpawnPoint();

        while (i < DisecNumber)
        {
            // Vector3 SpawnPosition = new Vector3((FirstSpawnPoint + i) * Distance * (Mathf.Pow(-1, i + 1)), 1f, (-(FirstSpawnPoint * (Mathf.Pow(-1, i)) * (1 / Distance))));
            Vector3 SpawnPosition = new Vector3((FirstSpawnPoint + i) * Distance * (Mathf.Pow(-1, i + 1)), 3f, (-(FirstSpawnPoint * (Mathf.Pow(-1, i)) * (Distance / DisecNumber) * Distance)));
            Quaternion spawnRotation = new Quaternion(UnityEngine.Random.Range(-i, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
            int n = UnityEngine.Random.Range(0, DicePref.Length);
            inst_obj = Instantiate(DicePref[n], SpawnPosition, spawnRotation) as GameObject;
            inst_obj.transform.parent = gameObject.transform;
            i++;
            if (GameControllerObj.GetComponent<ControlCS>().CurrentMode == ControlCS.StopMode.Automatic)
            {
                StopDiceRotate();
            }
        }
        ScoreCounterObj.GetComponent<ScoreCounter>().ScoreCleaner();
        GameControllerObj.GetComponent<ControlCS>().ShowHideStopButton();
    }

    void DestroyDices()
    {
        for (int j = gameObject.transform.childCount; j > 0; --j)
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
    }

    void NoAnimSpawnDice()
    {
        MadeFirstSpawnPoint();

        while (i < DisecNumber)
        {
            Vector3 SpawnPosition = new Vector3((FirstSpawnPoint + i) * Distance * (Mathf.Pow(-1, i + 1)), 1f, (-(FirstSpawnPoint * (Mathf.Pow(-1, i)) * (Distance/DisecNumber)* Distance))); 
            A = UnityEngine.Random.Range(0, Angles.Length - 1);
            B = UnityEngine.Random.Range(0, Angles.Length - 1);
            C = UnityEngine.Random.Range(0, Angles.Length - 1);
            Quaternion spawnRotation = Quaternion.Euler(Angles[A], Angles[B], Angles[C]);
            int n = UnityEngine.Random.Range(0, DicePref.Length);
            inst_obj = Instantiate(DicePref[n], SpawnPosition, spawnRotation) as GameObject;
            inst_obj.GetComponent<DiceRotation>().IsStoded = true;
            inst_obj.GetComponent<DiceRotation>().FormSpawner = spawnRotation;
            inst_obj.transform.parent = gameObject.transform;
            i++;
        }
        ScoreCounterObj.GetComponent<ScoreCounter>().NoAnimCubeCount();
        Reroll.SetActive(true);
    }

    public void StopDiceRotate()
    {
        Dices = GameObject.FindGameObjectsWithTag("Dice");

        foreach (GameObject Dice in Dices)
        {
            {
                int j = 0;
                Dice.GetComponent<DiceRotation>().StopRotation();
                j++;
            }
        }
        timer = 0;

        ScoreCounterObj.GetComponent<ScoreCounter>().ScoreCleaner();
        StopRotate.SetActive(false);
        Reroll.SetActive(true);

    }



    void TimeCounter()
    {
        timer += Time.deltaTime;
    }

    void Update()
    {
        ScoreShow();
        if (timer > 1)
        {
            StopDiceRotate();
        }
        else
        {
            timer = 0;
        }
    }

    void ChekcAmount()
    {

        if ((DisecNumber % 2) == 0)
        {
            IsAmountEven = true;
        }
        else
        {
            IsAmountEven = false;
        }
    }

    void MadeFirstSpawnPoint()
   
    {
        FirstSpawnPoint = - ((DisecNumber / 2));
    }
        

    void ScoreShow()
    {
        ScoreShowText.text = ScoreCounterObj.GetComponent<ScoreCounter>().Score.ToString();
    }

}

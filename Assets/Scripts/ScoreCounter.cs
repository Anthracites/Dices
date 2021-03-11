using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public int Score = 0;
    public int i;
    public int k = 0;
    public int j = 0;
    public int m;
    public GameObject[] CountCubes;
    public GameObject[] ScoreDetails;
    public GameObject inst_obj;
    public List<GameObject> ScoreDetalesObjList = new List<GameObject>();
    public List<GameObject> ScoreCubeObjList = new List<GameObject>();


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ScoreCube")
        {
            m = (Int32.Parse(other.name));
            Score = Score + m;
            ScoreDetails[m - 1].GetComponent<ScoreDetailShow>().DetailScore++;
            ScoreDetails[m - 1].GetComponent<ScoreDetailShow>().DetailScoreShow();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ScoreCube")
        {
            m = (Int32.Parse(other.name));
            Score = Score - m;
            ScoreDetails[m - 1].GetComponent<ScoreDetailShow>().DetailScore--;
            ScoreDetails[m - 1].GetComponent<ScoreDetailShow>().DetailScoreShow();
        }
    }

    public void NoAnimCubeCount()
    {
        ScoreCleaner();
        CountCubes = GameObject.FindGameObjectsWithTag("ScoreCube");
        i = 0;
        foreach (GameObject CountCube in CountCubes)
        {
                if (gameObject.GetComponent<Collider>().bounds.Intersects(CountCubes[i].GetComponent<Collider>().bounds))
                {
                    Score = Score + (Int32.Parse(CountCubes[i].name));
                    ScoreDetails[(Int32.Parse(CountCubes[i].name)) - 1].GetComponent<ScoreDetailShow>().DetailScore++;
                ScoreDetails[(Int32.Parse(CountCubes[i].name)) - 1].GetComponent<ScoreDetailShow>().DetailScoreShow();
                    ScoreCubeObjList.Add(CountCubes[i]);
                }
                i++;
        }
    }

    public void DetailShow()
    {
    }


    public void ScoreCleaner()
    {
        Score = 0;
        k = 0;
        j = 0;
        foreach (GameObject DetailScore in ScoreDetalesObjList)
        {
            {
                Destroy(ScoreDetalesObjList[k]);
                k++;
            }
        }

        k = 0;
        foreach (GameObject ScoreDetail in ScoreDetails)
        {
            {
                ScoreDetails[k].GetComponent<ScoreDetailShow>().ClearDetails();
                k++;
            }
        }

    }


    /* void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sphere")
            Score++;
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sphere")
            Score--;
    }

   public void NoAnimSphereCount() // Sphere
    {
        ScoreCleaner();
        Spheres = GameObject.FindGameObjectsWithTag("Sphere");
        i = 0;
        foreach (GameObject Sphere in Spheres)
        {
            {
                if (gameObject.GetComponent<Collider>().bounds.Intersects(Spheres[i].GetComponent<Collider>().bounds))
                {
                    Score++;
                }
                i++;
            }
        }
    }
    
     
   */

}

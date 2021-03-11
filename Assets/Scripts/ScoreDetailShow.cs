using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDetailShow : MonoBehaviour
{
    public int DetailScore = 0;
    private Text DetailText;


    public void DetailScoreShow()
    {
        gameObject.GetComponent<Text>().text = DetailScore.ToString();
    }

    public void ClearDetails()
    {
        DetailScore = 0;
        DetailScoreShow();
    }
}

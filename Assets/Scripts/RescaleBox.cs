using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RescaleBox : MonoBehaviour
{
    [SerializeField]
    private GameObject box, scoreDetail;

    [SerializeField]
    private float screenDpi, diagonal;

    [SerializeField]
    private int screenHeight, screenWidth;
    [SerializeField]
    private Vector2 detailSize;

    void Start()
    {
        ResizeBox();
    }

   public void ResizeBox()
    {
        detailSize = scoreDetail.gameObject.transform.localScale;
             screenDpi = Screen.dpi;
            screenHeight = Screen.height;
            screenWidth = Screen.width;
        Vector2 _detailScale = scoreDetail.GetComponent<RectTransform>().sizeDelta;

        diagonal = Mathf.Sqrt(screenWidth * screenWidth + screenHeight * screenHeight) / screenDpi;

        float z = (((screenHeight / screenDpi) * 3.5f) / (diagonal/(screenDpi/100)))/4;
        float x = (((screenWidth / screenDpi) / (diagonal / (screenDpi / 100)))) / 4;

        box.transform.localScale = new Vector3(x, 10f, z);


        Debug.Log("Panel's size:" + _detailScale.x + " X " + _detailScale.y);
    }

}



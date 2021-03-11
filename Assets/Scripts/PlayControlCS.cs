using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControlCS : MonoBehaviour
{
    public GameObject CanvasSettings;

    public void GoToSettings()
    {
        CanvasSettings.SetActive(true);
        gameObject.SetActive(false);
    }
}

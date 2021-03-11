using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlCS : MonoBehaviour
{
    public Slider DiceAmountSlider;
    public InputField DiceAmountField;
    public Dropdown StopModeSelect;
    public GameObject StopRotationButton;
    public GameObject RerollBotton;
    public GameObject ScoreDetails;
    public bool UseAnim = false;
    public enum StopMode { Manual, Automatic, OnTimer };
    public StopMode CurrentMode;
    public int DiceAmount;
    public int i;
    public int Score;

    void Start()
    {
        DiceAmountField.text = 1.ToString();
        CurrentMode = StopMode.Manual;
        i = 0;
    }

    public void ChangeStopMode()
    {
        if (StopModeSelect.value == 0)
        {
            CurrentMode = StopMode.Manual;
            StopRotationButton.SetActive(true);
        }
        else
        if (StopModeSelect.value == 1)
        {
            CurrentMode = StopMode.Automatic;
            StopRotationButton.SetActive(false);
        }
        else
        if (StopModeSelect.value == 2)
        {
            CurrentMode = StopMode.OnTimer;
        }
    }

    public void ChangeDetailShow()
    {
        if (ScoreDetails.activeSelf == true)
        {
            ScoreDetails.SetActive(false);
        }
        else 
        {
            ScoreDetails.SetActive(true);
        }
    }


    public void ChangeField()
    {
        DiceAmountField.text = DiceAmountSlider.value.ToString();
        DiceAmount = Convert.ToInt32(DiceAmountSlider.value);
    }

    public void ChangeSlider()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) != true)
        {
            DiceAmountSlider.value = Int32.Parse(DiceAmountField.text.ToString());
            DiceAmount = Int32.Parse(DiceAmountField.text.ToString());
        }
    }

    public void LessDiceAmount()
    {
        if (DiceAmount > 1)
        {
            DiceAmount--;
            ChangeDiceAmount();
        }
    }

    public void MoreDiceAmount()
    {
        if (DiceAmount < 10)
        {
            DiceAmount++;
            ChangeDiceAmount();
        }
    }

    void ChangeDiceAmount()
    {
        DiceAmountSlider.value = DiceAmount;
        DiceAmountField.text = DiceAmount.ToString();
    }

    public void ChangeAnimUsing()
    {
        if (UseAnim == false)
        {
            UseAnim = true;
            StopModeSelect.interactable = true;
            StopRotationButton.SetActive(true);
        }
        else
        {
            UseAnim = false;
            StopModeSelect.interactable = false;
            StopRotationButton.SetActive(false);
        }
    }
    public void ShowHideStopButton()
    {
        if (CurrentMode == StopMode.Manual)
        {
            StopRotationButton.SetActive(true);
            RerollBotton.SetActive(false);
        }
    }
}

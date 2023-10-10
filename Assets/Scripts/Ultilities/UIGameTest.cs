using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameTest : MonoBehaviour
{
    public Text[] parameterTexts;
    public InputField targetFrameRateText;
    public MainPlayerAnimator mainCharacterAnimator;
    public InputController inputController;

    public int countSpecifyParemeters;
    public int targetFrameRate;

    public void ApplyTargetFrameRate()
    {
        targetFrameRate = System.Convert.ToInt32(targetFrameRateText.text);
        Application.targetFrameRate = targetFrameRate;
    }
}

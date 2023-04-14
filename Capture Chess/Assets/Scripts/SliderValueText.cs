using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueText : MonoBehaviour
{
    public Slider slider;

    void Update()
    {
        int actualGameLength = (int)slider.value * 2;
        string valueString = "";
        if (actualGameLength > 150)
            valueString = "";
        else
            valueString = actualGameLength.ToString() + "\n";

        GetComponent<TextMeshProUGUI>().text = valueString + NameForLength(actualGameLength);
    }

    string NameForLength(float length)
    {
        switch (length)
        {
            case <= 50:
                return "Skirmish";
            case <= 75:
                return "Battle";
            case <= 120:
                return "War";
            case <= 150:
                return "Extended";
            case > 150:
                return "Regicide";
            default:
                return "N/A";
        }
    }
}

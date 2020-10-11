using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Year : MonoBehaviour
{
    public Slider slider;
    public Zoonose2 zoonose2;


    Vector2 original;
 
    TMP_Text yearText;

    public float invertedValue, constructYear, year2000, decadeValue, yearValue;

    private void Awake()
    {

        yearText = GetComponent<TMP_Text>();
        year2000 = 2020f;
        yearText.text = " " + 1900f;
    }
    
    public void ChangeYear(float _invertedValue)
    {
        
        //_invertedValue = slider.value;
        //Debug.Log("InvertedValue" + _invertedValue);
        decadeValue = (Mathf.RoundToInt(120 * _invertedValue) + 1900);
   
        //0.01191895

        //Debug.Log(decadeValue);
        yearText.text = " " + decadeValue;

        if (slider.value == 0)
        {
            yearText.text = " " + 1900f;
        }
        if (slider.value == 1)
        {
            yearText.text = " " + 2020f;
        }

        invertedValue = _invertedValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public Image barFill;
    public Image barBorder;
    public Gradient fillGradient;

    public void SetMaxStamina(float maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
        barFill.color = fillGradient.Evaluate(1f);
        
    }
    public void SetStamina(float stamina)
    {
        slider.value = stamina;
        barFill.color = fillGradient.Evaluate(slider.normalizedValue);
                
        if (slider.value == slider.maxValue)
        {
            Color auxColor = barFill.color;
            auxColor.a = 0;
            barFill.color = auxColor;
            barBorder.color = auxColor;
        }  
        else
        {
            Color auxColor = barFill.color;
            auxColor.a = 100;
            barFill.color = auxColor;
            barBorder.color = auxColor;
        }
    }
}

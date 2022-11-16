using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public Image barFill;
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
    }
}

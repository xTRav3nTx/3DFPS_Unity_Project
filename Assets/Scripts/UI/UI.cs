using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient grad;
    [SerializeField] private Image fill;

    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = grad.Evaluate(1f);
    }

    public void Sethealth(float health)
    {
        slider.value = health;
        fill.color = grad.Evaluate(slider.normalizedValue);
    }


}

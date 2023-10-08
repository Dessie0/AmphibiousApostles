using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetValue(float value)
    {
        this.slider.value = Math.Min(1.0f, value);
    }
    
}

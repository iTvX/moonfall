using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class enemyHealthStat
{

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    [SerializeField]
    private Image content;

    [SerializeField]
    private float fillAmount;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = map(value, 0, MaxValue, 0, 1);
        }

    }

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }
        set
        {
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            MaxValue = maxVal;
        }
    }

    public void HandleBar()
    {
        content.fillAmount = fillAmount;
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }

    private float map(float value, float inMin, float inMax, float outMin, float outMax)

    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}

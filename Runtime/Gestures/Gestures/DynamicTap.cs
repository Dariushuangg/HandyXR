using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;

public class DynamicTap : DynamicGesture
{
    [HideInInspector] public bool IsPerforming = false;
    public float MaxDuration;
    public Timer Timer;
    public StaticThumbRestOnIndex StaticThumbRestOnIndexGesture;
    public StaticThumbUpward StaticThumbUpwardGesture;

    [Header("Debug")]
    public TextMeshProUGUI debugText;

    private void Start()
    {
        FeatureUsed |= StaticThumbRestOnIndexGesture.FeatureUsed;
        FeatureUsed |= StaticThumbUpwardGesture.FeatureUsed;
    }

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        if (!IsPerforming)
        {
            debugText.text = "...";
            if (StaticThumbUpwardGesture.DetectGesture(featureData))
            {
                IsPerforming = true;
                Timer.StartTimer(MaxDuration, () => { IsPerforming = false; });
            }
            return false;
        }
        else 
        {
            if (StaticThumbRestOnIndexGesture.DetectGesture(featureData)) 
            {
                IsPerforming = false;
                Timer.StopTimer();
                OnGestureDetected.Invoke();
                debugText.text = "Single Tap!";
                return true;
            }
            return false;
        }
    }
}

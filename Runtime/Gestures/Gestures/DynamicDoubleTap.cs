using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicDoubleTap : DynamicGesture
{
    [HideInInspector] public bool IsPerforming = false;
    public float MaxDuration;
    public Timer Timer;
    public DynamicTap tap;

    [Header("Debug")]
    public TextMeshProUGUI debugText;

    private void Start()
    {
        FeatureUsed |= tap.FeatureUsed;
    }

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        if (!IsPerforming)
        {
            debugText.text = "...";
            if (tap.DetectGesture(featureData))
            {
                IsPerforming = true;
                Timer.StartTimer(MaxDuration, () => { IsPerforming = false; });
            }
            return false;
        }
        else
        {
            if (tap.DetectGesture(featureData))
            {
                IsPerforming = false;
                Timer.StopTimer();
                OnGestureDetected.Invoke();
                debugText.text = "Double Tap!";
                return true;
            }
            return false;
        }
    }
}

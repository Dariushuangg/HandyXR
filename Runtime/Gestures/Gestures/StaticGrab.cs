using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticGrab : StaticGesture
{
    private void Start()
    {
        FeatureUsed |= GestureFeature.ThumbExtendedState;
        FeatureUsed |= GestureFeature.IndexExtendedState;
        FeatureUsed |= GestureFeature.MiddleExtendedState;
        FeatureUsed |= GestureFeature.RingExtendedState;
        FeatureUsed |= GestureFeature.LittleExtendedState;
    }

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        Debug.Log("detecting");
        if (featureData.Handedness != Handedness) return false;
        if (featureData.ThumbExtendedState
            + featureData.IndexExtendedState
            + featureData.MiddleExtendedState
            + featureData.RingExtendedState
            + featureData.LittleExtendedState != 0)
        {
            return false;
        }
        else
        {
            OnGestureDetected.Invoke();
            return true;
        }
    }
}

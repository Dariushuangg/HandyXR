using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPoke : StaticGesture
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
        if (featureData.Handedness != Handedness) return false;
        if (featureData.MiddleExtendedState
            + featureData.RingExtendedState
            + featureData.LittleExtendedState != 0)
        {
            return false;
        }
        else
        {
            if (featureData.ThumbExtendedState == 2 && featureData.IndexExtendedState == 2)
            {
                OnGestureDetected.Invoke();
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}

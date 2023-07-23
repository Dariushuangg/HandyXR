using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class SingleHandDynamicGestureDetector : GestureDetector
{
    public DynamicGesture DynamicGesture;

    private void OnEnable()
    {
        IsDetecting = true;
    }

    private void OnDisable()
    {
        IsDetecting = false;
    }

    public override void DetectGesture(GestureFeatureData leftHandFeatureData, GestureFeatureData rightHandFeatureData)
    {
        if (IsDetecting)
        {
            GestureFeatureData featureData = DynamicGesture.Handedness == Handedness.Right ? rightHandFeatureData : leftHandFeatureData;
            DynamicGesture.DetectGesture(featureData);
        }
    }

    public override GestureFeature GetAllFeaturesInUse()
    {
        if (IsDetecting) { return DynamicGesture.FeatureUsed; }
        else { return GestureFeature.None; }
    }
}

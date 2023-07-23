using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class SingleHandStaticGestureDetector : GestureDetector
{
    public StaticGesture StaticGesture;

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
            GestureFeatureData featureData = StaticGesture.Handedness == Handedness.Right ? rightHandFeatureData : leftHandFeatureData;
            StaticGesture.DetectGesture(featureData); 
        }
    }

    public override GestureFeature GetAllFeaturesInUse()
    {
        if (IsDetecting) { return StaticGesture.FeatureUsed; }
        else { return GestureFeature.None; }
    }
}
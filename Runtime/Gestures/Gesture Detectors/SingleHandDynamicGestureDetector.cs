using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class SingleHandDynamicGestureDetector : GestureDetector
{
    public DynamicGesture DynamicGesture;
    public UnityEvent OnGestureDetected;
    public UnityEvent OnGestureTerminated;

    private void OnEnable()
    {
        IsDetecting = true;
        DynamicGesture.OnGestureTerminated.AddListener(InvokeOnGestureTerminated);
        DynamicGesture.OnGestureDetected.AddListener(InvokeOnGestureDetected);
    }

    private void OnDisable()
    {
        IsDetecting = false;
    }

    /// <summary>
    /// Wrapper for the OnGestureTerminated event in the gesture class.
    /// </summary>
    private void InvokeOnGestureTerminated() {
        OnGestureTerminated?.Invoke();
    }


    /// <summary>
    /// Wrapper for the OnGestureDetected event in the gesture class.
    /// </summary>
    private void InvokeOnGestureDetected() { 
        OnGestureDetected?.Invoke();
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

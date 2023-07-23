using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public abstract class GestureDetector : MonoBehaviour
{
    /// <summary>
    /// Whether this gesture detector is running.
    /// </summary>
    [HideInInspector] public bool IsDetecting;

    /// <summary>
    /// Return the flags of all feature used for detection.
    /// </summary>
    public abstract GestureFeature GetAllFeaturesInUse();

    /// <summary>
    /// Detect the gesture using the feature data.
    /// </summary>
    public abstract void DetectGesture(GestureFeatureData leftHandFeatureData, GestureFeatureData rightHandFeatureData);
}

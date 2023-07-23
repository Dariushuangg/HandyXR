using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public abstract class Gesture : MonoBehaviour {
    [HideInInspector] public GestureFeature FeatureUsed; // set this field in script
    [SerializeField] public Handedness Handedness;

    /// <summary>
    /// Fired when this gesture is detected.
    /// </summary>
    public UnityEvent OnGestureDetected = new();

    public abstract bool DetectGesture(GestureFeatureData featureData);
}

public abstract class StaticGesture : Gesture
{
}

public abstract class DynamicGesture : Gesture
{
}


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
    [HideInInspector] public UnityEvent OnGestureDetected = new UnityEvent();

    public abstract bool DetectGesture(GestureFeatureData featureData);
}

public abstract class StaticGesture : Gesture
{
}

public abstract class DynamicGesture : Gesture
{
    /// <summary>
    /// Fired when this gesture is terminated.
    /// </summary>
    [HideInInspector] public UnityEvent OnGestureTerminated = new UnityEvent();
}


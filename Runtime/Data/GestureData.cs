using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

/// <summary>
/// A class for recording gesture data and hand feature data.
/// </summary>
public class GestureData : MonoBehaviour
{
    public Features Features;
    public List<GestureDetector> GestureDetectors;
    [Header("Debug")]
    public TextMeshProUGUI DebugText;
    [HideInInspector] public RingBuffer<XRHandSnapshot> _RightHandBuffer { private set; get; }
    [HideInInspector] public RingBuffer<XRHandSnapshot> _LeftHandBuffer { private set; get; }
    [HideInInspector] public RingBuffer<GestureFeatureData> _RightHandFeatureBuffer { private set; get; }
    [HideInInspector] public RingBuffer<GestureFeatureData> _LeftHandFeatureBuffer { private set; get; }
    private static readonly List<XRHandSubsystem> _Subsystems = new List<XRHandSubsystem>();
    private XRHandSubsystem _Subsystem;

    private void OnEnable()
    {
        SubsystemManager.GetSubsystems(_Subsystems);
        if (_Subsystems.Count == 0)
        {
            Debug.LogError("XR Hand Subsystem not found.");
            return;
        }
        _Subsystem = _Subsystems[0];
        _Subsystem.updatedHands += OnUpdatedHands;
        _RightHandBuffer = new RingBuffer<XRHandSnapshot>();
        _LeftHandBuffer = new RingBuffer<XRHandSnapshot>();
        _RightHandFeatureBuffer = new RingBuffer<GestureFeatureData>();
        _LeftHandFeatureBuffer = new RingBuffer<GestureFeatureData>();
    }

    private void OnUpdatedHands(XRHandSubsystem handSubsystem,
    XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
    XRHandSubsystem.UpdateType updateType)
    {
        // Only use data from logics update
        if (updateType == XRHandSubsystem.UpdateType.BeforeRender) return;

        // Hand data update
        XRHand leftHand = handSubsystem.leftHand;
        XRHand rightHand = handSubsystem.rightHand;
        XRHandSnapshot leftHandSnapshot = new XRHandSnapshot(){};
        XRHandSnapshot rightHandSnapshot = new XRHandSnapshot(){};
        leftHandSnapshot.TakeSnapShot(leftHand);
        rightHandSnapshot.TakeSnapShot(rightHand);
        _RightHandBuffer.Append(rightHandSnapshot);
        _LeftHandBuffer.Append(leftHandSnapshot);

        // Extract hand feature data
        GestureFeatureData rightHandData = Features.ExtractFeatures(Handedness.Right, rightHand, ExtractUsedFeature());
        GestureFeatureData leftHandData = Features.ExtractFeatures(Handedness.Left, leftHand, ExtractUsedFeature());
        // DebugText.text = rightHandData.Print() + "\n" + leftHandData.Print();   
        _RightHandFeatureBuffer.Append(rightHandData);
        _LeftHandFeatureBuffer.Append(leftHandData);

        // Detect gestures based on raw hand data and hand feature data
        foreach(GestureDetector detector in GestureDetectors)
        {
            detector.DetectGesture(leftHandData, rightHandData);
        }   
    }

    private GestureFeature ExtractUsedFeature() 
    {
        GestureFeature usedFeatures = GestureFeature.None;
        foreach (GestureDetector detector in GestureDetectors)
        {
            usedFeatures |= detector.GetAllFeaturesInUse();
        }
        usedFeatures = GestureFeature.All; // Testing
        return usedFeatures;
    }


    /// <summary>
    /// Request the last <paramref name="frameCount"/> frames of XRHand data. 
    /// This is slow, so only use it when needing custom detection logics that
    /// occurs only once. Otherwise, add the logics into the feature library. 
    /// </summary>
    public static XRHandSnapshot[] RequestLastUpdatedHandDatas(Handedness handedness, int frameCount) 
    {
        GestureData dataSource = GameObject.Find("Data Source").GetComponent<GestureData>();
        if (handedness == Handedness.Right) return dataSource._RightHandBuffer.GetInterval(frameCount);
        else return dataSource._LeftHandBuffer.GetInterval(frameCount);
    }

    public static XRHandSnapshot RequestLastUpdateHandData(Handedness handedness)
    {
        return RequestLastUpdatedHandDatas(handedness, 1)[0];
    }
}

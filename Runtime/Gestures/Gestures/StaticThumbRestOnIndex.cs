using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;

public class StaticThumbRestOnIndex : StaticGesture
{
    [Header("Debug")]
    public TextMeshProUGUI debugText;

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        if (featureData.Handedness != Handedness)
            return false;

        // standard feature logics
        if (featureData.MiddleExtendedState + featureData.RingExtendedState + featureData.LittleExtendedState != 0)
            return false;

        // custom hand data logics
        XRHandSnapshot handData = GestureData.RequestLastUpdateHandData(Handedness);
        if (!(handData.GetJoint(XRHandJointID.IndexProximal).TryGetPose(out var indexProximalPose)
            && handData.GetJoint(XRHandJointID.ThumbTip).TryGetPose(out var thumbTipPose)
            && handData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose)
            && handData.GetJoint(XRHandJointID.IndexIntermediate).TryGetPose(out var indexIntermediatePose)
            ))
        {
            return false;
        }
        Vector3 intermediateToTip = indexTipPose.position - indexIntermediatePose.position;
        Vector3 intermediateToProximal = indexProximalPose.position - indexIntermediatePose.position;
        Vector3 halfwayVector = intermediateToTip + intermediateToProximal;
        Vector3 planeNormal = Vector3.Cross(intermediateToProximal, halfwayVector);
        float thumbTipToPlaneDistance = Vector3.Dot(thumbTipPose.position - indexIntermediatePose.position, planeNormal.normalized);

        if (thumbTipToPlaneDistance < 0.015f)
        {
            debugText.text = "Static Thumb Rest On Index";
            OnGestureDetected?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        FeatureUsed |= GestureFeature.MiddleExtendedState;
        FeatureUsed |= GestureFeature.RingExtendedState;
        FeatureUsed |= GestureFeature.LittleExtendedState;
    }
}

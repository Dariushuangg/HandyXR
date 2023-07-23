using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class StaticPinch : StaticGesture
{
    private float lastPinchTime = -1;
    [SerializeField] private float pinchInterval = 0.4f;
    [SerializeField] private float minPinchSpeed = 0.2f;
    public override bool DetectGesture(GestureFeatureData featureData)
    {
        // Early exit for for multiple triggers
        if (lastPinchTime != -1) { if (Time.time - lastPinchTime < pinchInterval) return false; }

        XRHandSnapshot lastHandData = GestureData.RequestLastUpdateHandData(Handedness);
        if (!(lastHandData.GetJoint(XRHandJointID.ThumbTip).TryGetPose(out var thumbTipPose)
            && lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose)))
            return false;

        float dist = (thumbTipPose.position - indexTipPose.position).magnitude;
        if (dist < 0.01f)
        {
            if (lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetLinearVelocity(out var speed))
            {
                if (speed.magnitude > minPinchSpeed) 
                {
                    lastPinchTime = Time.time;
                }
            }
            return true;
        }
        else return false;
    }
}

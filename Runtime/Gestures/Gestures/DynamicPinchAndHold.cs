using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class DynamicPinchAndHold : DynamicGesture
{
    public UnityEvent OnGestureTerminated;
    public StaticPinch staticPinch;
    private float pinchHoldTime = 0f;
    private bool isHolding = false;

    private void Start()
    {
        FeatureUsed |= staticPinch.FeatureUsed;
    }

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        if (!isHolding)
        {
            isHolding = staticPinch.DetectGesture(featureData);
            if (isHolding) 
            { 
                OnGestureDetected.Invoke();
                return true;
            } else return false;
        }
        else
        {
            XRHandSnapshot lastHandData = GestureData.RequestLastUpdateHandData(Handedness);
            if (!(lastHandData.GetJoint(XRHandJointID.ThumbTip).TryGetPose(out var thumbTipPose)
                && lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose)))
                return false;

            float dist = (thumbTipPose.position - indexTipPose.position).magnitude;
            if (dist < 0.03f)
            {
                // not invoking OnGestureDetected() because static pinch already did
                pinchHoldTime += Time.deltaTime;
                Debug.Log("Holding...");
                return true;
            }
            else
            {
                pinchHoldTime = 0;
                isHolding = false;
                Debug.Log("Stoped!");
                OnGestureTerminated.Invoke();
                return false;
            }
        }
    }

    public bool IsHolding() => isHolding;
    public float GetPinchHoldTime() 
    { 
        if (!isHolding) return -1;
        else return pinchHoldTime;
    }

}

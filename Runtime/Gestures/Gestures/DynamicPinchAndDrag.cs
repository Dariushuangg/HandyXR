using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class DynamicPinchAndDrag : DynamicGesture
{
    public DynamicPinchAndHold pinchAndHold;
    private bool isDragging = false;
    private Vector3 startPos;
    private Vector3 currPos;

    private void Start()
    {
        FeatureUsed |= pinchAndHold.FeatureUsed;
        pinchAndHold.OnGestureDetected.AddListener(() => { DragStart(); });
        pinchAndHold.OnGestureTerminated.AddListener(() => { EndDrag(); });
    }

    public override bool DetectGesture(GestureFeatureData featureData)
    {
        Debug.Log(isDragging);
        if (isDragging)
        {
            XRHandSnapshot lastHandData = GestureData.RequestLastUpdateHandData(Handedness);
            if (lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose))
            {
                currPos = indexTipPose.position;
                return true;
            }
            else return false;
        }
        else return false;
    }

    /// <summary>
    /// When pinch is detected, the position of the index tip at that frame is the start position of the drag.
    /// </summary>
    private void DragStart() 
    {
        XRHandSnapshot lastHandData = GestureData.RequestLastUpdateHandData(Handedness);
        if (lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose)) 
        {
            startPos = indexTipPose.position;
            isDragging = true;
            OnGestureDetected.Invoke();
        }
    }

    /// <summary>
    /// When the pinch is no longer held, the drag is ended.
    /// </summary>
    private void EndDrag() 
    {
        startPos = Vector3.zero;
        isDragging = false;
    }

    public bool GetStartAndCurr(out Vector3 startPos, out Vector3 currPos)
    {
        if (!isDragging)
        {
            startPos = Vector3.zero;
            currPos = Vector3.zero;
            return false;
        }
        else
        {
            startPos = this.startPos;
            currPos = this.currPos;
            return true;
        }
    }
}

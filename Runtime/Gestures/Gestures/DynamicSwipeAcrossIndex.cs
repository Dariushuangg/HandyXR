using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class DynamicSwipeAcrossIndex : DynamicGesture
{
    public UnityEvent LeftToRight;
    public UnityEvent RightToLeft;
    public UnityEvent Reset;

    [SerializeField] private StaticThumbRestOnIndex StaticThumbRestOnIndexGesture;
    public override bool DetectGesture(GestureFeatureData featureData)
    {
        if (featureData.Handedness != Handedness)
            return false;

        if (!StaticThumbRestOnIndexGesture.DetectGesture(featureData))
            return false;

        Reset?.Invoke();
        XRHandSnapshot lastHandData = GestureData.RequestLastUpdateHandData(Handedness);
        if (!(lastHandData.GetJoint(XRHandJointID.IndexIntermediate).TryGetPose(out var indexIntermediatePose)
            && lastHandData.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var indexTipPose)))
            return false;
        Vector3 directionalVector = (indexTipPose.position - indexIntermediatePose.position).normalized;

        // custom hand data logics
        XRHandSnapshot[] handDatas = GestureData.RequestLastUpdatedHandDatas(Handedness, 400);
        float[] coords = GetSlideCoordinateOnIndex(handDatas, directionalVector, indexIntermediatePose.position);
        if (coords.Max() - coords.Min() < 0.01f)
        {
            Debug.Log("Max " + coords.Max() + " Min " + coords.Min());
            return false;
        }
        
        float start = coords[0];
        float end = coords[coords.Length - 1];
        if (start > end)
        {
            // LeftToRight?.Invoke();
        }
        else
        {
            RightToLeft?.Invoke();
        }

        return true;
    }

    private float[] GetSlideCoordinateOnIndex(XRHandSnapshot[] handDatas, Vector3 directionalVector, Vector3 basePos) 
    {
        Pose ThumbTipPose;
        float[] coords = new float[handDatas.Length];
        for (int i = 0; i < handDatas.Length; i++) 
        {
            XRHandSnapshot hand = handDatas[i];
            if (!hand.GetJoint(XRHandJointID.ThumbTip).TryGetPose(out ThumbTipPose)) 
            { 
                coords[i] = 0; 
                Debug.LogError("Slide Error"); 
            }
            Vector3 v = ThumbTipPose.position - basePos;
            coords[i] = Vector3.Dot(v, directionalVector);
        }
        return coords;
    }

    
}

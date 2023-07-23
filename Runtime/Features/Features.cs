using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

[System.Flags]
public enum GestureFeature
{
    None = 0,
    All = ~0, // for testing
    ThumbExtendedState = 1 << 0,
    IndexExtendedState = 1 << 1,
    MiddleExtendedState = 1 << 2,
    RingExtendedState = 1 << 3,
    LittleExtendedState = 1 << 4,
    PalmWorldOrientationVec3 = 1 << 5,
    FingerTipsDistanceFloat = 1 << 6,
}

public struct GestureFeatureData { 
    public Handedness Handedness;
    public int ThumbExtendedState; // 0 = grab, 1 = natural, 2 = extended
    public int IndexExtendedState; 
    public int MiddleExtendedState;
    public int RingExtendedState;
    public int LittleExtendedState;
    public Vector3 PalmWorldOrientationVec3;
    public float FingerTipsDistanceFloat;

    public string Print() 
    {
        string s = "";
        s += "IndexExtendedBool: " + IndexExtendedState + "\n";
        s += "MiddleExtendedBool: " + MiddleExtendedState + "\n";
        s += "RingExtendedBool: " + RingExtendedState + "\n";
        s += "LittleExtendedBool: " + LittleExtendedState + "\n";
        s += "PalmWorldOrientationVec3: " + PalmWorldOrientationVec3.ToString() + "\n";
        s += "FingerTipsDistanceFloat: " + FingerTipsDistanceFloat + "\n";
        return s;
    }
}

public delegate bool FeatureExtractor(ref GestureFeatureData featureData, XRHand hand);

public class Features : MonoBehaviour
{
    Dictionary<GestureFeature, FeatureExtractor> featureExtractors = new Dictionary<GestureFeature, FeatureExtractor>();

    private int FingerExtended(ref GestureFeatureData featureData, Pose wristPose, Pose tipPose, Pose intermediatePose, Pose proximalPose, Pose distalPose, Pose metacarpalPose) 
    {
        Vector3 ProximalToMetacarpal = metacarpalPose.position - proximalPose.position;
        Vector3 ProximalToIntermediate = intermediatePose.position - proximalPose.position;
        float MainAngle = Vector3.Angle(ProximalToIntermediate, ProximalToMetacarpal);
        if (MainAngle > 165)
        {
            return 2;
        }
        else
        {
            var wristToTip = tipPose.position - wristPose.position;
            var wristToProximal = proximalPose.position - wristPose.position;
            if (wristToProximal.sqrMagnitude < wristToTip.sqrMagnitude)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public bool IndexExtended(ref GestureFeatureData featureData, XRHand hand)
    {
        if (!(hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out var wristPose)
            && hand.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var tipPose)
            && hand.GetJoint(XRHandJointID.IndexIntermediate).TryGetPose(out var intermediatePose)
            && hand.GetJoint(XRHandJointID.IndexProximal).TryGetPose(out var proximalPose)
            && hand.GetJoint(XRHandJointID.IndexDistal).TryGetPose(out var distalPose)
            && hand.GetJoint(XRHandJointID.IndexMetacarpal).TryGetPose(out var metacarpalPose)
            ))
        {
            return false;
        }

        int state = FingerExtended(ref featureData, wristPose, tipPose, intermediatePose, proximalPose, distalPose, metacarpalPose);
        featureData.IndexExtendedState = state;
        return true;
    }

    public bool MiddleExtended(ref GestureFeatureData featureData, XRHand hand)
    {
        if (!(hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out var wristPose)
            && hand.GetJoint(XRHandJointID.MiddleTip).TryGetPose(out var tipPose)
            && hand.GetJoint(XRHandJointID.MiddleIntermediate).TryGetPose(out var intermediatePose)
            && hand.GetJoint(XRHandJointID.MiddleProximal).TryGetPose(out var proximalPose)
            && hand.GetJoint(XRHandJointID.MiddleDistal).TryGetPose(out var distalPose)
            && hand.GetJoint(XRHandJointID.MiddleMetacarpal).TryGetPose(out var metacarpalPose)
            ))
        {
            return false;
        }

        int state = FingerExtended(ref featureData, wristPose, tipPose, intermediatePose, proximalPose, distalPose, metacarpalPose);
        featureData.MiddleExtendedState = state;
        return true;
    }

    public bool RingExtended(ref GestureFeatureData featureData, XRHand hand)
    {
        if (!(hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out var wristPose)
            && hand.GetJoint(XRHandJointID.RingTip).TryGetPose(out var tipPose)
            && hand.GetJoint(XRHandJointID.RingIntermediate).TryGetPose(out var intermediatePose)
            && hand.GetJoint(XRHandJointID.RingProximal).TryGetPose(out var proximalPose)
            && hand.GetJoint(XRHandJointID.RingDistal).TryGetPose(out var distalPose)
            && hand.GetJoint(XRHandJointID.RingMetacarpal).TryGetPose(out var metacarpalPose)
            ))
        {
            return false;
        }

        int state = FingerExtended(ref featureData, wristPose, tipPose, intermediatePose, proximalPose, distalPose, metacarpalPose);
        featureData.RingExtendedState = state;
        return true;
    }

    public bool LittleExtended(ref GestureFeatureData featureData, XRHand hand)
    {
        if (!(hand.GetJoint(XRHandJointID.Wrist).TryGetPose(out var wristPose)
            && hand.GetJoint(XRHandJointID.LittleTip).TryGetPose(out var tipPose)
            && hand.GetJoint(XRHandJointID.LittleIntermediate).TryGetPose(out var intermediatePose)
            && hand.GetJoint(XRHandJointID.LittleProximal).TryGetPose(out var proximalPose)
            && hand.GetJoint(XRHandJointID.LittleDistal).TryGetPose(out var distalPose)
            && hand.GetJoint(XRHandJointID.LittleMetacarpal).TryGetPose(out var metacarpalPose)
            ))
        {
            return false;
        }

        int state = FingerExtended(ref featureData, wristPose, tipPose, intermediatePose, proximalPose, distalPose, metacarpalPose);
        featureData.LittleExtendedState = state;
        return true;
    }

    private void Awake()
    {
        featureExtractors.Add(GestureFeature.IndexExtendedState, IndexExtended);
        featureExtractors.Add(GestureFeature.MiddleExtendedState, MiddleExtended);
        featureExtractors.Add(GestureFeature.RingExtendedState, RingExtended);
        featureExtractors.Add(GestureFeature.LittleExtendedState, LittleExtended);
    }

    public GestureFeatureData ExtractFeatures(Handedness handedness, XRHand hand, GestureFeature flag) { 
        GestureFeatureData featureData = new GestureFeatureData();
        foreach(KeyValuePair<GestureFeature, FeatureExtractor> featureExtractor in featureExtractors)
        {
            if ((flag & featureExtractor.Key) != 0)
            {
                featureExtractor.Value(ref featureData, hand);
            }
        }
        featureData.Handedness = handedness;
        return featureData;
    }

    #region Helper Functions
    public static float GetVectorsAngle(Vector3 v1, Vector3 v2)
    {
        return Vector3ToAngle360(v1, v2);
    }

    public static float Vector3ToAngle360(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);
        return cross.y > 0 ? angle : 360 - angle;
    }

    #endregion
}

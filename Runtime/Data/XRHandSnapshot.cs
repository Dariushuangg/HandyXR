using System;
using System.Collections.Generic;
using UnityEngine.XR.Hands;
using UnityEngine;

/// <summary>  A snapshot of <c>XRHand</c> data at a specific point in time. </summary>

/// <remarks>
/// In <c>XRHand</c>, The joint data is stored in an internal native array that isn't copied if you
/// make a shallow copy of an <c>XRHand</c> object. This native array is modified each time
/// A hand update occurs. Calling this function from a copied <c>XRHand</c> retrieves the latest hand data, 
/// not the data from when the hand object was copied. To take a snapshot of the joint data for use later, 
/// we must copy each individual <see cref="XRHandJoint"/> object.
/// </remarks>
public struct XRHandSnapshot 
{
    public List<XRHandJoint> m_Joints;
    public XRHandJoint GetJoint(XRHandJointID id) => m_Joints[id.ToIndex()];
    public Handedness handedness => m_Handedness;
    Handedness m_Handedness;
    public void TakeSnapShot(XRHand hand) {
        m_Handedness = hand.handedness;
        m_Joints = new List<XRHandJoint>(26);
        for (int i = 0; i < 26; i++)
        {
            m_Joints.Add(hand.GetJoint(XRHandJointIDUtility.FromIndex(i)));
        }
    }
}

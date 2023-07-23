using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPinchAndDrag : MonoBehaviour
{
    public DynamicPinchAndDrag pinchAndDrag;
    public GameObject startBall;
    public GameObject currBall;

    private void Update()
    {
        Vector3 startPos;
        Vector3 currPos;
        if (pinchAndDrag.GetStartAndCurr(out startPos, out currPos)) 
        {
            startBall.transform.position = startPos;
            currBall.transform.position = currPos;
        }
    }
}

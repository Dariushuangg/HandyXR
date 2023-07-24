using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColor : MonoBehaviour
{
    public Material defaultMat;
    public Timer resetTimer;

    // Update is called once per frame
    private void Start()
    {
        resetColor();
    }

    private void resetColor() { 
        GetComponent<MeshRenderer>().material = defaultMat;
        resetTimer.StartTimer(0.5f, () => { resetColor(); });
    }
}

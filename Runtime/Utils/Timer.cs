using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public delegate void OnTimerExpiresOutcome();
    private OnTimerExpiresOutcome onTimerExpiresOutcome;
    private float MaxDuration = -1;
    private bool HasTimerStarted = false;
    public void StartTimer(float MaxDuration, OnTimerExpiresOutcome Outcome)
    {
        Debug.Log("Timer started");
        this.MaxDuration = MaxDuration;
        onTimerExpiresOutcome = Outcome;
        HasTimerStarted = true;
        StartCoroutine(StartTimer());
    }

    public void StopTimer()
    {
        if (!HasTimerStarted) { Debug.LogError("Attempting to stop a timer that hasn't started"); return; }
        StopCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(MaxDuration);
        onTimerExpiresOutcome.Invoke();
        Debug.Log("Timer expired; Outcome triggered.");
    }
}

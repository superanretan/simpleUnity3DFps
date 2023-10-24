using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleEventsManager : MonoBehaviour
{
  public static ObstacleEventsManager Instance { get; private set; }

  private void Awake()
  {
    Instance = this;
  }


  public void ObstacleDestroyEventToTrigger(UnityEvent eventToTrigger, float delayTime)
  {
  //  UnityEvent _eventToTrigger = eventToTrigger;
    StartCoroutine(InvokeEventWithDelayIe( eventToTrigger,delayTime));
  }

  IEnumerator InvokeEventWithDelayIe(UnityEvent eventToTrigger, float delay)
  {
    yield return new WaitForSeconds(delay);
    eventToTrigger.Invoke();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event : MonoBehaviour
{
    public UnityEvent onEnter, onExit;

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();
    }
}

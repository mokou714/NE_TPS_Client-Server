using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraTransition : MonoBehaviour
{
    [Range(0f, 20f)] [SerializeField] private float speed;
    public Transform[] targetTransforms;
    public int index = 0;
    private bool _started;
    
    private List<Action> OnTransitionEvents = new List<Action>();
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = targetTransforms[index].position;
        transform.eulerAngles = targetTransforms[index].eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
    }

    public void NextTransition()
    {
        _started = true;
        index = (index + 1) % targetTransforms.Length;
        InvokeEvents();
    }

    public void PreviousTransition()
    {
        if (index == 0) return;
        _started = true;
        index--;
        InvokeEvents();
    }

    public void SetOnTransitionEvents(IEnumerable<Action> events)
    {
        OnTransitionEvents.AddRange(events);
    }

    private void InvokeEvents()
    {
        foreach (var e in OnTransitionEvents)
        {
            e();
        }
        OnTransitionEvents.Clear();
    }

    private void Transition()
    {
        if (!_started) return;
        transform.position = Vector3.Lerp(transform.position, targetTransforms[index].position, speed * Time.deltaTime);
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetTransforms[index].eulerAngles, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetTransforms[index].position) < 0.01f)
        {
            transform.position = targetTransforms[index].position;
            _started = false;
        }
    }
}

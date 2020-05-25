using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    public bool isMoving;

    [SerializeField] private GameObject[] balloonMeshes;
    [SerializeField] private PresentBox presentBox;
    
    private Transform _destination;
    private Vector3 _direction;
    private float _speed;
    private float _startTime;
    private bool boxDropped = true;

    //other component
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(Transform spawn, Transform destination, float moveSpeed)
    {
        _destination = destination;
        _speed = moveSpeed;
        transform.position = spawn.position;
        _direction = (destination.position - spawn.position).normalized;
        _startTime = Time.time;
        isMoving = true;
        boxDropped = false;
        foreach (var mesh in balloonMeshes)
        {
            mesh.SetActive(true);
        }
        presentBox.gameObject.SetActive(true);
        presentBox.Init(transform);
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        CheckLifetime();
    }

    public void Dropdown()
    {
        if(boxDropped) return;
        presentBox.Dropdown();
        audioSource.Play();
        boxDropped = true;
        Reset();
    }


    private void Animate()
    {
        if (boxDropped) return;
        transform.position +=  Mathf.Sin(Time.time * 2f) * Time.deltaTime * Vector3.up;
        transform.Translate( Time.deltaTime * _speed *_direction);

        if (Vector3.Distance(transform.position, _destination.position) < 1f)
        {
            Reset();
        }
    }

    private void CheckLifetime()
    {
        if (Time.time > _startTime + 120f)
        {
            Reset();
            presentBox.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        isMoving = false;
        foreach (var mesh in balloonMeshes)
        {
            mesh.SetActive(false);
        }
    }


}

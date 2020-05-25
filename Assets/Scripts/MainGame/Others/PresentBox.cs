using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PresentBox : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _ballon;
    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private bool isDropping;

    [SerializeField] private GameObject[] pickables;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Transform balloon)
    {
        _ballon = balloon;
        _localPosition = transform.localPosition;
        _localRotation = transform.localRotation;
    }

    public void Dropdown()
    {
        _rigidbody.isKinematic = false;
        transform.parent = null;
        isDropping = true;
    }

    private void Reset()
    {
        _rigidbody.isKinematic = true;
        transform.parent = _ballon;
        transform.localPosition = _localPosition;
        transform.localRotation = _localRotation;
        isDropping = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (isDropping)
        {
            var obj = Instantiate(pickables[Random.Range(0, pickables.Length)], null);
            obj.transform.position += transform.position;
            Reset();
            gameObject.SetActive(false);
        }
    }
}


using System;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    [Range(0f,50f)]
    public float rotationSpeed;
    [Range(0f,50f)]
    public float translateFrequency;
    [Range(0f,50f)]
    public float translateAmplitude;
    [SerializeField] private GameObject model;
    protected PlayerController _player;
    protected abstract void PickUp();

    protected virtual void Update()
    {
        Animate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject.GetComponent<PlayerController>();
            PickUp();
        }
    }

    private void Animate()
    {
        model.transform.Rotate(rotationSpeed*10f*Time.deltaTime*Vector3.up, Space.World);
        model.transform.Translate(Mathf.Sin(Time.time*translateFrequency/10f) * translateAmplitude /10f * Time.deltaTime * Vector3.up, Space.World);
    }
}

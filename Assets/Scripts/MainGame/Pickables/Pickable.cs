
using System;
using System.Security.Cryptography;
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
    [SerializeField] private AudioClip pickupSFX;

    protected virtual void PickUp(GameObject obj)
    {
        obj.GetComponent<PlayerController>().Center.GetComponent<AudioSource>().PlayOneShot(pickupSFX, 2f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PickUp(other.gameObject);
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        Animate();
    }
    
    
    private void Animate()
    {
        model.transform.Rotate(rotationSpeed*10f*Time.deltaTime*Vector3.up, Space.World);
        model.transform.Translate(Mathf.Sin(Time.time*translateFrequency/10f) * translateAmplitude /10f * Time.deltaTime * Vector3.up, Space.World);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BalloonManager : MonoBehaviour
{
    public float movingSpeed;
    public float minWaitingTime;
    public float maxWaitingTime;
    
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private Balloon balloon;

    private bool isActivated;

    private static BalloonManager _instance;

    public static BalloonManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        Activate();
    }

    public void Activate()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(BallonGenerator());
        }
    }

    public void Deactivate()
    {
        isActivated = false;
        StopAllCoroutines();
    }

    private IEnumerator BallonGenerator()
    {
        while (isActivated)
        {
            yield return new WaitUntil(()=>!balloon.isMoving);
            var interval = Random.Range(minWaitingTime, maxWaitingTime);
            yield return new WaitForSeconds(interval);
            var spawnIndex = Random.Range(0, spawnTransforms.Length);
            var leftBound = spawnIndex - 3;
            var rightBound = spawnIndex + 3;

            var destIndex = Random.Range(rightBound, leftBound + spawnTransforms.Length + 1) % spawnTransforms.Length;
            
            balloon.Init(spawnTransforms[spawnIndex], spawnTransforms[destIndex], movingSpeed);
        }
    }
}

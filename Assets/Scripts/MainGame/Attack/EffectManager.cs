using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//only handle skill effect, not damage(by healthmanager)
public class EffectManager : MonoBehaviour
{
    private AIStatus _status;
    private EnemyAIController _aiController;

    //particle effect
    [SerializeField] private GameObject dizzy;
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private ParticleSystem gunFire;

    private float _startTime;

    private void Start()
    {
        _status = GetComponent<AIStatus>();
        _aiController = GetComponent<EnemyAIController>();
    }

    public void Stun(float stunningTime)
    {
        if (!_status.isStunned)
        {
            _status.isStunned = true;
            _startTime = Time.time; 
            _status.aiState = AIState.DETECT;
            _aiController.LosePlayer();
            _aiController.StopMoving();
            _aiController.SwitchBodyPosture(false); //change posture after stop walking/running animation!!!
            dizzy.SetActive(true);
            StartCoroutine(StunningTimeChecking(stunningTime)); 
        }
        //if already stunned, extend stunning time
        else {
            _startTime = Time.time; 
        } 
    }


    public void BloodEffect()
    {
        blood.Play();
    }

    public void GunFire()
    {
        gunFire.Play();
    }
    
    //check stunning time every frame
    IEnumerator StunningTimeChecking(float stunningTime)
    {
        while (true)
        {
            if (Time.time > _startTime + stunningTime)
            {
                Debug.Log("Stunned enemies recovered");
                _status.isStunned = false;
                dizzy.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
}

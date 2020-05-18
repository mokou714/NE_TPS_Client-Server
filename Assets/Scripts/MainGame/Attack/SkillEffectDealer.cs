using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//only handle skill effect, not damage(by healthmanager)
public class SkillEffectDealer : MonoBehaviour
{
    private AIStatus _status;
    private EnemyAIController _aiController;

    private float _startTime;

    private void Start()
    {
        _status = GetComponent<AIStatus>();
        _aiController = GetComponent<EnemyAIController>();
    }

    public void Stun(bool stunned, float stunningTime)
    {
        _status.isStunned = stunned;
        if (stunned)
        {
            _startTime = Time.time;
            _status.aiState = AIState.DETECT;
            _aiController.StopMoving();
            StartCoroutine(StunningTimeChecking(stunningTime));
        }
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
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
}

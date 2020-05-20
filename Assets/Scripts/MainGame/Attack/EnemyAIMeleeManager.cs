using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAIMeleeManager : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackCDTime;
    public bool isAttacking;
    private BaseAnimationController _animationController;
    private AIStatus _status;
    private float _lastAttackTime;
   
    
    //other components
    [SerializeField] private Knife _knife;

    void Start()
    {
        _animationController = GetComponent<BaseAnimationController>();
        _status = GetComponent<AIStatus>();
        _knife.SetDamage(attackDamage);
        
    }

    // Update is called once per frame
    void Update()
    {
        KnifeAttack();
        CheckAttackCD();
    }

    private void KnifeAttack()
    {
        if(_status.aiState == AIState.ATTACK && _status.isAlive && !isAttacking)
        {
            _animationController.KnifeAttack();
            _lastAttackTime = Time.time;
            isAttacking = true;
        }
        
    }

    private void CheckAttackCD()
    {
        if(Time.time > _lastAttackTime + attackCDTime)
            isAttacking = false;
    }

}

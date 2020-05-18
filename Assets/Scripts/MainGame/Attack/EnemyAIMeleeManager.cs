using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMeleeManager : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackCDTime;
    private EnemyAIController _aiController;
    private BaseAnimationController _animationController;
    private AIStatus _status;
    private float _lastAttackTime;
    private bool _isAttacking;
    
    //other components
    [SerializeField] private Knife _knife;

    void Start()
    {
        _aiController = GetComponent<EnemyAIController>();
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

    public Vector3 GetAimingDirection()
    {
        var aiController = _aiController;
        return (aiController.EyeTransform.position - aiController.TargetPlayer.Center.position).normalized;
    }
    

    private void KnifeAttack()
    {
        if(_status.aiState == AIState.ATTACK && !_isAttacking)
        {
            _animationController.KnifeAttack();
            _lastAttackTime = Time.time;
            _isAttacking = true;
        }
        
    }

    private void CheckAttackCD()
    {
        if(Time.time > _lastAttackTime + attackCDTime)
            _isAttacking = false;
    }

}

using UnityEngine;
using UnityEngine.UI;

public class BaseHealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] protected int maxHealth;
    protected bool burstMode;
    
    //status
    protected CharacterStatus _status;
    
    //other components
    protected BaseAnimationController _animationController;
    protected EffectManager effectManager;
    
   
    protected virtual void Start()
    {
        _status = GetComponent<CharacterStatus>();
        _animationController = GetComponent<BaseAnimationController>();
        effectManager = GetComponent<EffectManager>();
    }

    public virtual bool DealDamage(int damage)
    {
        if (!_status.isAlive) return false;
        
        health -= damage;

        
        if (health < 0)
        {
            health = 0;
            CharacterDied();
        }
        else
        {
            _animationController.TakeDamage();
        }
        
        effectManager.BloodEffect();

        return true;
    }

    public virtual void IncreaseHealth(int healthPoint)
    {
        if (!_status.isAlive) return;
        
        health += healthPoint;
        if (health > maxHealth)
            health = maxHealth;
        
    }
    
    

    protected virtual void CharacterDied()
    {
       _status.isAlive = false;
       _animationController.Die();
    }

  
}

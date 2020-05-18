using UnityEngine;
using UnityEngine.UI;

public class BaseHealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] protected int maxHealth;
    protected bool burstMode;
    
    //status
    private CharacterStatus _status;
    
    //other components
    private BaseAnimationController _animationController;
    
   
    protected virtual void Start()
    {
        _status = GetComponent<CharacterStatus>();
        _animationController = GetComponent<BaseAnimationController>();
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

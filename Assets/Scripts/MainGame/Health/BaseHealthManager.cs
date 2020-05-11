using UnityEngine;
using UnityEngine.UI;

public class BaseHealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] protected int maxHealth;
    protected bool burstMode;
    
    //status
    private bool _isAlive = true;
    
    //other components
    private BaseAnimationController _animationController;
    
   
    protected virtual void Start()
    {
        _animationController = GetComponent<BaseAnimationController>();
    }

    public virtual void DealDamage(int damage)
    {
        if (!_isAlive) return;
        
        health -= damage;

        if (health > maxHealth)
        {
            health = maxHealth;
            _animationController.TakeDamage();
        }
        else if (health < 0)
        {
            health = 0;
            CharacterDied();
        }
        
    }

    public virtual void IncreaseHealth(int healthPoint)
    {
        if (!_isAlive) return;
        
        health += healthPoint;
        if (health > maxHealth)
            health = maxHealth;
        
    }
    
    

    protected virtual void CharacterDied()
    {
        _isAlive = false;
        _animationController.Die();
    }

  
}

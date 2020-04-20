using UnityEngine;
using UnityEngine.UI;

public class BaseHealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] protected int maxHealth;
    protected bool burstMode;
    
    //other components
    private BaseAnimationController _animationController;
    
   
    protected virtual void Start()
    {
        _animationController = GetComponent<BaseAnimationController>();
    }

    public virtual void DealDamage(int damage)
    {
        health -= damage;
        
        if (health > maxHealth)
            health = maxHealth;
        else if (health < 0)
        {
            health = 0; 
            CharacterDied();
        }
        _animationController.TakeDamage();
    }
    

    protected virtual void CharacterDied()
    {
        
    }

  
}

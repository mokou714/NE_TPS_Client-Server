using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float bodyDisappearingDelay;
    [SerializeField] protected float droppingDelay;

    //status
    protected CharacterStatus _status;
    
    //other components
    protected BaseAnimationController _animationController;
    protected EffectManager effectManager;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip gettingHit;
    [SerializeField] protected AudioClip dying;
    
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

        
        if (health <= 0)
        {
            health = 0;
            audioSource.clip = dying;
            CharacterDied();
        }
        else
        {
            audioSource.clip = gettingHit;
            _animationController.TakeDamage();
        }
        
        effectManager.BloodEffect();
        audioSource.Play();

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
       StartCoroutine(DelayBodyDisappear());
       Debug.Log("Enemy died");
    }
    
    private IEnumerator DelayBodyDisappear()
    {
        yield return new WaitForSeconds(bodyDisappearingDelay);
        Destroy(gameObject);
    }

  
}

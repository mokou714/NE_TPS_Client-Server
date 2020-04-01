using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthNumber;
    [SerializeField] private float healthAnimSpeed;
    private bool burstMode;

    //helper data
    private float _maxHealthBarWidth;
    private float _newHealthBarWidth;
    private bool isChangingHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        initHealthBar();
        StartCoroutine(HealthBarAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHealthAnimation();
    }

    void initHealthBar()
    {
        _maxHealthBarWidth = healthBar.rectTransform.rect.width;
        healthNumber.text = health.ToString();
        Vector2 _min = healthBar.rectTransform.offsetMin;
        Vector2 _max = healthBar.rectTransform.offsetMax;
        _newHealthBarWidth = (float)(maxHealth - health) / maxHealth * _maxHealthBarWidth / 2;
        healthBar.rectTransform.offsetMin = new Vector2(_newHealthBarWidth,_min.y);
        healthBar.rectTransform.offsetMax = new Vector2(-_newHealthBarWidth,_max.y);
    }
    

    public void DealDamage(int damage)
    {
        float widthChange = (float)damage / (maxHealth) * _maxHealthBarWidth / 2;
        _newHealthBarWidth += widthChange;
        
        if (_newHealthBarWidth > _maxHealthBarWidth / 2)
            _newHealthBarWidth = _maxHealthBarWidth / 2;
        else if (_newHealthBarWidth < 0)
            _newHealthBarWidth = 0;
        
        health -= damage;
        
        if (health > maxHealth)
            health = maxHealth;
        else if (health < 0)
        {
            health = 0;
            PlayerDied();
        }

        healthNumber.text = health.ToString();
        
        isChangingHealth = true;
    }

    public void ChangeHealthAnimation()
    {
        if (!isChangingHealth) return;

        Vector2 _min = healthBar.rectTransform.offsetMin;
        Vector2 _max = healthBar.rectTransform.offsetMax;
        float _newWidth = Mathf.Lerp(_min.x,  _newHealthBarWidth, healthAnimSpeed/10f);
        float new_minX = _newWidth;
        float new_maxX = -_newWidth;
        
        if (Mathf.Abs(new_minX - _newHealthBarWidth) < 0.1f)
        {
            new_minX = _newHealthBarWidth;
            new_maxX = -_newHealthBarWidth;
            isChangingHealth = false;
        }

        healthBar.rectTransform.offsetMin = new Vector2(new_minX,_min.y);
        healthBar.rectTransform.offsetMax = new Vector2(new_maxX,_max.y);
        
    }

    void PlayerDied()
    {
        
    }

    IEnumerator HealthBarAnimation()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(3);
            DealDamage(5);
        }

    }
}

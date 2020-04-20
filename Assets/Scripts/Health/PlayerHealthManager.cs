using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthManager : BaseHealthManager{
    
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthNumber;
    [SerializeField] private float healthAnimSpeed;

    //helper data
    private float _maxHealthBarWidth;
    private float _newHealthBarWidth;
    private bool _isChangingHealth;
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
        initHealthBar();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ChangeHealthAnimation();
    }

    void initHealthBar()
    {
        _maxHealthBarWidth = healthBar.rectTransform.rect.width;
        healthNumber.text = health.ToString();
        var _min = healthBar.rectTransform.offsetMin;
        var _max = healthBar.rectTransform.offsetMax;
        _newHealthBarWidth = (float)(maxHealth - health) / maxHealth * _maxHealthBarWidth / 2;
        healthBar.rectTransform.offsetMin = new Vector2(_newHealthBarWidth,_min.y);
        healthBar.rectTransform.offsetMax = new Vector2(-_newHealthBarWidth,_max.y);
    }
    

    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        
        //set up health bar animation
        var widthChange = (float)damage / (maxHealth) * _maxHealthBarWidth / 2;
        _newHealthBarWidth += widthChange;
        
        if (_newHealthBarWidth > _maxHealthBarWidth / 2)
            _newHealthBarWidth = _maxHealthBarWidth / 2;
        else if (_newHealthBarWidth < 0)
            _newHealthBarWidth = 0;

        healthNumber.text = health.ToString();
        _isChangingHealth = true;
    }

    private void ChangeHealthAnimation()
    {
        if (!_isChangingHealth) return;

        var _min = healthBar.rectTransform.offsetMin;
        var _max = healthBar.rectTransform.offsetMax;
        var _newWidth = Mathf.Lerp(_min.x,  _newHealthBarWidth, healthAnimSpeed/10f);
        var new_minX = _newWidth;
        var new_maxX = -_newWidth;
        
        if (Mathf.Abs(new_minX - _newHealthBarWidth) < 0.1f)
        {
            new_minX = _newHealthBarWidth;
            new_maxX = -_newHealthBarWidth;
            _isChangingHealth = false;
        }

        healthBar.rectTransform.offsetMin = new Vector2(new_minX,_min.y);
        healthBar.rectTransform.offsetMax = new Vector2(new_maxX,_max.y);
        
    }



}

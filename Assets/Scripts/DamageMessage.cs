using System;
using UnityEngine;
using UnityEngine.UI;

public class DamageMessage : MonoBehaviour
{
    //components
    private Text damageText;
    private DamageMessageManager _damageMessageManager;
    
    //properties
    private float _lifetime;
    private Vector3 _moveDirection;
    private float _moveSpeed;
    private float _scaleSpeed;
    private float _fadeOutSpeed;
    private int _msgIndex;
    
    //helper data
    private RectTransform _rectTransform;
    private bool _isAnimating;
    private float _startTime;
    private Color _textColor;
    private Vector3 _scale;
    private Vector3 _position;

    private void Awake()
    {
        damageText = GetComponent<Text>();
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        Animate();
        CheckLifetime();
    }

    public void Init(float lifetime, float moveSpeed, float scaleSpeed, float fadeOutSpeed,DamageMessageManager damageMessageManager)
    { 
        _lifetime = lifetime;
        _moveSpeed = moveSpeed;
        _scaleSpeed = scaleSpeed;
        _fadeOutSpeed = fadeOutSpeed;
        _rectTransform = GetComponent<RectTransform>();
        _damageMessageManager = damageMessageManager;
        _textColor = damageText.color;
        _scale = transform.localScale;
    }

    public void Show(int msgIndex, int damage, Vector3 hitCharacterWorldPosition)
    {
        damageText.text = damage.ToString();
        _msgIndex = msgIndex;
        _startTime = Time.time;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(hitCharacterWorldPosition);
        _rectTransform.position = _position = screenPosition;
        
        _moveDirection = Vector3.right;
        
        _isAnimating = true;
        gameObject.SetActive(true);
    }

    void Reset()
    {
        _isAnimating = false;
        gameObject.SetActive(false);
        transform.localScale = _scale;
        damageText.color = _textColor;
        _damageMessageManager.EndShowing(_msgIndex);
    }
    
    void Animate()
    {
        if (!_isAnimating) return;
        var logScalar = Mathf.Log(Time.time - _startTime + 1, 7f);
        var expScalar = 0.5f * Mathf.Pow((Time.time - _startTime), 2);
        
        _rectTransform.localScale = _scaleSpeed * logScalar * Vector3.one;
        _rectTransform.position = _moveSpeed * logScalar * _moveDirection + _position;
        damageText.color = new Color(_textColor.r,_textColor.g,_textColor.b,logScalar>1?0:(1-expScalar*_fadeOutSpeed));
    }

    void CheckLifetime()
    {
        if (_isAnimating && Time.time - _startTime > _lifetime)
            Reset();
    }
    
}

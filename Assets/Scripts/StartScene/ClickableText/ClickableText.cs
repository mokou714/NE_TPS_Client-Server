using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableText : MonoBehaviour
{
    [SerializeField] private Color onSelectColor;
    public bool isClickable;
    private TextMesh _text;
    private Color _defualtColor;
    protected bool mouseDown;
    void Start()
    {
        _text = GetComponent<TextMesh>();
        _defualtColor = _text.color;
    }

    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }

    private void OnMouseEnter()
    {

        if(isClickable)
            _text.color = onSelectColor;
    }

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseExit()
    {
        mouseDown = false;
        _text.color = _defualtColor;
    }

    protected virtual void OnMouseUp()
    {
        mouseDown = false;
    }
}

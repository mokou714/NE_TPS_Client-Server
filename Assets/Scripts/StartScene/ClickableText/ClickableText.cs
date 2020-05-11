using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableText : MonoBehaviour
{
    [SerializeField] private Color onSelectColor;
    private TextMesh _text;
    private Color _defualtColor;

    void Start()
    {
        _text = GetComponent<TextMesh>();
        _defualtColor = _text.color;
    }


    private void OnMouseEnter()
    {
        _text.color = onSelectColor;
    }

    private void OnMouseExit()
    {
        _text.color = _defualtColor;
    }
}

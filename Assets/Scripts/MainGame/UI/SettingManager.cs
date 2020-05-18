using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    private bool _isDisplaying;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Display();
    }

    public void Display()
    {
        _isDisplaying = !_isDisplaying;
        menuUI.SetActive(_isDisplaying);
        Time.timeScale = _isDisplaying ? 0 : 1;
        Cursor.visible = _isDisplaying;
    }
    
}

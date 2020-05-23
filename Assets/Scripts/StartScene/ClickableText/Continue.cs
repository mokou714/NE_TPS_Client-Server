
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : ClickableText
{

    protected override void OnMouseUp()
    {
        if (isClickable && mouseDown)
        {
            SceneManager.LoadScene(1);
        }
        base.OnMouseUp();
        
    }
}
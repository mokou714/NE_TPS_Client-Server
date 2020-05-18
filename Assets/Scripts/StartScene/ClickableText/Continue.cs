
using UnityEngine.SceneManagement;

public class Continue : ClickableText
{

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        SceneManager.LoadScene(1);
    }
}
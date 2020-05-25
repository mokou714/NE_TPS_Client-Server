using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIconAnim : MonoBehaviour
{
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private int maxAngle;
    [SerializeField] private float speed;
    
    private bool isAnimating = false;
    private int index = 0;
    private int size;
    private bool burstMode = false;

    // Start is called before the first frame update
    void Start()
    {
        size = bullets.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    public void SwitchMode(bool isBurstMode)
    {
        isAnimating = true;
        burstMode = isBurstMode;
    }

    void Animate()
    {
        if (!isAnimating) return;

        float targetAngle = burstMode ? maxAngle : 0f;
        
        for (int i = index; i < size; ++i)
        {
            float newZAngle = Mathf.Lerp((bullets[i].transform.rotation.eulerAngles.z - 360) % 360,
                (float) i / (size - 1) * targetAngle, speed / 10);
            bullets[i].transform.rotation = Quaternion.Euler(Vector3.forward * newZAngle);
        }

        float angleDiff = (bullets[index].transform.rotation.eulerAngles.z - 360) % 360 -
                          (float) index / (size - 1) * targetAngle;
        
            
        if (Mathf.Abs(angleDiff)< 0.02f)
        {
            bullets[index].transform.rotation = Quaternion.Euler(Vector3.forward * (float) index / (size - 1) * targetAngle);
            index++;
        }

        if (index >= size)
        {
            index = 0;
            isAnimating = false;
        }
    }
    
    
    
    
    
}

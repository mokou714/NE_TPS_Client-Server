using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBackButton : MonoBehaviour
{
    [SerializeField] private CameraTransition cameraTransition;

    public void Click()
    {
        cameraTransition.PreviousTransition();
        //not display on the connection menu
        if (cameraTransition.index == 0)
        {
            gameObject.SetActive(false);
        }
        
    }
}

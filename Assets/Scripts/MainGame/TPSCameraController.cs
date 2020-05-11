using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 aimingModePosition;
    private Vector3 _defaultLocalPosition;
    private Camera _camera;

    //camera setting 
    public bool revertY;
    public bool revertX;
    public Vector2 VerticalAngleRange;
    
    //rotation pivot
    public Transform pivot;
    
    //helper data
    private Vector3 _lastMousePos;
    private Vector3 _lastVerticalDirection;
    private Vector3 _lastHorizontalDirection;
    private bool _isAiming;
    private bool _isSwitchingAiming;
    
    //other components
    [SerializeField] private PlayerShootManager _shootManager;
    [SerializeField] private PlayerController _playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        _lastMousePos = Input.mousePosition;
        _lastHorizontalDirection = _lastVerticalDirection = Vector3.forward;
        pivot = transform.parent;
        _defaultLocalPosition = transform.localPosition;
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAiming();
    }

    public void SwitchAimingMode()
    { 
        if (_isSwitchingAiming) return;
        
        _isSwitchingAiming = true;
        _isAiming = !_isAiming;
        StartCoroutine(CheckSwitchingAiming());
        
    }

    private void LateUpdate()
    {
        MouseControl();
    }

    void MouseControl()
    {
        if (_isSwitchingAiming) return;
        
        //rotate pivot along world Y axis
        int rX = revertX ? -1 : 1;
        int rY = revertY ? -1 : 1;
        Vector3 currentMousePos = Input.mousePosition;
        float deltaX = currentMousePos.x - _lastMousePos.x;
        float deltaY = currentMousePos.y - _lastMousePos.y;
        pivot.Rotate(Vector3.up, Time.deltaTime*sensitivity*deltaX*rX,Space.World);
        //rotate self along local X axis
        transform.Rotate(Vector3.right, -Time.deltaTime*sensitivity*deltaY*rY,Space.Self); 
    
        _lastMousePos = currentMousePos;
    
        //clamp to angle range
        Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x,0,0);
        if (newRotation.x < VerticalAngleRange.x+360 && newRotation.x > VerticalAngleRange.y)
        {
            float toMax = newRotation.x - VerticalAngleRange.y;
            float toMin = VerticalAngleRange.x + 360 - newRotation.x;
            newRotation.x = toMax < toMin ? VerticalAngleRange.y : VerticalAngleRange.x;
        }
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    void SwitchAiming()
    {
        if(!_isSwitchingAiming) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, (_isAiming ? aimingModePosition : _defaultLocalPosition), 0.15f);
    }
    
    //coroutines
    IEnumerator CheckSwitchingAiming()
    {
        if (_isAiming)
        {
            yield return new WaitUntil(()=>Vector3.Distance(transform.localPosition, aimingModePosition) < 0.02f);
            transform.localPosition = aimingModePosition;
            
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _defaultLocalPosition, 0.5f);
            yield return new WaitUntil(()=>Vector3.Distance(transform.localPosition, _defaultLocalPosition) < 0.02f);
            transform.localPosition = _defaultLocalPosition;
        }
        
        _isSwitchingAiming = false;

    }
    
    //accessors
    public Camera getCamera()
    {
        return _camera;
    }

    public bool IsSwitchingAiming()
    {
        return _isSwitchingAiming;
    }
}

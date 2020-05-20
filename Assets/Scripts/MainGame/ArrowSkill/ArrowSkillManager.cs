
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArrowSkillManager : MonoBehaviour
{
    //data
    public int arrowCount;
    
    //proerties
    [SerializeField] private Text arrowCountUI;
    [SerializeField] private BaseArrow[] regularArrow;
    [SerializeField] private BaseArrow[] piercingArrow;
    [SerializeField] private BaseArrow[] stunningArrow;
    [SerializeField] private float arrowLifetime;
    [SerializeField] private float minShootForce;
    [SerializeField] private float maxShootForce;
    [SerializeField] private float maxDrawingPosOffset;
    [SerializeField] private float drawingSpeed;
    [SerializeField] private float shootCD;
    [SerializeField] private Transform arrowOrigin;
    [SerializeField] private int layerMask;
    
    //helper data
    public bool inAimingState;
    private float _lastShootTime;
    private int _chosenArrowType;
    [SerializeField]private bool _isDrawing;
    private float _currentForce;
    private Vector3 _defaultArrowOriginPosition = new Vector3();
    private Quaternion _defaultArrowOriginRotation = new Quaternion();
    private BaseArrow _currentArrow;
    private Vector3 _screenCenter;
    
    //other component
    private PlayerController _playerController;
    [SerializeField] private TPSCameraController _cameraController;
    [SerializeField] private ArrowIconUI _arrowIconUi;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pullArrow;
    [SerializeField] private AudioClip releaseArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultArrowOriginPosition = arrowOrigin.localPosition;
        _defaultArrowOriginRotation = arrowOrigin.localRotation;
        _playerController = GetComponent<PlayerController>();
        _screenCenter = new Vector3(Screen.width/2f,Screen.height/2f,0f);
        _currentForce = minShootForce;
        arrowCountUI.text = arrowCount.ToString();
        InitArrows();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAimingState();
        SwitchArrowType();
        UpdateCurrentArrow();
        ShootArrow();
        Drawing();
    }


    private void InitArrows()
    {
        foreach (var arrow in regularArrow)
        {
            arrow.Initialize(arrowOrigin);
        }
        foreach (var arrow in piercingArrow)
        {
            arrow.Initialize(arrowOrigin);
        }
        foreach (var arrow in stunningArrow)
        {
            arrow.Initialize(arrowOrigin);
        }
    }

    private void ShootArrow()
    {
        if (arrowCount <= 0) return;
        if (!inAimingState) return;
        if(_playerController.posture == BodyPosture.GUARD) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_currentArrow is null) return;
            _isDrawing = true;
            _currentArrow.ShakingRadious *= 2;
            audioSource.PlayOneShot(pullArrow);
        }


    }

    private void Drawing()
    {
        if (_isDrawing)
        {
            var dir = GetArrowDirection();
            //release
            if (Input.GetMouseButtonUp(0))
            {
                _currentArrow.Shoot(arrowLifetime, _currentForce * 200f * dir); //no need to specify shoot direction here
                _lastShootTime = Time.time;
                arrowCount--;
                arrowCountUI.text = arrowCount.ToString();
                _isDrawing = false;
                _currentForce = minShootForce;
                arrowOrigin.localPosition = _defaultArrowOriginPosition;
                arrowOrigin.localRotation = _defaultArrowOriginRotation;
                _currentArrow = null;
                audioSource.PlayOneShot(releaseArrow);
            }
            //still drawing
            else
            {
                //accumulate force
                if(_currentForce < maxShootForce){
                    _currentForce += Mathf.Lerp(0, maxShootForce-minShootForce, Time.deltaTime * drawingSpeed); 
                    arrowOrigin.localPosition -= new Vector3(0f, 0f, 
                        Mathf.Lerp(0, maxDrawingPosOffset,  Time.deltaTime * drawingSpeed)
                    );
                }
                //adjust aiming direction
                _currentArrow.transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }

    private void UpdateCurrentArrow()
    {
        if (!isCDOver()) return;
        if(!(_currentArrow is null)) return;

        BaseArrow[] arrowSet = null;
        switch (_chosenArrowType)
        {    
            case 0:
                arrowSet = regularArrow;
                break;
            case 1:
                arrowSet = piercingArrow;
                break;
            case 2:
                arrowSet = stunningArrow;
                break;
            default:
                arrowSet = regularArrow;
                break;
        }
        
        foreach (var arrow in arrowSet)
        {
            if (!arrow.isOnAir)
            {
                _currentArrow = arrow;
                DisplayArrow();
                return;
            }
        }

        _currentArrow = null;
    }


    private void ChangeAimingState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //turn on/off skill when not in GUARD state
            if (_playerController.posture == BodyPosture.COMBAT || _playerController.posture == BodyPosture.CROUCH)
            {
                UpdateCurrentArrow();
                if (_currentArrow is null) return;
                inAimingState = !inAimingState;
                DisplayArrow();
            }
        }
        //turn off when switch back to GUARD
        else if (Input.GetKeyDown(KeyCode.F) && inAimingState)
        {
            inAimingState = false;
            DisplayArrow();
        }
    }

    private bool isCDOver()
    {
        return Time.time > _lastShootTime + shootCD;
    }


    private Vector3 GetArrowDirection()
    {
        var cursorRay = _cameraController.getCamera().ScreenPointToRay(_screenCenter);
        return 
            Physics.Raycast(cursorRay, out var hitInfo, 100f, 1 << layerMask) ?
                (hitInfo.point - arrowOrigin.position).normalized : //direction from bullet to hit point
                cursorRay.direction; //aiming ray not hit, return aiming direction
    }

    private void DisplayArrow()
    {
        _currentArrow.isAiming = inAimingState;
        if(inAimingState)
            _currentArrow.gameObject.SetActive(true);
        else 
            _currentArrow.gameObject.SetActive(false);
    }

    private void SwitchArrowType()
    {
        if (!inAimingState || !Input.GetKeyDown(KeyCode.E) || _arrowIconUi._isSwitching) return;

        _chosenArrowType = (_chosenArrowType + 1) % 3;
        _arrowIconUi.SwitchArrow();

        //deselect current arrow
        if (!(_currentArrow is null))
        {
            _currentArrow.gameObject.SetActive(false);
            _currentArrow = null;
        }

        UpdateCurrentArrow();

        if (!(_currentArrow is null))
            _currentArrow.gameObject.SetActive(true);
    }

    public void ObtainArrows(int count)
    {
        arrowCount += count;
        arrowCountUI.text = arrowCount.ToString();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSkillManager : MonoBehaviour
{
    [SerializeField] private BaseArrow[] arrows;
    [SerializeField] private Rigidbody[] previewTrailRBs;
    [SerializeField] private TrailRenderer[] previewTrailRenders;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private float shootForce;
    [SerializeField] private float shootCD;
    [SerializeField] private float trailsInterval;

    
    //status
    public bool inAimingState;
    private BaseArrow _currentArrow;
    private float _lastShootTime;
    
    //other component
    private PlayerController _playerController;
    private PlayerShootManager _playerShootManager;
    

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShootManager = GetComponent<PlayerShootManager>();
        _currentArrow = arrows[0];
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAimingState();
        ShootArrow();
    }

    public void ShootArrow()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!inAimingState) return;
        if(_playerController.posture == BodyPosture.GUARD) return;
        if (!ShootCDEnded()) return;
        
        var dir = _playerShootManager.GetAimingDirection();
        _currentArrow.Initialize(dir,1f, shootOrigin.position, shootForce * 200f * dir);
        _lastShootTime = Time.time;

    }


    public void ChangeAimingState()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        
        inAimingState = !inAimingState;
        if (inAimingState)
            StartCoroutine(GeneratePreviewTrails());
        else
        {
            StopAllCoroutines();
            for (var i = 0; i < previewTrailRenders.Length; ++i)
            {
                ResetTrail(i);
            }
        }
    }

    private bool ShootCDEnded()
    {
        return Time.time > _lastShootTime + shootCD;
    }


    private void ShootTrail(int trailIndex)
    {
        var dir = _playerShootManager.GetAimingDirection();
        previewTrailRenders[trailIndex].gameObject.SetActive(true);
        previewTrailRenders[trailIndex].transform.position = shootOrigin.position;
        previewTrailRBs[trailIndex].AddForce(shootForce * 200f * dir);
    }

    private void ResetTrail(int trailIndex)
    {
        previewTrailRenders[trailIndex].gameObject.SetActive(false);
        previewTrailRenders[trailIndex].Clear();
        previewTrailRBs[trailIndex].velocity = Vector3.zero;
    }


    private IEnumerator GeneratePreviewTrails()
    {
        for(var i=0; i< previewTrailRenders.Length; ++i){
            StartCoroutine(GenerateSinglePreviewTrail(i));
            yield return new WaitForSeconds(trailsInterval/2f);
        }
    }

    private IEnumerator GenerateSinglePreviewTrail(int trailIndex)
    {
        while (true)
        {
            ShootTrail(trailIndex);
            yield return new WaitForSeconds(trailsInterval);
            ResetTrail(trailIndex);
        }
    }

}

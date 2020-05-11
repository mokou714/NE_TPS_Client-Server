using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraTransition : MonoBehaviour
{
    [Range(0f, 20f)] [SerializeField] private float speed;
    public Transform[] targetTransforms;
    private bool _started;
    private int _index = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = targetTransforms[_index].position;
        transform.eulerAngles = targetTransforms[_index].eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
    }

    public void NextTransition()
    {
        _started = true;
        _index = (_index + 1) % targetTransforms.Length;
    }


    private void Transition()
    {
        if (!_started) return;
        transform.position = Vector3.Lerp(transform.position, targetTransforms[_index].position, speed * Time.deltaTime);
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetTransforms[_index].eulerAngles, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetTransforms[_index].position) < 0.01f)
        {
            transform.position = targetTransforms[_index].position;
            _started = false;
        }
    }
}

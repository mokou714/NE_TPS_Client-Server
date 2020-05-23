using UnityEngine;
using UnityEngine.UI;

public class ArrowIconUI : MonoBehaviour
{
    public float speed;
    [SerializeField] private Transform[] arrowIconTransforms; //initailize with pre, current, next
    [SerializeField] private Image[] arrowIconImages;
    [SerializeField] private Transform[] PreCurrentNext; //0 pre, 1 current, 2 next
    public bool _isSwitching;

    private int pre;
    private int current;
    private int next;
    private Color _defaultIconColor;

    private Color _defaultTransparentIconColor;
    // Start is called before the first frame update
    void Start()
    {
        pre = 0;
        current = 1;
        next = 2;
        arrowIconTransforms[0].localPosition = PreCurrentNext[0].localPosition;
        arrowIconTransforms[1].localPosition = PreCurrentNext[1].localPosition;
        arrowIconTransforms[2].localPosition = PreCurrentNext[2].localPosition;
        _defaultTransparentIconColor = arrowIconImages[0].color;
        _defaultIconColor = new Color(_defaultTransparentIconColor.r, _defaultTransparentIconColor.g, _defaultTransparentIconColor.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Switching();
    }

    public void SwitchArrow()
    {
        _isSwitching = true;
       
    }

    private void Switching()
    {
        if (!_isSwitching) return;
        //pre -> next
        arrowIconTransforms[pre].localPosition += Vector3.Lerp(Vector3.zero , PreCurrentNext[2].localPosition - PreCurrentNext[0].localPosition, speed * Time.deltaTime);
        //current -> pref
        arrowIconTransforms[current].localPosition += Vector3.Lerp(Vector3.zero,  PreCurrentNext[0].localPosition - PreCurrentNext[1].localPosition, speed * Time.deltaTime);
        arrowIconImages[current].color -= new Color(0,0,0, Mathf.Lerp(0,1,speed * Time.deltaTime));
        //next -> current
        arrowIconTransforms[next].localPosition += Vector3.Lerp(Vector3.zero,  PreCurrentNext[1].localPosition - PreCurrentNext[2].localPosition, speed * Time.deltaTime);
        arrowIconImages[next].color += new Color(0,0,0, Mathf.Lerp(0,1,speed * Time.deltaTime));
        
        if (Vector3.Distance(arrowIconTransforms[pre].localPosition, PreCurrentNext[2].localPosition) < 5f)
        {
            arrowIconImages[current].color = _defaultTransparentIconColor;
            arrowIconImages[next].color = _defaultIconColor;
            arrowIconTransforms[pre].localPosition = PreCurrentNext[2].localPosition;
            arrowIconTransforms[current].localPosition = PreCurrentNext[0].localPosition;
            arrowIconTransforms[next].localPosition = PreCurrentNext[1].localPosition;
            pre = (pre + 1) % arrowIconTransforms.Length;
            current = (current + 1) % arrowIconTransforms.Length;
            next = (next + 1) % arrowIconTransforms.Length;
            _isSwitching = false;
        }

    }
    
    
}

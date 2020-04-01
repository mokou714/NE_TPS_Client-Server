using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DamageMessageManager : MonoBehaviour
{
    [SerializeField] private float msgMoveSpeed;
    [SerializeField] private float msgScaleSpeed;
    [SerializeField] private float msgFadeOutSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private DamageMessage[] messages;

    private Queue<int> _availableMsg;
    private int _messageSize;


    void Start()
    {
        _messageSize = messages.Length;
        _availableMsg = new Queue<int>(_messageSize);
        for(var i=0;i<_messageSize;++i)
        {
            messages[i].Init(lifetime,msgMoveSpeed,msgScaleSpeed,msgFadeOutSpeed,this);
            _availableMsg.Enqueue(i);
        }
    }

    public void ShowMessage(Vector3 characterPosition)
    {
        StartCoroutine(ShowDamageMessage(characterPosition));
    }

    public void EndShowing(int index)
    {
        _availableMsg.Enqueue(index);
    }


    IEnumerator ShowDamageMessage(Vector3 characterPosition)
    {
        yield return new WaitUntil(()=>_availableMsg.Count>0);
        int index = _availableMsg.Dequeue();
        messages[index].Show(index,characterPosition);
    }
    




}

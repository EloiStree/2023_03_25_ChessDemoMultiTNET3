using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerInteractionEventMono : MonoBehaviour
{
    public LayerMask m_authorizedTrigger;
    public UnityEvent m_OnTriggerEnter;
    public UnityEvent m_OnTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(IsInLayerMask(other.gameObject.layer, m_authorizedTrigger))
        m_OnTriggerEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, m_authorizedTrigger))
            m_OnTriggerExit.Invoke();

    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinkedStartPointChessMono : MonoBehaviour
{
    public Transform m_toMove;
    public Transform m_whereToMove;

    [ContextMenu("MoveToStartPoint")]
    public void MoveToStartPoint()
    {
        m_toMove.position = m_whereToMove.position;
        m_toMove.rotation = m_whereToMove.rotation;
    }
    private void Reset()
    {
        m_toMove = this.transform;
        GameObject[] o = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> l = o.Where(o => o != m_toMove.gameObject && o.name == m_toMove.gameObject.name).ToList();
        if (l.Count > 0)
            m_whereToMove = l[0].transform;
    }
}


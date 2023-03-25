using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNet;
using UnityEngine.Events;

/// <summary>
/// Very simple event manager script that sends out basic touch and mouse-based notifications using NGUI's syntax.
/// </summary>

public class SphereTouchHandlerTNET : MonoBehaviour
{
    public Transform m_sphereAnchorRef;

    public LayerMask eventReceiverMask = -1;

    GameObject m_currentSelectedObject;

    public bool m_isActive;
    public Vector3 m_startDragPosition;

    public GameObjectEvent m_onObjectSelected;
    public GameObjectEvent m_onObjectUnselected;

    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { };

    public void SetAsActive(bool isActive) {

        bool hasChanged = m_isActive != isActive;

        m_isActive = isActive;
        if (hasChanged) {
            if (m_isActive)
            {

                m_startDragPosition = m_sphereAnchorRef.position;
                SendPress(m_sphereAnchorRef.position);
            }
            if (!m_isActive)
            {

                SendRelease(m_sphereAnchorRef.position);
            }

        }
    }

    public float GetRadius() {
        return m_sphereAnchorRef.localScale.x;
    }


    /// <summary>
    /// Update the touch and mouse position and send out appropriate events.
    /// </summary>

    void Update()
    {
        if (m_currentSelectedObject != null &&  m_isActive) {

            SendDrag(m_sphereAnchorRef.position);
        }

      
    }

    /// <summary>
    /// Send out a press notification.
    /// </summary>

    void SendPress(Vector3 pos)
    {
        m_currentSelectedObject = OverlapSphereCast(pos);
        if (m_currentSelectedObject != null) m_currentSelectedObject.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Send out a release notification.
    /// </summary>

    void SendRelease(Vector3 pos)
    {

        if (m_currentSelectedObject != null)
        {
            GameObject go = OverlapSphereCast(pos);
            if (m_currentSelectedObject == go) m_currentSelectedObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            m_currentSelectedObject.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
            m_currentSelectedObject = null;
        }
    }

    /// <summary>
    /// Send out a drag notification.
    /// </summary>

    void SendDrag(Vector3 pos)
    {
        Vector3 delta = pos - m_startDragPosition ;

        if (delta.sqrMagnitude > 0.00001f)
        {
            OverlapSphereCast(pos);
            m_currentSelectedObject.SendMessage("OnDrag3D", pos, SendMessageOptions.DontRequireReceiver);
            m_startDragPosition = pos;
        }
    }

    /// <summary>
    /// Helper function that raycasts into the screen to determine what's underneath the specified position.
    /// </summary>

    GameObject OverlapSphereCast(Vector3 pos)
    {
        Collider[] touchedbySphereCast = Physics.OverlapSphere(pos, GetRadius(), eventReceiverMask );
        foreach (Collider coll in touchedbySphereCast)
        {
            ChessGrippableTag tag = coll.GetComponentInParent<ChessGrippableTag>(false);
            if (tag != null)
            {
                SetSelectedObject(coll.gameObject);
                return coll.gameObject;

            }
        }
        SetSelectedObject(null);
        return null;
    }

    GameObject m_selectedObject;
    public void SetSelectedObject(GameObject obj) {

        if (obj != m_selectedObject) {
            if (obj == null)
                m_onObjectUnselected.Invoke(m_selectedObject);
            else m_onObjectSelected.Invoke(obj);
        }
        m_selectedObject = obj;
    }
}

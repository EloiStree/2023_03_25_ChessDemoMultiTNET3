using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandPlayerRightTag : MonoBehaviour
{
    public static VRHandPlayerRightTag I;
    public static Vector3 m_position;
    public static Quaternion m_rotation;

    // Start is called before the first frame update
    void Start()
    {
        I = this;
    }
    private void Update()
    {
        m_position = transform.position;
        m_rotation = transform.rotation;
    }
}

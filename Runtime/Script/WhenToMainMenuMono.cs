using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenToMainMenuMono : MonoBehaviour
{
    public static WhenToMainMenuMono m_instance;
    // Start is called before the first frame update
    void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    

   
}

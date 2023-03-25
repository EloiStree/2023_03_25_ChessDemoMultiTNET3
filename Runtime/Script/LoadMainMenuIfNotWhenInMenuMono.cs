using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenuIfNotWhenInMenuMono : MonoBehaviour
{

    public string m_sceneIdToLoad = "";
    void Start()
    {
        if (WhenToMainMenuMono.m_instance == null) {
            if (m_sceneIdToLoad.Length == 0)
                SceneManager.LoadScene(0);
            else
                SceneManager.LoadScene(m_sceneIdToLoad);
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToRenderMono : MonoBehaviour
{

    public Renderer [] m_targets
        ;


    public void SetColor(Color color) {

        foreach (var item in m_targets)
        {
            item.material.color = color;
        }
    }

}

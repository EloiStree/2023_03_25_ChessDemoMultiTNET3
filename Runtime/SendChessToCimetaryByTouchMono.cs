using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendChessToCimetaryByTouchMono : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        LinkedCimeteryChessMono script = collision.gameObject.GetComponentInChildren<LinkedCimeteryChessMono>();
       if(script) script.MoveToCimetary();
    }

    private void OnTriggerEnter(Collider other)
    {
        LinkedCimeteryChessMono script = other.gameObject.GetComponentInChildren<LinkedCimeteryChessMono>();
        if (script) script.MoveToCimetary();
    }
}

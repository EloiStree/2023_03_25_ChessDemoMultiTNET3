using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CallbackContentToBooleanEventMono : MonoBehaviour
{

    public Eloi.PrimitiveUnityEvent_Bool m_onBooleanChanged;
    public void PushCallbackContentAsBool(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValueAsButton();
        m_onBooleanChanged.Invoke(isPressed);
    }
}

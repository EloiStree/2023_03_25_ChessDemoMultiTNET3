using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TNet;
using UnityEngine;

[RequireComponent(typeof(TNObject))]
public class TNHeadAndHandsMono : MonoBehaviour
{

    [System.NonSerialized]
    public TNObject tno;
    public Transform m_handLeftView;
    public Transform m_handRightView;
    public Transform m_headCenterView;
    public float m_timeBetweenPush = 0.1f;
    public bool m_displayMine = true;
    public bool m_displayMineHead = false;
    public bool m_displayMineHand = true;

    public Eloi.ClassicUnityEvent_Color m_onColorEvent;
    //[Header("Debug")]
    //public ChessRootReferenceMono m_chessRootRefInScene;
    //public VRHandPlayerLeftTag m_handLeftInScene;
    //public VRHandPlayerRightTag m_handRightInScene;
    //public VRHeadPlayerTag m_headCenterEyeInScene;
    public Color m_playerColor;

    public float m_lerpHead=10;
    public float m_lerpHands=20;



    public Vector3 m_headPosition;
    public Vector3 m_rightHandPosition;
    public Vector3 m_leftHandPosition;
    public Quaternion m_headRotation;
    public Quaternion m_rightHandRotation;
    public Quaternion m_leftHandRotation;
    
    public Vector3      m_wantedheadPosition;
    public Vector3      m_wantedrightHandPosition;
    public Vector3      m_wantedleftHandPosition;
    public Quaternion   m_wantedheadRotation;
    public Quaternion   m_wantedrightHandRotation;
    public Quaternion   m_wantedleftHandRotation;



    private void Awake()
    {
        tno = GetComponent<TNObject>();
    }
    private void Start()
    {
        PushColorToNetwork();
        InvokeRepeating("RefreshAndPush", 0.1f, m_timeBetweenPush);
    }

    private void PushColorToNetwork()
    {
        Color c = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        tno.Send("SetColor", Target.All,c);

    }
    [RFC]
    protected void SetColor(
        Color color)
    {
        m_onColorEvent.Invoke(color);
    }

    void RefreshAndPush()
    {
        

        if ( tno.isMine)
        {
            PushLocalPositionToRelocatedVariables();
            tno.Send("PushValueOnNetwork", Target.OthersSaved,
                 m_headPosition ,
            m_headRotation ,
            m_leftHandPosition  ,
            m_leftHandRotation  ,
            m_rightHandPosition ,
            m_rightHandRotation 
                );

        }

        if (!m_displayMine && tno.isMine) { 
            return;
        }
       

        LocalPositionToWorldPosition();
    }

    private void LocalPositionToWorldPosition()
    {
        Eloi.E_RelocationUtility.GetLocalToWorld_DirectionalPoint(
            m_headPosition,
            m_headRotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
            out  m_wantedheadPosition,
            out  m_wantedheadRotation
            );

        

        Eloi.E_RelocationUtility.GetLocalToWorld_DirectionalPoint(
           m_leftHandPosition,
           m_leftHandRotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
           out m_wantedleftHandPosition,
           out m_wantedleftHandRotation
           );
       


        Eloi.E_RelocationUtility.GetLocalToWorld_DirectionalPoint(
           m_rightHandPosition,
           m_rightHandRotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
           out m_wantedrightHandPosition,
           out m_wantedrightHandRotation
           );


    }
    private void Update()
    {
        if (tno.isMine)
        {

            m_handLeftView.gameObject.SetActive(m_displayMine && m_displayMineHand);
            m_handRightView.gameObject.SetActive(m_displayMine && m_displayMineHand);
            m_headCenterView.gameObject.SetActive(m_displayMine && m_displayMineHead);
        }
        else
        {

            m_handLeftView.gameObject.SetActive(true);
            m_handRightView.gameObject.SetActive(true);
            m_headCenterView.gameObject.SetActive(true);
        }

        m_headCenterView.position = Vector3.Lerp(m_headCenterView.position, m_wantedheadPosition, Time.deltaTime * m_lerpHead);
        m_headCenterView.rotation = Quaternion.Lerp(m_headCenterView.rotation, m_wantedheadRotation, Time.deltaTime * m_lerpHead);
        m_handLeftView.position = Vector3.Lerp(m_handLeftView.position, m_wantedleftHandPosition, Time.deltaTime * m_lerpHands);
        m_handLeftView.rotation = Quaternion.Lerp(m_handLeftView.rotation, m_wantedleftHandRotation, Time.deltaTime * m_lerpHands);
        m_handRightView.position = Vector3.Lerp(m_handRightView.position, m_wantedrightHandPosition, Time.deltaTime * m_lerpHands);
        m_handRightView.rotation = Quaternion.Lerp(m_handRightView.rotation, m_wantedrightHandRotation, Time.deltaTime * m_lerpHands);

    }
    private void PushLocalPositionToRelocatedVariables()
    {
        Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint
            (
            VRHeadPlayerTag.m_position,
            VRHeadPlayerTag.m_rotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
            out m_headPosition,
            out m_headRotation
            ); 
        Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint
            (
            VRHandPlayerLeftTag.m_position,
            VRHandPlayerLeftTag.m_rotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
            out m_leftHandPosition,
            out m_leftHandRotation
            ); 
        Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint
            (
            VRHandPlayerRightTag.m_position,
            VRHandPlayerRightTag.m_rotation,
            ChessRootReferenceMono.m_position,
            ChessRootReferenceMono.m_rotation,
            out m_rightHandPosition,
            out m_rightHandRotation
            );
    }
    [RFC]
    protected void PushValueOnNetwork(
        Vector3 pHead, Quaternion rHead,
        Vector3 pHandLeft, Quaternion rHandLeft,
        Vector3 pHandRight, Quaternion rHandRight)
    {
        m_headPosition = pHead;
        m_headRotation = rHead;
        m_leftHandPosition = pHandLeft;
        m_leftHandRotation = rHandLeft;
        m_rightHandPosition = pHandRight;
        m_rightHandRotation = rHandRight;
    }

}

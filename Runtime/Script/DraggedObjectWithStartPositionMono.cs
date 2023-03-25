using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TNet;

/// <summary>
/// This script shows how it's possible to associate objects with players.
/// You can see it used on draggable cubes in Example 3.
/// </summary>

public class DraggedObjectWithStartPositionMono : TNBehaviour
{
    Transform mTrans;
    Player mOwner;
    Vector3 mTarget;
    Vector3 mEditorStartPosition;
    Vector3 m_previousPosition;

    [ContextMenu("Override Target With Current")]
    public void OverrideTargetWithCurrent() {
        mTarget = transform.position;

    }

    protected override void Awake()
    {
        base.Awake();
        mTrans = transform;
        mTarget = mTrans.position;
        SaveCurrentPositionAsStartPoint();
    }


    [ContextMenu("Save Current Position As Start Point")]
    public void SaveCurrentPositionAsStartPoint()
    {
        mTarget = mTrans.position;
        mEditorStartPosition = mTarget;
    }
    [ContextMenu("Recover Current Position As Start Point")]
    public void RecoverCurrentPositionAsStartPoint()
    {
        mTarget = mEditorStartPosition ;
        mTrans.position = mEditorStartPosition;
        // Inform everyone else
        tno.Send(5, Target.OthersSaved, mEditorStartPosition);
    }

    [ContextMenu("Recover all position")]
    public void RecoverAllPositionsOfAllObjectInScene() {
        DraggedObjectWithStartPositionMono[] targetToReset = new DraggedObjectWithStartPositionMono[0];
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref targetToReset);
        foreach (var item in targetToReset)
        {
            if (item != null)
                item.RecoverCurrentPositionAsStartPoint();
        }
    }

    public float m_applyIfDistanceToBigInFrame = 0.5f;
    void Update()
    {
        Vector3 brutalVsCurrent = m_previousPosition - mTrans.position;
        if (brutalVsCurrent.magnitude > m_applyIfDistanceToBigInFrame)
        {
            tno.Send(5, Target.AllSaved,  mTrans.position);
        }
        mTrans.position = Vector3.Lerp(mTrans.position, mTarget, Time.deltaTime * 20f);
        m_previousPosition = mTrans.position;
    }

    /// <summary>
    /// Press / release event handler.
    /// </summary>

    void OnPress(bool isPressed)
    {
        // When pressed on an object, claim it for the player (unless it was already claimed).
        if (isPressed)
        {
            if (mOwner == null)
            {
                // Call the claim function directly in order to make it feel more responsive
                ClaimObject(TNManager.playerID, mTrans.position);

                // Inform everyone else
                tno.Send(4, Target.OthersSaved, TNManager.playerID, mTrans.position);
            }
        }
        else if (mOwner == TNManager.player)
        {
            // When the mouse or touch gets released, inform everyone that the player no longer has control.
            ClaimObject(0, mTrans.position);
            tno.Send(4, Target.OthersSaved, 0, mTrans.position);
        }
    }

    /// <summary>
    /// Remember the last player who claimed control of this object.
    /// </summary>

    [RFC(4)]
    void ClaimObject(int playerID, Vector3 pos)
    {
        mOwner = TNManager.GetPlayer(playerID);
        mTrans.position = pos;
        mTarget = pos;

        // Move the object to the Ignore Raycast layer while it's being dragged
        gameObject.layer = LayerMask.NameToLayer((mOwner != null) ? "Ignore Raycast" : "Default");
    }


    public void UnclaimObject()
    {
        tno.Send(6, Target.AllSaved);

    }
    [RFC(6)]
    void RFC_UnclaimObject()
    {
        mOwner = TNManager.GetPlayer(0);
        gameObject.layer = LayerMask.NameToLayer((mOwner != null) ? "Ignore Raycast" : "Default");
    }

    /// <summary>
    /// When the player is dragging the object around, update the target position for everyone.
    /// </summary>

    void OnDrag(Vector2 delta)
    {
        if (mOwner == TNManager.player)
        {
            mTarget = TouchHandler.worldPos;

            // Here we send the function via "SendQuickly", which is faster than regular "Send"
            // as it goes via UDP instead of TCP whenever possible. The downside of this approach
            // is that there is up to a 4% chance that the packet will get lost. However since
            // this update is sent so frequently, we simply don't care.
            tno.SendQuickly(3, Target.OthersSaved, mTarget);
        }
    }
    void OnDrag3D(Vector3 position)
    {
        if (mOwner == TNManager.player)
        {
            mTarget = position;

            // Here we send the function via "SendQuickly", which is faster than regular "Send"
            // as it goes via UDP instead of TCP whenever possible. The downside of this approach
            // is that there is up to a 4% chance that the packet will get lost. However since
            // this update is sent so frequently, we simply don't care.
            tno.SendQuickly(3, Target.OthersSaved, mTarget);
        }
    }

    /// <summary>
    /// Save the target position.
    /// </summary>

    [RFC(3)] void MoveObject(Vector3 pos) { mTarget = pos; }
    [RFC(5)] void TeleportObject(Vector3 pos) { mTarget = pos; mTrans.position = pos; }
}

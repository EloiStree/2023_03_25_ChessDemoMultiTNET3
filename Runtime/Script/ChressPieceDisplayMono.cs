using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChressPieceDisplayMono : MonoBehaviour
{

    public GameObject m_createdObject;

    public Transform m_whereToCreate;
    public GameObject m_bishopBlack;
    public GameObject m_kingBlack;
    public GameObject m_knightBlack;
    public GameObject m_pawnBlack;
    public GameObject m_queenBlack;
    public GameObject m_rookBlack;
    public GameObject m_bishopWhite;
    public GameObject m_kingWhite;
    public GameObject m_knightWhite;
    public GameObject m_pawnWhite;
    public GameObject m_queenWhite;
    public GameObject m_rookWhite;

   
    public ChessPieceType m_chessChoosed;
    public ChessColor m_colorChoosed;

    private void Start()
    {
        RefreshDisplayPiece();
    }

    [ContextMenu("Refresh Display Piece")]
    private void RefreshDisplayPiece()
    {
        SetWith(m_chessChoosed, m_colorChoosed);
    }

    public void SetWith(ChessPieceType chesstype, ChessColor color) {

       

        switch (chesstype)
        {
            case ChessPieceType.King: if (color == ChessColor.Black) Create(m_kingBlack); else Create(m_kingWhite); break;
            case ChessPieceType.Queen: if (color == ChessColor.Black) Create(m_queenBlack); else Create(m_queenWhite); break;
            case ChessPieceType.Bishop: if (color == ChessColor.Black) Create(m_bishopBlack); else Create(m_bishopWhite); break;
            case ChessPieceType.Knight: if (color == ChessColor.Black) Create(m_knightBlack); else Create(m_knightWhite); break;
            case ChessPieceType.Rook: if (color == ChessColor.Black) Create(m_rookBlack); else Create(m_rookWhite); break;
            case ChessPieceType.Pawn: if (color == ChessColor.Black) Create(m_pawnBlack); else Create(m_pawnWhite); break;
        }
    }

    private void Create(GameObject toCreatePrefab)
    {
        if (m_createdObject != null)
        {   

            Destroy(m_createdObject);
        }
        m_createdObject = GameObject.Instantiate(toCreatePrefab);
        m_createdObject.transform.parent = m_whereToCreate;
    }
}

public enum ChessPieceType { King, Queen, Bishop,Knight,Rook, Pawn}
public enum ChessColor { Black, White}

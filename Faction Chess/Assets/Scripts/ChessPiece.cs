using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public Sprite white_king, white_queen, white_bishop, white_knight, white_rook, white_pawn;
    public Sprite black_king, black_queen, black_bishop, black_knight, black_rook, black_pawn;
    string pieceType;
    string team;
    int boardX;
    int boardY;
    float boardOffsetX = -3.2f;

    public string GetPieceType()
    {
        return pieceType;
    }
    public void SetPieceType(string t)
    {
        pieceType = t;
    }
    public int GetBoardX()
    {
        return boardX;
    }
    public int GetBoardY()
    {
        return boardY;
    }
    public void SetBoardX(int x)
    {
        boardX = x;
    }
    public void SetBoardY(int y)
    {
        boardY = y;
    }
    public string GetTeam()
    {
        return team;
    }
    public bool IsOfTeam(string t)
    {
        if (t == team)
            return true;
        return false;
    }
    public void SetTeam(string t)
    {
        team = t;
    }
    public void UpdateSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (team == "white")
        {
            switch (pieceType)
            {
                case "king":
                    sr.sprite = white_king;
                    break;
                case "queen":
                    sr.sprite = white_queen;
                    break;
                case "bishop":
                    sr.sprite = white_bishop;
                    break;
                case "knight":
                    sr.sprite = white_knight;
                    break;
                case "rook":
                    sr.sprite = white_rook;
                    break;
                case "pawn":
                    sr.sprite = white_pawn;
                    break;
            }
        }
        else if (team == "black")
        {
            switch (pieceType)
            {
                case "king":
                    sr.sprite = black_king;
                    break;
                case "queen":
                    sr.sprite = black_queen;
                    break;
                case "bishop":
                    sr.sprite = black_bishop;
                    break;
                case "knight":
                    sr.sprite = black_knight;
                    break;
                case "rook":
                    sr.sprite = black_rook;
                    break;
                case "pawn":
                    sr.sprite = black_pawn;
                    break;
            }
        }
    }
    public void MoveToPosition(int x, int y)
    {
        boardX = x;
        boardY = y;
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        float convertedX = boardX - 3.5f + boardOffsetX;
        convertedX *= 1.22f;
        float convertedY = boardY - 3.5f;
        convertedY *= 1.22f;

        this.transform.position = new Vector3(convertedX, convertedY, -1);
    }
}

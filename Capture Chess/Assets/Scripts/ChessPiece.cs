using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    //public Sprite white_king, white_queen, white_bishop, white_knight, white_rook, white_pawn, white_spawner;
    //public Sprite black_king, black_queen, black_bishop, black_knight, black_rook, black_pawn, black_spawner;
    public SpriteLibrary spriteLibrary;
    string pieceType;
    string team;
    int boardX;
    int boardY;
    float boardOffsetX = -3.9f;

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
                    sr.sprite = spriteLibrary.white_king;
                    break;
                case "queen":
                    sr.sprite = spriteLibrary.white_queen;
                    break;
                case "bishop":
                    sr.sprite = spriteLibrary.white_bishop;
                    break;
                case "knight":
                    sr.sprite = spriteLibrary.white_knight;
                    break;
                case "rook":
                    sr.sprite = spriteLibrary.white_rook;
                    break;
                case "pawn":
                    sr.sprite = spriteLibrary.white_pawn;
                    break;
                case "spawner":
                    sr.sprite = spriteLibrary.white_spawner;
                    break;
            }
        }
        else if (team == "black")
        {
            switch (pieceType)
            {
                case "king":
                    sr.sprite = spriteLibrary.black_king;
                    break;
                case "queen":
                    sr.sprite = spriteLibrary.black_queen;
                    break;
                case "bishop":
                    sr.sprite = spriteLibrary.black_bishop;
                    break;
                case "knight":
                    sr.sprite = spriteLibrary.black_knight;
                    break;
                case "rook":
                    sr.sprite = spriteLibrary.black_rook;
                    break;
                case "pawn":
                    sr.sprite = spriteLibrary.black_pawn;
                    break;
                case "spawner":
                    sr.sprite = spriteLibrary.black_spawner;
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
        float convertedX = boardX - 4.25f + boardOffsetX;
        convertedX *= 1.03f;
        float convertedY = boardY - 4.45f;
        convertedY *= 1.03f;

        this.transform.position = new Vector3(convertedX, convertedY, -1);
    }
}

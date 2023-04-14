using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptureInventory : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI whiteInvPawnText, whiteInvKnightText, whiteInvBishopText, whiteInvRookText, whiteInvQueenText, whiteScoreText;
    public TextMeshProUGUI blackInvPawnText, blackInvKnightText, blackInvBishopText, blackInvRookText, blackInvQueenText, blackScoreText;
    int whiteInvPawn, whiteInvKnight, whiteInvBishop, whiteInvRook, whiteInvQueen;
    int blackInvPawn, blackInvKnight, blackInvBishop, blackInvRook, blackInvQueen;
    public int whiteScore = 0, blackScore = 0, whiteScorePenalty = 0, blackScorePenalty = 0;

    public void ChangeInventory(string invOfTeam, string pieceType, int changeValue)
    {
        if (invOfTeam == "white")
        {
            switch (pieceType)
            {
                case "pawn":
                    whiteInvPawn += changeValue;
                    break;
                case "knight":
                    whiteInvKnight += changeValue;
                    break;
                case "bishop":
                    whiteInvBishop += changeValue;
                    break;
                case "rook":
                    whiteInvRook += changeValue;
                    break;
                case "queen":
                    whiteInvQueen += changeValue;
                    break;
            }
        }
        else if (invOfTeam == "black")
        {
            switch (pieceType)
            {
                case "pawn":
                    blackInvPawn += changeValue;
                    break;
                case "knight":
                    blackInvKnight += changeValue;
                    break;
                case "bishop":
                    blackInvBishop += changeValue;
                    break;
                case "rook":
                    blackInvRook += changeValue;
                    break;
                case "queen":
                    blackInvQueen += changeValue;
                    break;
            }
        }
        UpdateInvCountText();
    }

    public bool HasInInventory(string invOfTeam, string pieceType)
    {
        int numInInventory = 0;
        if (invOfTeam == "white")
        {
            switch (pieceType)
            {
                case "pawn":
                    numInInventory = whiteInvPawn;
                    break;
                case "knight":
                    numInInventory = whiteInvKnight;
                    break;
                case "bishop":
                    numInInventory = whiteInvBishop;
                    break;
                case "rook":
                    numInInventory = whiteInvRook;
                    break;
                case "queen":
                    numInInventory = whiteInvQueen;
                    break;
            }
        }
        else if (invOfTeam == "black")
        {
            switch (pieceType)
            {
                case "pawn":
                    numInInventory = blackInvPawn;
                    break;
                case "knight":
                    numInInventory = blackInvKnight;
                    break;
                case "bishop":
                    numInInventory = blackInvBishop;
                    break;
                case "rook":
                    numInInventory = blackInvRook;
                    break;
                case "queen":
                    numInInventory = blackInvQueen;
                    break;
            }
        }
        if (numInInventory > 0)
            return true;
        else
            return false;
    }

    public void UpdateInvCountText()
    {
        whiteInvPawnText.text = whiteInvPawn.ToString();
        whiteInvKnightText.text = whiteInvKnight.ToString();
        whiteInvBishopText.text = whiteInvBishop.ToString();
        whiteInvRookText.text = whiteInvRook.ToString();
        whiteInvQueenText.text = whiteInvQueen.ToString();

        blackInvPawnText.text = blackInvPawn.ToString();
        blackInvKnightText.text = blackInvKnight.ToString();
        blackInvBishopText.text = blackInvBishop.ToString();
        blackInvRookText.text = blackInvRook.ToString();
        blackInvQueenText.text = blackInvQueen.ToString();

        whiteScore = whiteInvPawn + whiteInvKnight * 3 + whiteInvBishop * 3 + whiteInvRook * 5 + whiteInvQueen * 9 - whiteScorePenalty;
        whiteScoreText.text = "Score: " + whiteScore.ToString();

        blackScore = blackInvPawn + blackInvKnight * 3 + blackInvBishop * 3 + blackInvRook * 5 + blackInvQueen * 9 - blackScorePenalty;
        blackScoreText.text = "Score: " + blackScore.ToString();
    }

    public void ShowPenalty(string team, string pieceType, int penalty)
    {
        int basePieceValue = 0;
        switch (pieceType)
        {
            case "pawn":
                basePieceValue = 1;
                break;
            case "knight":
                basePieceValue = 3;
                break;
            case "bishop":
                basePieceValue = 3;
                break;
            case "rook":
                basePieceValue = 5;
                break;
            case "queen":
                basePieceValue = 9;
                break;
        }
        int fullPenalty = basePieceValue + penalty;
        if (team == "white")
            whiteScoreText.text = "Score: " + whiteScore.ToString() + "(-" + fullPenalty.ToString() + ")";
        else
            blackScoreText.text = "Score: " + blackScore.ToString() + "(-" + fullPenalty.ToString() + ")";
    }

    public void Clear()
    {
        whiteInvPawn = 0;
        whiteInvKnight = 0;
        whiteInvBishop = 0;
        whiteInvRook = 0;
        whiteInvQueen = 0;

        blackInvPawn = 0;
        blackInvKnight = 0;
        blackInvBishop = 0;
        blackInvRook = 0;
        blackInvQueen = 0;

        UpdateInvCountText();
    }
}

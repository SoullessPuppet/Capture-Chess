using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptureInventory : MonoBehaviour
{
    public int whiteScore = 0, blackScore = 0, whiteScorePenalty = 0, blackScorePenalty = 0;
    public TextMeshProUGUI whiteScoreText, blackScoreText;

    public void ChangeScore(string team, int amount)
    {
        if (team == "white")
        {
            whiteScore += amount;
        }
        else
        {
            blackScore += amount;
        }
        UpdateUI();
    }

    public void AddScoreFromCapturing(string team, string pieceType)
    {
        int score = 0;
        switch (pieceType)
        {
            case "pawn":
                score = 1;
                break;
            case "knight":
                score = 3;
                break;
            case "bishop":
                score = 4;
                break;
            case "rook":
                score = 5;
                break;
            case "queen":
                score = 9;
                break;
        }
        if (team == "white")
        {
            whiteScore += score;
        }
        else
        {
            blackScore += score;
        }
        UpdateUI();
    }

    public void ShowPenalty(string team, string pieceType)
    {
        int cost = GetCost(pieceType);
        if (team == "white")
            whiteScoreText.text = "Score: " + whiteScore.ToString() + "(-" + cost.ToString() + ")";
        else
            blackScoreText.text = "Score: " + blackScore.ToString() + "(-" + cost.ToString() + ")";
    }

    public int GetCost(string pieceType)
    {
        switch (pieceType)
        {
            case "pawn":
                return 2;
            case "knight":
                return 2;
            case "bishop":
                return 4;
            case "rook":
                return 6;
            case "queen":
                return 10;
            default:
                return 0;
        }
    }

    public bool CanAfford(string team, string pieceType)
    {
        if (team == "white")
        {
            if (whiteScore - GetCost(pieceType) >= 0)
                return true;
            else
                return false;
        }
        else
        {
            if (blackScore - GetCost(pieceType) >= 0)
                return true;
            else
                return false;
        }
    }

    public void Reset()
    {
        whiteScore = 0;
        blackScore = 0;
        UpdateUI();
    }

    public void UpdateUI()
    {
        whiteScoreText.text = "Score: " + whiteScore.ToString();
        blackScoreText.text = "Score: " + blackScore.ToString();
    }
}

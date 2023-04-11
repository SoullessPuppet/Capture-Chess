using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureInventory : MonoBehaviour
{
    public List<ChessPiece> whiteInventory = new();
    public List<ChessPiece> blackInventory = new();

    public void AddToInventory(ChessPiece capturedPiece, string invOfTeam)
    {
        if (invOfTeam == "white")
            whiteInventory.Add(capturedPiece);
        else if (invOfTeam == "black")
            blackInventory.Add(capturedPiece);
    }

    public void RemoveFromInventory(ChessPiece capturedPiece, string invOfTeam)
    {
        if(invOfTeam == "white")
            whiteInventory.Remove(capturedPiece);
        else if (invOfTeam == "black")
            blackInventory.Remove(capturedPiece);
    }
}

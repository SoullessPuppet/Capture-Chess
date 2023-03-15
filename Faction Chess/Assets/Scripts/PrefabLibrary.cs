using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLibrary : MonoBehaviour
{
    GameObject movePlate;
    GameObject king;
    GameObject queen;
    GameObject pawn;
    GameObject bishop;
    GameObject knight;
    GameObject rook;

    public GameObject GetMovePlate()
    {
        return movePlate;
    }
    public GameObject GetKing()
    {
        return king;
    }
    public GameObject GetQueen()
    {
        return queen;
    }
    public GameObject GetPawn()
    {
        return pawn;
    }
    public GameObject GetBishop()
    {
        return bishop;
    }
    public GameObject GetKnight()
    {
        return knight;
    }
    public GameObject GetRook()
    {
        return rook;
    }
}

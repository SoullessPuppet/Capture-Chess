using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    int boardX;
    int boardY;
    bool isAttack = false;
    bool isSpawn = false;
    float boardOffsetX = -3.9f;

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
    public void ConvertToAttack()
    {
        isAttack = true;
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
    }
    public void ConvertToSpawn()
    {
        isSpawn = true;
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
    }

    public bool IsAttack()
    {
        return isAttack;
    }
    public bool IsSpawn()
    {
        return isSpawn;
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

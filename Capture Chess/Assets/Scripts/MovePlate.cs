using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    int boardX;
    int boardY;
    bool isAttack = false;
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
    public bool IsAttack()
    {
        return isAttack;
    }
    public void MoveToPosition(int x, int y)
    {
        boardX = x;
        boardY = y;
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        float convertedX = boardX - 4.5f + boardOffsetX;
        //convertedX *= 1.22f;
        float convertedY = boardY - 4.5f;
        //convertedY *= 1.22f;

        this.transform.position = new Vector3(convertedX, convertedY, -1);
    }
}

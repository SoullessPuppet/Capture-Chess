using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI currentPlayerText;
    public GameObject gameButtonsPanel;
    public PromotionMenu promotionMenu;
    public BoardManager boardManager;
    public CaptureInventory captureInventory;
    public GameObject selectedObject;

    public string currentPlayer = "white";
    int turn = 0;
    int lastTurn = 30;
    bool gameOver = false;

    private void Start()
    {
        RestartGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            Vector2 ray = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject hitObject = null;

            RaycastHit2D hitPlate = Physics2D.Raycast(ray, Vector2.zero, 0f, LayerMask.GetMask("PlateHitbox"));
            if (hitPlate)
            {
                hitObject = hitPlate.collider.gameObject;
                MovePlate hitPlateScript = hitObject.GetComponent<MovePlate>();
                if (hitPlateScript.IsSpawn())
                    boardManager.SpawnFromInventory(hitPlateScript);
                else
                    boardManager.MoveToPlate(hitPlateScript);
                return;
            }

            RaycastHit2D hitPiece = Physics2D.Raycast(ray, Vector2.zero, 0f, LayerMask.GetMask("PieceHitbox"));
            if (hitPiece)
            {
                hitObject = hitPiece.collider.gameObject;
                if (hitObject == selectedObject)
                    return;
                if (hitObject.CompareTag("ChessPiece"))
                {
                    boardManager.DestroyMovePlates();
                    selectedObject = hitObject;
                    boardManager.SpawnMovePlates();
                    return;
                }
            }
            ClearSelection();
            return;
        }
    }

    public void RestartGame()
    {
        gameOver = false;
        currentPlayer = "white";
        currentPlayerText.text = "White's turn (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        boardManager.ResetBoard();
    }

    public void SwitchPlayer()
    {
        ClearSelection();
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            currentPlayerText.text = "Black's turn (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        else if (currentPlayer == "black")
        {
            currentPlayer = "white";
            currentPlayerText.text = "White's turn (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }

        if (turn == lastTurn)
            GameOverPointCount();
        else
            turn += 1;
    }

    public void GameOver()
    {
        gameOver = true;
        if (currentPlayer == "white")
        {
            currentPlayerText.text = "White WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        else if (currentPlayer == "black")
        {
            currentPlayerText.text = "Black WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
    }

    public void GameOverPointCount()
    {
        gameOver = true;
        int whiteScoreAdvantage = captureInventory.whiteScore - captureInventory.blackScore;
        if (whiteScoreAdvantage > 0)
            currentPlayerText.text = "White WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        else if (whiteScoreAdvantage < 0)
            currentPlayerText.text = "Black WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        else
        {
            currentPlayerText.text = "TIE! (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
    }

    public void ClearSelection()
    {
        selectedObject = null;
        boardManager.DestroyMovePlates();
    }
}
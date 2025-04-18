using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject boardCanvas;
    public GameObject gameButtonsPanel;
    public GameObject mainMenuPanel;
    public GameObject gameLengthPanel;
    public PromotionMenu promotionMenu;
    public TextMeshProUGUI currentPlayerText;
    public Slider gameLengthSlider;

    public BoardManager boardManager;
    public CaptureInventory captureInventory;
    public GameObject selectedObject;

    public string currentPlayer = "white";
    int turn = 1;
    int lastTurn = 30;
    bool gameOver = false;

    private void Start()
    {
        ShowMainMenu();
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

    public void NewGame()
    {
        gameLengthPanel.SetActive(false);
        boardCanvas.SetActive(true);
        if (gameLengthSlider.value * 2 > 150)
            lastTurn = 999;
        else
            lastTurn = (int)gameLengthSlider.value*2;

        gameOver = false;
        turn = 1;
        currentPlayer = "white";
        currentPlayerText.text = "White's turn (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        boardManager.ResetBoard();
        gameLengthPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        boardCanvas.SetActive(false);
    }

    public void ShowGameLengthPanel()
    {
        mainMenuPanel.SetActive(false);
        gameLengthPanel.SetActive(true);
    }

    public void SwitchPlayer()
    {
        ClearSelection();
        if (turn == lastTurn)
        {
            GameOverScoreCount();
            return;
        }
        else
            turn += 1;

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
    }

    public void GameOver()
    {
        print("normal game over");
        if (currentPlayer == "white")
        {
            currentPlayerText.text = "White WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        else if (currentPlayer == "black")
        {
            currentPlayerText.text = "Black WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        gameOver = true;
    }

    public void GameOverScoreCount()
    {
        int whiteScoreAdvantage = captureInventory.whiteScore - captureInventory.blackScore;
        if (whiteScoreAdvantage > 0)
        {
            currentPlayerText.text = "White WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        else if (whiteScoreAdvantage < 0)
            currentPlayerText.text = "Black WON!(Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        else
        {
            currentPlayerText.text = "TIE! (Turn " + turn.ToString() + "/" + lastTurn.ToString() + ")";
        }
        gameOver = true;
    }

    public void ClearSelection()
    {
        selectedObject = null;
        boardManager.DestroyMovePlates();
    }
}
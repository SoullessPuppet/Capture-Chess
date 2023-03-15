using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject chessPiece;
    public GameObject movePlate;

    public TextMeshProUGUI currentPlayerText;
    public GameObject gameButtonsPanel;
    public GameObject skillsPanel;
    public PromotionMenu promotionMenu;

    GameObject selectedObject;

    GameObject[,] boardMatrix = new GameObject[8,8];
    List<GameObject> whitePieces = new();
    List<GameObject> blackPieces = new();
    string currentPlayer = "white";
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
                MoveToPlate(hitObject.GetComponent<MovePlate>());
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
                    DestroyMovePlates();
                    selectedObject = hitObject;
                    SpawnMovePlates();
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
        skillsPanel.SetActive(true);
        currentPlayerText.text = "White Player's Turn";
        ResetBoard();
    }

    void SwitchPlayer()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            currentPlayerText.text = "Black Player's Turn";
        }
        else if (currentPlayer == "black")
        {
            currentPlayer = "white";
            currentPlayerText.text = "White Player's Turn";
        }
    }

    void GameOver()
    {
        gameOver = true;
        skillsPanel.SetActive(false);
        if (currentPlayer == "white")
        {
            currentPlayerText.text = "White won!";
        }
        else if (currentPlayer == "black")
        {
            currentPlayerText.text = "Black won!";
        }
    }

    void ClearSelection()
    {
        selectedObject = null;
        DestroyMovePlates();
    }

    void ResetBoard()
    {
        Array.Clear(boardMatrix, 0, boardMatrix.Length);
        whitePieces.Clear();
        blackPieces.Clear();

        ChessPiece[] allChessPieces = FindObjectsOfType<ChessPiece>();
        if (allChessPieces.Length > 0)
            for (int i = 0; i < allChessPieces.Length; i++)
                Destroy(allChessPieces[i].gameObject);

        MovePlate[] allMovePlates = FindObjectsOfType<MovePlate>();
        if (allMovePlates.Length > 0)
            for (int i = 0; i < allMovePlates.Length; i++)
                Destroy(allMovePlates[i].gameObject);

        //White backline
        SpawnPiece("white", "rook", 0, 0);
        SpawnPiece("white", "knight", 1, 0);
        SpawnPiece("white", "bishop", 2, 0);
        SpawnPiece("white", "queen", 3, 0);
        SpawnPiece("white", "king", 4, 0);
        SpawnPiece("white", "bishop", 5, 0);
        SpawnPiece("white", "knight", 6, 0);
        SpawnPiece("white", "rook", 7, 0);
        //White frontline
        SpawnPiece("white", "pawn", 0, 1);
        SpawnPiece("white", "pawn", 1, 1);
        SpawnPiece("white", "pawn", 2, 1);
        SpawnPiece("white", "pawn", 3, 1);
        SpawnPiece("white", "pawn", 4, 1);
        SpawnPiece("white", "pawn", 5, 1);
        SpawnPiece("white", "pawn", 6, 1);
        SpawnPiece("white", "pawn", 7, 1);
        //Black frontline
        SpawnPiece("black", "pawn", 0, 6);
        SpawnPiece("black", "pawn", 1, 6);
        SpawnPiece("black", "pawn", 2, 6);
        SpawnPiece("black", "pawn", 3, 6);
        SpawnPiece("black", "pawn", 4, 6);
        SpawnPiece("black", "pawn", 5, 6);
        SpawnPiece("black", "pawn", 6, 6);
        SpawnPiece("black", "pawn", 7, 6);
        //Black backline
        SpawnPiece("black", "rook", 0, 7);
        SpawnPiece("black", "knight", 1, 7);
        SpawnPiece("black", "bishop", 2, 7);
        SpawnPiece("black", "queen", 3, 7);
        SpawnPiece("black", "king", 4, 7);
        SpawnPiece("black", "bishop", 5, 7);
        SpawnPiece("black", "knight", 6, 7);
        SpawnPiece("black", "rook", 7, 7);
    }

    void SpawnPiece(string team, string pieceType, int x, int y)
    {
        GameObject newPiece = Instantiate(chessPiece, Vector3.zero, Quaternion.identity);
        ChessPiece newPieceScript = newPiece.GetComponent<ChessPiece>();
        newPieceScript.SetTeam(team);
        newPieceScript.SetPieceType(pieceType);
        newPieceScript.MoveToPosition(x, y);
        newPieceScript.UpdateSprite();

        boardMatrix[x, y] = newPiece;

        if (team == "white")
            whitePieces.Add(boardMatrix[x, y]);
        else if (team == "black")
            blackPieces.Add(boardMatrix[x, y]);
    }

    void SpawnMovePlates()
    {
        if (selectedObject != null && selectedObject.GetComponent<ChessPiece>().GetTeam() != currentPlayer)
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        switch (selectedPiece.GetPieceType())
        {
            case "king":
                KingMovePlates();
                break;
            case "queen":
                LineMovePlates(-1, 0); //left
                LineMovePlates(1, 0); //right
                LineMovePlates(0, -1); //down
                LineMovePlates(0, 1); //up
                LineMovePlates(-1, -1); //left-down
                LineMovePlates(1, -1); //right-down
                LineMovePlates(-1, 1); //left-up
                LineMovePlates(1, 1); //right-up
                break;
            case "bishop":
                LineMovePlates(-1, -1); //left-down
                LineMovePlates(1, -1); //right-down
                LineMovePlates(-1, 1); //left-up
                LineMovePlates(1, 1); //right-up
                break;
            case "rook":
                LineMovePlates(-1, 0); //left
                LineMovePlates(1, 0); //right
                LineMovePlates(0, 1); //up
                LineMovePlates(0, -1); //down
                break;
            case "knight":
                KnightMovePlates();
                break;
            case "pawn":
                PawnMovePlates();
                break;
        }
    }
    void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject go in movePlates)
            Destroy(go);
    }

    void MoveToPlate(MovePlate mp)
    {
        ChessPiece movingPiece = selectedObject.GetComponent<ChessPiece>();
        int movePlateX = mp.GetBoardX();
        int movePlateY = mp.GetBoardY();
        string pieceTypeAtDestination = null;

        if (boardMatrix[movePlateX, movePlateY] != null)
            pieceTypeAtDestination = boardMatrix[movePlateX, movePlateY].GetComponent<ChessPiece>().GetPieceType();

        if (mp.IsAttack())
        {
            boardMatrix[movePlateX, movePlateY].SetActive(false);
            boardMatrix[movePlateX, movePlateY] = null;
        }

        boardMatrix[movingPiece.GetBoardX(), movingPiece.GetBoardY()] = null;
        boardMatrix[movePlateX, movePlateY] = movingPiece.gameObject;
        movingPiece.MoveToPosition(movePlateX, movePlateY);
        ClearSelection();

        if (pieceTypeAtDestination == "king")
        {
            GameOver();
            return;
        }

        if (movingPiece.GetPieceType() == "pawn")
        {
            if (movingPiece.GetBoardY() == 7 && movingPiece.GetTeam() == "white")
            {
                promotionMenu.PromotePawn(movingPiece);
            }
            else if (movingPiece.GetBoardY() == 0 && movingPiece.GetTeam() == "black")
            {
                promotionMenu.PromotePawn(movingPiece);
            }
        }

        SwitchPlayer();
    }

    bool TileIsOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x+1 > boardMatrix.GetLength(0) || y+1 > boardMatrix.GetLength(1))
            return false;
        else
            return true;
    }

    GameObject GetAtPosition(int x, int y)
    {
        return boardMatrix[x, y];
    }
    void SpawnMovePlateAt(int x, int y)
    {
        if (!TileIsOnBoard(x, y))
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        ChessPiece pieceAtTargetTile = null;
        if (GetAtPosition(x, y))
            pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();

        if (pieceAtTargetTile != null && pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam())
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.MoveToPosition(x, y);
        newPlateScript.UpdatePosition();

        if (pieceAtTargetTile != null)
            newPlateScript.ConvertToAttack();
    }

    void SpawnMoveOnlyPlateAt(int x, int y)
    {
        if (!TileIsOnBoard(x, y))
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        ChessPiece pieceAtTargetTile = null;
        if (GetAtPosition(x, y))
            pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();

        if (pieceAtTargetTile != null)
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.SetBoardX(x);
        newPlateScript.SetBoardY(y);
        newPlateScript.UpdatePosition();
    }

    void SpawnAttackOnlyPlateAt(int x, int y)
    {
        if (!TileIsOnBoard(x, y))
            return;

        if (GetAtPosition(x, y) == null)
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        ChessPiece pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();

        if (pieceAtTargetTile != null && pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam())
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.SetBoardX(x);
        newPlateScript.SetBoardY(y);
        newPlateScript.UpdatePosition();
        newPlateScript.ConvertToAttack();
    }

    void LineMovePlates(int xIncrement, int yIncrement)
    {
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX() + xIncrement;
        int y = selectedPiece.GetBoardY() + yIncrement;

        while (TileIsOnBoard(x, y))
        {
            if (GetAtPosition(x, y) != null)
            {
                ChessPiece pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();
                if (pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam())
                    break;
                if (pieceAtTargetTile.GetTeam() != selectedPiece.GetTeam())
                {
                    SpawnMovePlateAt(x, y);
                    break;
                }
            }
            SpawnMovePlateAt(x, y);
            x += xIncrement;
            y += yIncrement;
        }
    }

    void KingMovePlates()
    {
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX();
        int y = selectedPiece.GetBoardY();

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                SpawnMovePlateAt(i, j);
            }
        }
    }

    void KnightMovePlates()
    {
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX();
        int y = selectedPiece.GetBoardY();

        SpawnMovePlateAt(x-2, y-1);
        SpawnMovePlateAt(x-2, y+1);
        SpawnMovePlateAt(x-1, y-2);
        SpawnMovePlateAt(x-1, y+2);
        SpawnMovePlateAt(x+1, y-2);
        SpawnMovePlateAt(x+1, y+2);
        SpawnMovePlateAt(x+2, y-1);
        SpawnMovePlateAt(x+2, y+1);
    }

    void PawnMovePlates()
    {
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX();
        int y = selectedPiece.GetBoardY();
        int forwardOneTile = 0;

        if (selectedPiece.GetTeam() == "white")
            forwardOneTile = y + 1;
        else if (selectedPiece.GetTeam() == "black")
            forwardOneTile = y - 1;

        SpawnMoveOnlyPlateAt(x, forwardOneTile);
        SpawnAttackOnlyPlateAt(x - 1, forwardOneTile);
        SpawnAttackOnlyPlateAt(x + 1, forwardOneTile);

        if (selectedPiece.GetTeam() == "white" && y == 1)
            SpawnMoveOnlyPlateAt(x, forwardOneTile + 1);
        if (selectedPiece.GetTeam() == "black" && y == 6)
            SpawnMoveOnlyPlateAt(x, forwardOneTile - 1);
    }
}

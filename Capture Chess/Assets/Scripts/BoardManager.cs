using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    public GameManager gameManager;
    public CaptureInventory captureInventory;
    public GameObject chessPiece;
    public GameObject movePlate;
    GameObject whiteSpawner, blackSpawner;

    GameObject[,] boardMatrix = new GameObject[10, 10];
    List<GameObject> whitePieces = new();
    List<GameObject> blackPieces = new();

    string spawningTeam = null;
    string spawningType = null;
    int spawningPenalty = 0;

    const int smallSpawningPenalty = 1;
    const int largeSpawningPenalty = 3;

    public void ResetBoard()
    {
        Array.Clear(boardMatrix, 0, boardMatrix.Length);
        whitePieces.Clear();
        blackPieces.Clear();
        captureInventory.Clear();

        ChessPiece[] allChessPieces = FindObjectsOfType<ChessPiece>();
        if (allChessPieces.Length > 0)
            for (int i = 0; i < allChessPieces.Length; i++)
                Destroy(allChessPieces[i].gameObject);

        MovePlate[] allMovePlates = FindObjectsOfType<MovePlate>();
        if (allMovePlates.Length > 0)
            for (int i = 0; i < allMovePlates.Length; i++)
                Destroy(allMovePlates[i].gameObject);

        //White backline
        SpawnPiece("white", "rook", 1, 0);
        SpawnPiece("white", "knight", 2, 0);
        SpawnPiece("white", "bishop", 3, 0);
        SpawnPiece("white", "queen", 4, 0);
        SpawnPiece("white", "king", 5, 0);
        SpawnPiece("white", "bishop", 6, 0);
        SpawnPiece("white", "knight", 7, 0);
        SpawnPiece("white", "rook", 8, 0);
        //White frontline
        SpawnPiece("white", "pawn", 0, 1);
        SpawnPiece("white", "pawn", 1, 1);
        SpawnPiece("white", "pawn", 2, 1);
        SpawnPiece("white", "pawn", 3, 1);
        SpawnPiece("white", "pawn", 4, 1);
        SpawnPiece("white", "pawn", 5, 1);
        SpawnPiece("white", "pawn", 6, 1);
        SpawnPiece("white", "pawn", 7, 1);
        SpawnPiece("white", "pawn", 8, 1);
        SpawnPiece("white", "pawn", 9, 1);
        whiteSpawner = SpawnPiece("white", "spawner", 5, 2);
        //Black frontline
        SpawnPiece("black", "pawn", 0, 8);
        SpawnPiece("black", "pawn", 1, 8);
        SpawnPiece("black", "pawn", 2, 8);
        SpawnPiece("black", "pawn", 3, 8);
        SpawnPiece("black", "pawn", 4, 8);
        SpawnPiece("black", "pawn", 5, 8);
        SpawnPiece("black", "pawn", 6, 8);
        SpawnPiece("black", "pawn", 7, 8);
        SpawnPiece("black", "pawn", 8, 8);
        SpawnPiece("black", "pawn", 9, 8);
        blackSpawner = SpawnPiece("black", "spawner", 5, 7);
        //Black backline
        SpawnPiece("black", "rook", 1, 9);
        SpawnPiece("black", "knight", 2, 9);
        SpawnPiece("black", "bishop", 3, 9);
        SpawnPiece("black", "queen", 4, 9);
        SpawnPiece("black", "king", 5, 9);
        SpawnPiece("black", "bishop", 6, 9);
        SpawnPiece("black", "knight", 7, 9);
        SpawnPiece("black", "rook", 8, 9);
    }
    public GameObject SpawnPiece(string team, string pieceType, int x, int y)
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

        return newPiece;
    }

    public void SpawnMovePlates()
    {
        GameObject selectedObject = gameManager.selectedObject;
        if (selectedObject != null && selectedObject.GetComponent<ChessPiece>().GetTeam() != gameManager.currentPlayer)
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
            case "spawner":
                SpawnerMovePlates();
                break;
        }
    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject go in movePlates)
            Destroy(go);
    }

    public void MoveToPlate(MovePlate mp)
    {
        GameObject selectedObject = gameManager.selectedObject;
        ChessPiece movingPiece = selectedObject.GetComponent<ChessPiece>();
        int movePlateX = mp.GetBoardX();
        int movePlateY = mp.GetBoardY();
        string pieceTypeAtDestination = null;

        if (boardMatrix[movePlateX, movePlateY] != null)
        {
            pieceTypeAtDestination = boardMatrix[movePlateX, movePlateY].GetComponent<ChessPiece>().GetPieceType();
        }

        if (mp.IsAttack())
        {
            Destroy(boardMatrix[movePlateX, movePlateY]);
            captureInventory.ChangeInventory(gameManager.currentPlayer, pieceTypeAtDestination, 1);
            //boardMatrix[movePlateX, movePlateY] = null;
        }

        boardMatrix[movingPiece.GetBoardX(), movingPiece.GetBoardY()] = null;
        boardMatrix[movePlateX, movePlateY] = movingPiece.gameObject;
        movingPiece.MoveToPosition(movePlateX, movePlateY);
        gameManager.ClearSelection();

        if (pieceTypeAtDestination == "king")
        {
            gameManager.GameOver();
            return;
        }

        if (movingPiece.GetPieceType() == "pawn")
        {
            if (movingPiece.GetBoardY() == 9 && movingPiece.GetTeam() == "white")
            {
                gameManager.promotionMenu.PromotePawn(movingPiece);
            }
            else if (movingPiece.GetBoardY() == 0 && movingPiece.GetTeam() == "black")
            {
                gameManager.promotionMenu.PromotePawn(movingPiece);
            }
        }

        gameManager.SwitchPlayer();
    }

    public bool TileIsOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x + 1 > boardMatrix.GetLength(0) || y + 1 > boardMatrix.GetLength(1))
            return false;
        else
            return true;
    }

    public GameObject GetAtPosition(int x, int y)
    {
        return boardMatrix[x, y];
    }

    public void SpawnMovePlateAt(int x, int y)
    {
        GameObject selectedObject = gameManager.selectedObject;

        if (!TileIsOnBoard(x, y))
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        ChessPiece pieceAtTargetTile = null;
        if (GetAtPosition(x, y))
            pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();

        if (pieceAtTargetTile != null && pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam())
            return;
        if (pieceAtTargetTile != null && pieceAtTargetTile.GetPieceType() == "spawner")
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.MoveToPosition(x, y);
        newPlateScript.UpdatePosition();

        if (pieceAtTargetTile != null)
            newPlateScript.ConvertToAttack();
    }

    public void SpawnMoveOnlyPlateAt(int x, int y)
    {
        GameObject selectedObject = gameManager.selectedObject;

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

    public void SpawnAttackOnlyPlateAt(int x, int y)
    {
        GameObject selectedObject = gameManager.selectedObject;
        if (!TileIsOnBoard(x, y))
            return;

        if (GetAtPosition(x, y) == null)
            return;

        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        ChessPiece pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();

        if (pieceAtTargetTile != null && pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam())
            return;
        if (pieceAtTargetTile != null && pieceAtTargetTile.GetPieceType() == "spawner")
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.SetBoardX(x);
        newPlateScript.SetBoardY(y);
        newPlateScript.UpdatePosition();
        newPlateScript.ConvertToAttack();
    }

    public void SpawnSpawnPlateAt(int x, int y)
    {
        if (!TileIsOnBoard(x, y))
            return;

        GameObject newPlate = Instantiate(movePlate, Vector3.zero, Quaternion.identity);
        MovePlate newPlateScript = newPlate.GetComponent<MovePlate>();
        newPlateScript.MoveToPosition(x, y);
        newPlateScript.UpdatePosition();
        newPlateScript.ConvertToSpawn();
    }

    public void LineMovePlates(int xIncrement, int yIncrement)
    {
        GameObject selectedObject = gameManager.selectedObject;
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX() + xIncrement;
        int y = selectedPiece.GetBoardY() + yIncrement;

        while (TileIsOnBoard(x, y))
        {
            if (GetAtPosition(x, y) != null)
            {
                ChessPiece pieceAtTargetTile = GetAtPosition(x, y).GetComponent<ChessPiece>();
                if (pieceAtTargetTile != null && pieceAtTargetTile.GetTeam() == selectedPiece.GetTeam() && pieceAtTargetTile.GetPieceType() != "spawner")
                    break;
                if (pieceAtTargetTile.GetTeam() != selectedPiece.GetTeam() && pieceAtTargetTile.GetPieceType() != "spawner")
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

    public void KingMovePlates()
    {
        GameObject selectedObject = gameManager.selectedObject;
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
        GameObject selectedObject = gameManager.selectedObject;
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX();
        int y = selectedPiece.GetBoardY();

        SpawnMovePlateAt(x - 2, y - 1);
        SpawnMovePlateAt(x - 2, y + 1);
        SpawnMovePlateAt(x - 1, y - 2);
        SpawnMovePlateAt(x - 1, y + 2);
        SpawnMovePlateAt(x + 1, y - 2);
        SpawnMovePlateAt(x + 1, y + 2);
        SpawnMovePlateAt(x + 2, y - 1);
        SpawnMovePlateAt(x + 2, y + 1);
    }

    void PawnMovePlates()
    {
        GameObject selectedObject = gameManager.selectedObject;
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
        if (selectedPiece.GetTeam() == "black" && y == 8)
            SpawnMoveOnlyPlateAt(x, forwardOneTile - 1);
    }

    void SpawnerMovePlates()
    {
        GameObject selectedObject = gameManager.selectedObject;
        ChessPiece selectedPiece = selectedObject.GetComponent<ChessPiece>();
        int x = selectedPiece.GetBoardX();
        int y = selectedPiece.GetBoardY();

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (selectedPiece.GetTeam() == "white" && j >= 7)
                    continue;
                if (selectedPiece.GetTeam() == "black" && j <= 2)
                    continue;
                SpawnMoveOnlyPlateAt(i, j);
            }
        }
    }

    public void SpawnerSpawnPlates()
    {
        GameObject spawner;
        if (spawningTeam == "white")
            spawner = whiteSpawner;
        else
            spawner = blackSpawner;

        ChessPiece spawnerScript = spawner.GetComponent<ChessPiece>();
        int x = spawnerScript.GetBoardX();
        int y = spawnerScript.GetBoardY();

        if(spawningType == "pawn")
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    GameObject pieceAtDestination = GetAtPosition(i, j);
                    if (pieceAtDestination == null)
                        SpawnSpawnPlateAt(i, j);
                }
            }
        }
        else
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    GameObject goAtDestination = GetAtPosition(i, j);
                    if (goAtDestination == null)
                        continue;

                    ChessPiece pieceAtDestination = goAtDestination.GetComponent<ChessPiece>();
                    if (pieceAtDestination.GetPieceType() == "pawn" && pieceAtDestination.GetTeam() == gameManager.currentPlayer)
                        SpawnSpawnPlateAt(i, j);
                }
            }
        }
    }

    public void SpawnFromInventory(MovePlate spawnPlate)
    {
        if (!captureInventory.HasInInventory(spawningTeam, spawningType))
            return;

        if (spawningTeam == "white")
            captureInventory.whiteScorePenalty += spawningPenalty;
        else
            captureInventory.blackScorePenalty += spawningPenalty;
        captureInventory.ChangeInventory(spawningTeam, spawningType, -1);

        int x = spawnPlate.GetBoardX();
        int y = spawnPlate.GetBoardY();
        Destroy(GetAtPosition(x, y));
        SpawnPiece(spawningTeam, spawningType, x, y);

        gameManager.SwitchPlayer();
    }

    public void SetCurrentSpawnerTo(string team_type)
    {
        string[] splitString = team_type.Split("_");
        string teamPartOfInput = splitString[0];
        string typePartOfInput = splitString[1];

        if (teamPartOfInput != gameManager.currentPlayer)
            return;
        if (!captureInventory.HasInInventory(teamPartOfInput, typePartOfInput))
            return;

        spawningTeam = teamPartOfInput;
        spawningType = typePartOfInput;

        switch (spawningType)
        {
            case "pawn":
                spawningPenalty = 0;
                break;
            case "knight":
            case "bishop":
            case "rook":
                spawningPenalty = smallSpawningPenalty;
                break;
            case "queen":
                spawningPenalty = largeSpawningPenalty;
                break;
        }
        captureInventory.ShowPenalty(spawningTeam, spawningType, spawningPenalty);

        gameManager.ClearSelection();
        SpawnerSpawnPlates();
    }
}


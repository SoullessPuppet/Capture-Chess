using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionMenu : MonoBehaviour
{
    public GameObject gameButtonsPanel;
    //public Sprite white_queen, white_bishop, white_knight, white_rook;
    //public Sprite black_queen, black_bishop, black_knight, black_rook;
    public SpriteLibrary spriteLibrary;
    public Image queenImage;
    public Image knightImage;
    public Image bishopImage;
    public Image rookImage;
    ChessPiece target;
    string team;


    public void PromotePawn(ChessPiece cp)
    {
        target = cp;
        team = cp.GetTeam();
        if (team == "white")
        {
            queenImage.sprite = spriteLibrary.white_queen;
            bishopImage.sprite = spriteLibrary.white_bishop;
            knightImage.sprite = spriteLibrary.white_knight;
            rookImage.sprite = spriteLibrary.white_rook;
        }
        else if (team == "black")
        {
            queenImage.sprite = spriteLibrary.black_queen;
            bishopImage.sprite = spriteLibrary.black_bishop;
            knightImage.sprite = spriteLibrary.black_knight;
            rookImage.sprite = spriteLibrary.black_rook;
        }
        gameButtonsPanel.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void PromoteTo(string type)
    {
        switch (type)
        {
            case "queen":
                target.SetPieceType("queen");
                break;
            case "bishop":
                target.SetPieceType("bishop");
                break;
            case "knight":
                target.SetPieceType("knight");
                break;
            case "rook":
                target.SetPieceType("rook");
                break;
        }
        target.UpdateSprite();
        this.gameObject.SetActive(false);
        gameButtonsPanel.SetActive(true);
    }
}

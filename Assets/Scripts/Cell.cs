using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int row;
    public int column;
    public CellManager cellManager;
    bool CellEmpty;
    private Image cellImage;

    void Awake()
    {
        cellImage = transform.Find("Image").GetComponent<Image>();
        if (cellImage == null)
        {
            Debug.LogError("Cell Image not found!");
        }
    }

    public void Setup(int row, int column, CellManager cellManager, string playerSymbol)
    {
        this.row = row;
        this.column = column;
        this.cellManager = cellManager;
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => cellManager.UpdateBoard(row, column, playerSymbol));
    }


    public void SetSprite(Sprite sprite)
    {
        if (cellImage != null)
        {
            cellImage.sprite = sprite;
            cellImage.raycastTarget = sprite != null;
        }
    }

    public void ResetCell()
    {
        SetSprite(null);
    }
}


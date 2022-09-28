using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap _tilemap {get; private set;}
    public Tile tileUnknown;
    public Tile tileMine;
    public Tile tileEmpty;
    public Tile tileFlag;
    public Tile tileMineExploded;
    public Tile[] tileNums;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();    
    }
    /// <summary>
    /// Update the tilemaps
    /// </summary>
    /// <param name="state"></param>
    public  void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x,y];
                _tilemap.SetTile(cell._position, GetTile(cell));
            }
        }

    }

    private Tile GetTile(Cell cell)
    {
        if(cell._revealed)
            return GetRevealedTile(cell);
        else if(cell._flagged)
            return tileFlag;
        else
            return tileUnknown;
    }
    /// <summary>
    /// Ritorna la tipologia della nostra cell
    /// </summary>
    /// <param name="cell">cellula presa in esame</param>
    /// <returns></returns>
    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell._type)
        {
            case Cell.Type.Empty : return tileEmpty;
            case Cell.Type.Mine : return cell._exploded ? tileMineExploded : tileMine ;
            case Cell.Type.Number : return GetNumberTile(cell);
            default : return null;
        }
    }


    private Tile GetNumberTile(Cell cell)
    {
        switch (cell._number)
        {
            case 1 : return tileNums[0];
            case 2 : return tileNums[1];
            case 3 : return tileNums[2];
            case 4 : return tileNums[3];
            case 5 : return tileNums[4];
            case 6 : return tileNums[5];
            case 7 : return tileNums[6];
            case 8 : return tileNums[7];
            default : return null;
        }
    }
}

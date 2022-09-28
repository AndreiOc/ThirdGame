using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16; 
    public int height = 16;
    public int mineCount = 32;
    public Board board;
    private Cell[,]state;
    private bool gameover;

    private void Awake() 
    {
        board = GetComponentInChildren<Board>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width,height];
        gameover = false;
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
        Camera.main.transform.position = new Vector3(width/2,height/2,-10);
        board.Draw(state);
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {  
                Cell cell = new Cell();
                cell._position = new Vector3Int(x,y,0);
                cell._type = Cell.Type.Empty;
                state[x,y] = cell;
            }
        }
    }
    /// <summary>
    /// Genero le mine casualamente
    /// </summary>
    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x;
            int y;    
            do
            {       
                x = Random.Range(0,width);
                y = Random.Range(0,height);    
            
            }while (state[x,y]._type == Cell.Type.Mine);
            
            state[x,y]._type = Cell.Type.Mine;
        }
   }
    /// <summary>
    /// Funzione che genera i numeri da inserire all'interno prentendo in co
    /// considerazione le mine piazzate randomicamente 
    /// </summary>
    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {  
                Cell cell = state[x,y];
                if(cell._type == Cell.Type.Mine)
                    continue;

                cell._number = CountMines(x,y);
                if(cell._number > 0)
                    cell._type = Cell.Type.Number;
                state[x,y] = cell;
            }
        }
    }
    /// <summary>
    /// Ritorna il numero di mine attorno alla nostra cella 
    /// quindi cerco nel suo intorno di 1
    /// </summary>
    /// <param name="cellX">Posizione X della cella</param>
    /// <param name="cellY">Posizione Y della cella</param>
    /// <returns></returns>
    private int CountMines(int cellX, int cellY)
    {
        int count = 0;
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {  

                if(adjacentX == 0 && adjacentY == 0)
                    continue;
                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if(GetCell(x,y)._type == Cell.Type.Mine)
                    count++;
            }
        }
        return count;
    }

    private void Update()
    {
        if(!gameover)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Flag();              
            }
            if(Input.GetMouseButtonDown(0))
            {
                Reveal();
            }
            if(Input.GetMouseButtonDown(2))
            {
                Debug.Log("Scan");
                CheckZone();
                
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
                NewGame();
        }
    }

    /// <summary>
    /// Premo sul punto del numero e devo verificare che il valore sia soddisfatto o menoi
    /// </summary>
    private void CheckZone()
    {
        Vector3 wordlPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board._tilemap.WorldToCell(wordlPosition);  

        Cell cell = GetCell(cellPosition.x,cellPosition.y);    
        if(cell._type == Cell.Type.Invalid || cell._flagged)
            return;
        switch(cell._type)
        {
            //se la cella presa in considirazione è un numero ed è soddisfatta
            case Cell.Type.Number:
                CheckCell(cell);
            break;
            case Cell.Type.Mine:
                Explode(cell);
            break;
            default :
                cell._revealed = true;
                state [cellPosition.x,cellPosition.y] = cell;
            break;    
        }        
        board.Draw(state);

    }

    private void CheckCell(Cell cell)
    {
        int count = 0;
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {  

                if(adjacentX == 0 && adjacentY == 0)
                    continue;
                int x = cell._position.x + adjacentX;
                int y = cell._position.y + adjacentY;

                if(GetCell(x,y)._type == Cell.Type.Mine)
                    count++;                    
            }
        }
        if(cell._number == count)
        {
            RevealCheck(cell);
        }
    }
    private void RevealCheck(Cell cell)
    {
        Flood(cell);
    }
    private void Reveal()
    {
        Vector3 wordlPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board._tilemap.WorldToCell(wordlPosition);  

        Cell cell = GetCell(cellPosition.x,cellPosition.y);
        if(cell._type == Cell.Type.Invalid || cell._revealed || cell._flagged)
            return;
        switch(cell._type)
        {
            case Cell.Type.Empty:
                Flood(cell);
                CheckIfIWin();
            break;
            case Cell.Type.Mine:
                Explode(cell);
            break;
            default :
                cell._revealed = true;
                state [cellPosition.x,cellPosition.y] = cell;
                break;
        }        
        board.Draw(state);
    }

    private void CheckIfIWin()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {  
                Cell cell = state[x,y];
                if(cell._type != Cell.Type.Mine && !cell._revealed)
                {
                    return;
                }

            }
        }
        Debug.Log("Hai vinto");
        gameover = true;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x,y];
                if(state[x,y]._type == Cell.Type.Mine)
                {
                    cell._flagged = true;
                    state[x,y] = cell;
                }
            }
        }        
    }

    private void Explode(Cell cell)
    {
        gameover = true;

        cell._exploded = true;
        cell._revealed = true;
        state[cell._position.x,cell._position.y] = cell;


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x,y];
                if(state[x,y]._type == Cell.Type.Mine)
                {
                    cell._revealed = true;
                    state[x,y] = cell;
                }
            }
        }
        board.Draw(state);

    }
    
    /// <summary>
    /// Cerco tutte le celle vuote vicino alla cella vuota trovata
    /// </summary>
    /// <param name="cell"></param>
    private void Flood(Cell cell)
    {
        if(cell._revealed) return;
        if(cell._type == Cell.Type.Mine || cell._type == Cell.Type.Invalid) return;

        cell._revealed = true;
        state[cell._position.x,cell._position.y] = cell;
        if(cell._type == Cell.Type.Empty)
        {
            Flood(GetCell(cell._position.x -1, cell._position.y ));
            Flood(GetCell(cell._position.x +1, cell._position.y ));
            Flood(GetCell(cell._position.x , cell._position.y -1 ));
            Flood(GetCell(cell._position.x , cell._position.y + 1));
        }


    }

    /// <summary>
    /// Flag la tile
    /// </summary>
    private void Flag()
    {
        Vector3 wordlPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board._tilemap.WorldToCell(wordlPosition);  
        Debug.Log(cellPosition.x + " " +  cellPosition.y);

        Cell cell = GetCell(cellPosition.x,cellPosition.y);

        if(cell._type == Cell.Type.Invalid || cell._revealed)
            return;
        
        cell._flagged = !cell._flagged;
        state [cellPosition.x,cellPosition.y] = cell;
        board.Draw(state);
        
    }
    private Cell GetCell(int x, int y)
    {
        if(IsValid(x,y))
            return state[x,y];
        else
            return new Cell();
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

}

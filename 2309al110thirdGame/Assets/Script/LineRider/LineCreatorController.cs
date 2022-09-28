using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreatorController : MonoBehaviour
{
    public GameObject[] linePrefab;
    public Texture2D []curserArrow;
    Line activeLine;
    int selectedLine = 0;
    private void Start() {
        Debug.Log(linePrefab.Length);
    }
    void Update()
    {
        SelectLine();
        if(Input.GetMouseButtonDown(0))
        {
            GameObject lineGO = Instantiate(linePrefab[selectedLine]);
            activeLine = lineGO.GetComponent<Line>();
        }        

        if(Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }
        if(activeLine !=null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);

        }
    }

    void SelectLine()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedLine > linePrefab.Length - 1)
                selectedLine = 0;
            else
                selectedLine++;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedLine <= 0)
                selectedLine = linePrefab.Length - 1;
            else
                selectedLine--;
        }
        Cursor.SetCursor(curserArrow[selectedLine],Vector2.zero,CursorMode.ForceSoftware);

    }

}

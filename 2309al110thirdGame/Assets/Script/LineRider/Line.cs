using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{


    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol2D;

    List<Vector2> points;
    public void UpdateLine(Vector2 mousePos)
    {
        if(points == null)
        {
            points = new List<Vector2>();
            SetPoint(mousePos);
            return;
        }
        if(Vector2.Distance(points.Last(), mousePos) > .1f)
        {
            SetPoint(mousePos);
        }

    }

    /// <summary>
    /// Inserisco i punti sullo schermo
    /// </summary>
    /// <param name="point"></param>
    private void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;//TODO ATTENZIONE
        lineRenderer.SetPosition(points.Count - 1, point);
        if(points.Count > 1)
            edgeCol2D.points = points.ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

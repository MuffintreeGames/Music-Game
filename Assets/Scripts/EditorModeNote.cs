using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeNote : MonoBehaviour
{
    bool beingDragged = false;
    int closestColumn = -1;

    static float upperYBound = 2.65f;
    static float lowerYBound = -2.65f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (beingDragged)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            closestColumn = (int)Mathf.Clamp(Mathf.Floor((transform.localPosition.x - ChartManager.leftmostColumnPlacement - ChartManager.columnWidth/2) / ChartManager.columnWidth), -1, 8) + 1;
            Debug.Log("calculated closest column: " + closestColumn);
            transform.localPosition = new Vector2(ChartManager.leftmostColumnPlacement + closestColumn*ChartManager.columnWidth, Mathf.Clamp(transform.localPosition.y, lowerYBound, upperYBound));
        }
    }

    public void Dragged()
    {
        if (ChartManager.selectedMode != "Drag")
        {
            return;
        }
        Debug.Log("Getting dragged");
        beingDragged = true;
    }

    public void StopDrag()
    {
        if (ChartManager.selectedMode != "Drag")
        {
            return;
        }
        Debug.Log("No longer getting dragged");
        beingDragged = false;
        GameObject.Find("ChartManager").GetComponent<ChartManager>().MoveNote(gameObject, closestColumn);
    }

    public void Delete()
    {
        if (ChartManager.selectedMode != "Delete")
        {
            return;
        }
        Debug.Log("Getting deleted");
        GameObject.Find("ChartManager").GetComponent<ChartManager>().DeleteNote(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridPlanner : MonoBehaviour//wish you commented this or left a guide :)))))))))
{
    [SerializeField]
    RectTransform gridParent;
    [SerializeField]
    GameObject gridObject;
    RectTransform gridTransform;
    [SerializeField]
    Vector2 areaPad, objPad;
    [SerializeField]
    int count;

    Vector2 gridSize, activeArea, objSize;

    [SerializeField]
    bool autoPadX, autoPadY;
    [SerializeField]
    int rowBreak, rowLimit;

    [SerializeField]
    bool autoRow;

    [HideInInspector]
    public List<GameObject> gridObjects;

    public List<Vector2> GridPositions()
    {
        gridSize = gridParent.rect.size;

        gridTransform = gridObject.GetComponent<RectTransform>();

        objSize = gridTransform.rect.size;

        activeArea = gridSize - (areaPad * 2);


        List<Vector2> positions = new List<Vector2>();


        Vector2 basePosition = Vector2.zero;
        basePosition.x += areaPad.x;
        basePosition.x += objSize.x / 2;
        basePosition.y -= areaPad.y;
        basePosition.y -= objSize.y / 2;

        float baseX = basePosition.x;

        
        if(autoPadX)
        {
            autoRow = false;
            objPad.x = (int)((activeArea.x - (objSize.x * rowBreak)) / (rowBreak - 1));
        }
        if (autoPadY)
        {
            objPad.y = (int)((activeArea.y - (objSize.y * rowLimit)) / (rowLimit - 1));
        }

        if (autoRow)
        {
            autoPadX = false;
            rowBreak = (int)((activeArea.x + objPad.x) / (objPad.x + objSize.x));
        }


        for(int i = 0; i < count; i++)
        {
            positions.Add(basePosition);
            if((i+1) % rowBreak == 0 && (i + 1) / rowBreak > 0)//if on row break, shift y and reset x
            {
                basePosition.y -= objPad.y + objSize.y;
                basePosition.x = baseX;
            }
            else//shift x
            {
                basePosition.x += objPad.x + objSize.x;
            }
        }

        return positions;
    }
    #if UNITY_EDITOR
    public void FillGrid()
    {
        List<Vector2> positions = GridPositions();

        if(gridObjects == null)
        {
            gridObjects = new List<GameObject>();
        }
        else
        {
            ClearGrid();
        }

        foreach (Vector2 position in positions)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(gridObject);
            newObj.GetComponent<RectTransform>().parent = gridParent;
            newObj.GetComponent<RectTransform>().localPosition = position;
            gridObjects.Add(newObj);
            //gridObjects.Add(Instantiate(gridObject, gridParent.position+(Vector3)position, gridTransform.rotation, gridParent));
        }
    }

    public void ClearGrid()
    {
        if (gridObjects == null)
            return;

        foreach (GameObject gridObject in gridObjects)
        {
            DestroyImmediate(gridObject);
        }
        gridObjects.Clear();
    }

    #endif
}

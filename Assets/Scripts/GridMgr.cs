using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridMgr : MonoSingleton<GridMgr>
{

    private Grid<GridElement> map;

    public GridElement elementPrefab;


    private void Start()
    {
        InitMap();
    }

    private void InitMap()
    {
        map = new Grid<GridElement>(30, 30);

        for (int i = 0; i < map.size.x; i++)
        {
            for (int j = 0; j < map.size.y; j++)
            {
                if (Random.Range(0f, 1f) < 0.4f)
                {
                    InstantiateGridElement(new Vector2Int(i, j));   
                }
            }
        }
        
        RefreshDisplay();
    }


    private void InstantiateGridElement(Vector2Int idx)
    {
        map[idx] = Instantiate(elementPrefab, new Vector3(idx.x - map.size.x / 2f, idx.y - map.size.y / 2f, 0f), Quaternion.identity);

        map[idx].mapIdx = idx;
        map[idx].mapAffiliated = map;
        map[idx].UpdateDisplay();
    }

    private void RefreshDisplay()
    {
        foreach (GridElement element in map)
        {
            element?.UpdateDisplay();
        }
    }
}

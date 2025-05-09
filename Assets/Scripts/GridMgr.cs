using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridMgr : MonoSingleton<GridMgr>
{

    private Grid<GridElement> map;

    public GridElement elementPrefab;

    private GridElement player;

    private void Start()
    {
        InitMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMovePlayer(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            TryMovePlayer(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMovePlayer(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMovePlayer(Vector2Int.down);
        }
    }

    private void InitMap()
    {
        map = new Grid<GridElement>(30, 30);

        for (int i = 0; i < map.size.x; i++)
        {
            for (int j = 0; j < map.size.y; j++)
            {
                if (i == map.size.x / 2 && j == map.size.y / 2)
                {
                    player = InstantiateGridElement(new Vector2Int(i, j), false);
                    player.text.text = "P";
                    player.text.color = Color.green;
                }
                else if (Random.Range(0f, 1f) < 0.4f)
                {
                    var rock = InstantiateGridElement(new Vector2Int(i, j), true);   
                    rock.text.text = "R";
                    rock.text.color = Color.grey;
                }
            }
        }
        
        RefreshDisplay();
    }


    private void TryMovePlayer(Vector2Int direction)
    {
        var newPosition = player.mapIdx + direction;

        if (map.IsPositionValid(newPosition) && map[newPosition] == null)
        {
            map[player.mapIdx] = null;
            map[newPosition] = player;

            player.mapIdx = newPosition;

            player.transform.position = new Vector3(newPosition.x - map.size.x / 2f, newPosition.y - map.size.y / 2f, 0f);
        }
    }
    
    private GridElement InstantiateGridElement(Vector2Int idx, bool isMine)
    {
        map[idx] = Instantiate(elementPrefab, new Vector3(idx.x - map.size.x / 2f, idx.y - map.size.y / 2f, 0f), Quaternion.identity, transform);

        map[idx].isMine = isMine;
        map[idx].mapIdx = idx;
        map[idx].mapAffiliated = map;
        map[idx].UpdateDisplay();

        return map[idx];
    }

    private void RefreshDisplay()
    {
        foreach (GridElement element in map)
        {
            element?.UpdateDisplay();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridMgr : MonoSingleton<GridMgr>
{

    private Grid<GridElement> map;

    public GridElement elementPrefab;

    private GridElement player;

    public float moveInterval;
    private float moveTimer;

    public Vector2Int mapSize;
    public float perlinNoiseRatio = 0.04f;
    public float rockThreshold = 0.4f;

    public List<GridElement> enemies = new List<GridElement>();

    public List<float> enemyTimers = new List<float>();
    public float enemyMoveInterval;
    
    private void Start()
    {
        InitMap();

        moveTimer = 0f;
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;
        
        if (Input.GetKey(KeyCode.A) && moveTimer > moveInterval)
        {
            TryMovePlayer(Vector2Int.left);
            moveTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.W) && moveTimer > moveInterval)
        {
            TryMovePlayer(Vector2Int.up);
            moveTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.D) && moveTimer > moveInterval)
        {
            TryMovePlayer(Vector2Int.right);
            moveTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.S) && moveTimer > moveInterval)
        {
            TryMovePlayer(Vector2Int.down);
            moveTimer = 0f;
        }

        for (int i = 0; i < enemyTimers.Count; i++)
        {
            enemyTimers[i] += Time.deltaTime;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && enemyTimers[i] > enemyMoveInterval)
            {
                enemyTimers[i] = 0f;
                
                // move
                var path = AStarPathFinding.FindPath(map, element => element == null || !element.isMine, 
                    enemies[i].mapIdx, 
                    player.mapIdx);
                
                if(path == null || path.Count == 0) continue;

                var nextPos = path[0];
                if (map.IsPositionValid(nextPos) && map[nextPos] == null)
                {
                    map[enemies[i].mapIdx] = null;
                    
                    map[nextPos] = enemies[i];
                    enemies[i].mapIdx = nextPos;

                    enemies[i].transform.position = new Vector3(nextPos.x - map.size.x / 2f, nextPos.y - map.size.y / 2f, 0f);   
                }

            }
        }
    }

    private void LateUpdate()
    {
        Camera.main.transform.DOMoveX(player.transform.position.x, 0.5f);
        Camera.main.transform.DOMoveY(player.transform.position.y, 0.5f);
    }

    private void InitMap()
    {
        map = new Grid<GridElement>(mapSize.x, mapSize.y);

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
                else if (Mathf.PerlinNoise(i * perlinNoiseRatio, j * perlinNoiseRatio) > rockThreshold
                && Random.Range(0f,1f) < 0.003f)
                {
                    var enemy = InstantiateGridElement(new Vector2Int(i, j), false);
                    enemy.text.text = "E";
                    enemy.text.color = Color.red;
                    
                    enemies.Add(enemy);
                    enemyTimers.Add(0.0f);
                }
                else if (Mathf.PerlinNoise(i * perlinNoiseRatio, j * perlinNoiseRatio) < rockThreshold)
                {
                    var rock = InstantiateGridElement(new Vector2Int(i, j), true);
                    rock.mineType = "R";
                    rock.text.color = Color.grey;
                }
            }
        }
        
        RefreshDisplay();
    }


    
    
    private void TryMovePlayer(Vector2Int direction)
    {
        var newPosition = player.mapIdx + direction;

        if (map.IsPositionValid(newPosition))
        {
            if (map[newPosition] == null)
            {
                map[player.mapIdx] = null;
                map[newPosition] = player;

                player.mapIdx = newPosition;

                player.transform.position = new Vector3(newPosition.x - map.size.x / 2f, newPosition.y - map.size.y / 2f, 0f);   
            }
            else
            {
                // break the rock
                Destroy(map[newPosition].gameObject);
                map[newPosition] = null;

                player.text.transform.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f, 0f));
                
                RefreshDisplay();
            }
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

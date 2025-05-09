using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public SpriteRenderer frameSpr;
    public TMP_Text text;

    public Grid<GridElement> mapAffiliated;
    public Vector2Int mapIdx;
    
    public void UpdateDisplay()
    {
        if(!mapAffiliated.IsPositionValid(mapIdx)) return;


        bool isTopAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.up) 
                             && mapAffiliated[mapIdx + Vector2Int.up] != null;
        bool isRightAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.right) 
                             && mapAffiliated[mapIdx + Vector2Int.right] != null;
        bool isDownAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.down) 
                             && mapAffiliated[mapIdx + Vector2Int.down] != null;
        bool isLeftAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.left) 
                             && mapAffiliated[mapIdx + Vector2Int.left] != null;

        frameSpr.sprite = DataMgr.Instance.GetFrameSprite(isTopAdjacent, isRightAdjacent, isDownAdjacent, isLeftAdjacent);

    }
}

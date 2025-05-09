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

    public bool isMine;
    
    public void UpdateDisplay()
    {
        if(!mapAffiliated.IsPositionValid(mapIdx)) return;

        if (isMine)
        {
            bool isTopAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.up) 
                                 && mapAffiliated[mapIdx + Vector2Int.up] != null
                                 && mapAffiliated[mapIdx + Vector2Int.up].isMine;
            bool isRightAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.right) 
                                   && mapAffiliated[mapIdx + Vector2Int.right] != null
                                   && mapAffiliated[mapIdx + Vector2Int.right].isMine;
            bool isDownAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.down) 
                                  && mapAffiliated[mapIdx + Vector2Int.down] != null
                                  && mapAffiliated[mapIdx + Vector2Int.down].isMine;
            bool isLeftAdjacent = mapAffiliated.IsPositionValid(mapIdx + Vector2Int.left) 
                                  && mapAffiliated[mapIdx + Vector2Int.left] != null
                                  && mapAffiliated[mapIdx + Vector2Int.left].isMine;
        
            frameSpr.sprite = DataMgr.Instance.GetFrameSprite(isTopAdjacent, isRightAdjacent, isDownAdjacent, isLeftAdjacent);
        }
        else
        {
            frameSpr.sprite = null;
        }

    }
}

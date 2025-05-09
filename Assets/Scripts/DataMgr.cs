using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoSingleton<DataMgr>
{
    public List<Sprite> frameSprites;

    public Sprite GetFrameSprite(bool topAdjacent, bool rightAdjacent, bool downAdjacent, bool leftAdjacent)
    {
        int binaryIdx = (!topAdjacent ? 1 << 3 : 0) |
                        (!rightAdjacent ? 1 << 2 : 0) |
                        (!downAdjacent ? 1 << 1 : 0) |
                        (!leftAdjacent ? 1 << 0 : 0);

        return frameSprites[binaryIdx];
    }
}

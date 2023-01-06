using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TileList
{
    public List<TileSerialize> tiles;
    /*
     * info stuff like:
     * name,
     * date created,
     * date last edited,
     * etc.
     */

    public TileList()
    {
        this.tiles = new List<TileSerialize>();
    }
}


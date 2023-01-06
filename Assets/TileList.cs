using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TileList
{
    public List<TileSerialize> tiles;

    public string name;

    //date created...

    //last edited...

    //etc....

    public TileList()
    {
        this.tiles = new List<TileSerialize>();
        this.name = "";
    }
}


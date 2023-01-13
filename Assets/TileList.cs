using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public TileList(string name)
    {
        this.name = name;
    }

    public String toString()
    {
        string str = "";
        foreach(TileSerialize tile in this.tiles)
        {
            str +=("("+tile.toString()+"), ");
        }
        return "level:"+name+", info:"+str;
    }
}


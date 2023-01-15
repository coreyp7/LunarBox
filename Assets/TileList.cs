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

    public float playerSpeed;
    public float playerJumpForce;

    //date created...

    //last edited...

    //etc....

    public TileList()
    {
        this.tiles = new List<TileSerialize>();
        this.name = "";
        playerSpeed = 10;
        playerJumpForce = 11;
    }

    public TileList(string name)
    {
        this.name = name;
        this.tiles = new List<TileSerialize>();
        playerSpeed = 10;
        playerJumpForce = 11;
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


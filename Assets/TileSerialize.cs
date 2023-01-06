using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSerialize
{
    // Tiles always have integer positions except for checkpoints,
    // which are treated the same as Tiles during serialization.
    // So, these have to be floats.
    public TileSerialize(float x, float y, string type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    public float x;
    public float y;
    public string type;

}

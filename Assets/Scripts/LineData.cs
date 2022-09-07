using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineData
{
    public Vector3 Position;
    public Color Color;
    public Vector3[] Points;
}

public class BoardData
{
    public List<LineData> Data;

    public BoardData()
    {
        Data = new List<LineData>();
    }
}


using UnityEngine;

public struct Cell 
{
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number
    }

    public Vector3Int _position;
    public Type _type;
    public int _number;
    public bool _revealed;
    public bool _flagged;
    public bool _exploded;

}

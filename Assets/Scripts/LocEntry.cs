using System;
using UnityEngine;

[Serializable]
public class LocEntry
{
    public string key;
    [TextArea(1, 3)] public string en;
    [TextArea(1, 3)] public string fr;
    [TextArea(1, 3)] public string sp;
}
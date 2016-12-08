using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DLandform
{
    public int id;
    public string name;
    public float movein_cost_base;
}

[Serializable]
public class DLandformCollection
{
    public List<DLandform> dLandformList = new List<DLandform>();
    public Dictionary<int, DLandform> dLandformDic = new Dictionary<int, DLandform>();
}

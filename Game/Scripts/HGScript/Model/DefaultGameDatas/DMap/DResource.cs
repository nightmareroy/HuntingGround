using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DResource
{
    public int id;
    public string name;
}

[Serializable]
public class DResourceCollection
{
    public List<DResource> dResourceList = new List<DResource>();
    public Dictionary<int, DResource> dResourceDic = new Dictionary<int, DResource>();
}
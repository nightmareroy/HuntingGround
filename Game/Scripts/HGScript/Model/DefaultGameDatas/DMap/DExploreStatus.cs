using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DExploreStatus
{
    public int id;
    public string name;
}

[Serializable]
public class DExploreStatusCollection
{
    public List<DExploreStatus> dExploreStatusList = new List<DExploreStatus>();
    public Dictionary<int, DExploreStatus> dDetectiveStatusDic = new Dictionary<int, DExploreStatus>();
}
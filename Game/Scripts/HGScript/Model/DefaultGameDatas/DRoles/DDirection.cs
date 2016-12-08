using System;
using System.Collections.Generic;

[Serializable]
public class DDirection
{
    public int id;
    public string name;
}

[Serializable]
public class DDirectionCollection
{
    public List<DDirection> dDirectionList = new List<DDirection>();
    public Dictionary<int, DDirection> dDirectionDic = new Dictionary<int, DDirection>();
}
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DBuildingType
{
    public int id;
    public string name;
}

[Serializable]
public class DBuildingTypeCollection
{
    public List<DBuildingType> dBuildingTypeList = new List<DBuildingType>();
    public Dictionary<int, DBuildingType> dBuildingTypeDic = new Dictionary<int, DBuildingType>();
}


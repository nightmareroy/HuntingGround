using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

//[Serializable]
public class DResource
{
    public int resource_id;
    public string desc;

    public DResource(int resource_id,string desc)
    {
        this.resource_id = resource_id;
        this.desc = desc;
    }
}
//
//[Serializable]
public class DResourceCollection
{
//    public List<DResource> dResourceList = new List<DResource>();
    public Dictionary<int, DResource> dResourceDic = new Dictionary<int, DResource>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dResourceDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
            int resource_id = int.Parse(jo["resource_id"].ToString());
            string desc = jo["desc"].ToString();



            DResource dResource = new DResource(resource_id,desc);
            dResourceDic.Add(dResource.resource_id,dResource);
        }
    }
}
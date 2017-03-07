using System;
using System.Collections.Generic;
using SimpleJson;
using UnityEngine;


public class DDirection
{
    public int direction_did;

    public string name;

    public List<int> role_did;
    public List<int> building_did;
    public int building_uid;
    public int building_group;
    public List<int> landform;
    public List<int> resource;

    public int hide;

//    public DDirection(int direction_did,string name)
//    {
//        this.direction_did = direction_did;
//        this.name = name;
//    }
}

//[Serializable]
public class DDirectionCollection
{
//    public List<DDirection> dDirectionList = new List<DDirection>();
    public Dictionary<int, DDirection> dDirectionDic = new Dictionary<int, DDirection>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dDirectionDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
//            int directionid = int.Parse(jo["directionid"].ToString());
//            string name = jo["name"].ToString();



            DDirection dDirection = SimpleJson.SimpleJson.DeserializeObject<DDirection>(jo.ToString());

                //new DDirection(directionid,name);
            dDirectionDic.Add(dDirection.direction_did,dDirection);
        }
    }
}
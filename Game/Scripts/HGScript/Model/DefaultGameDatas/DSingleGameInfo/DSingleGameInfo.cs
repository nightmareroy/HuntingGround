using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class DSingleGameInfo
{
    public int progress_id;
    public string name;
    public string win_condition;

    public List<int> food_ids;
    public List<int> direction_dids;
    public List<int> landform_map;
    public List<int> resource_map;
    public List<int> meat_map;
}

public class DSingleGameInfoCollection
{
    public Dictionary<int, DSingleGameInfo> dSingleGameInfoDic = new Dictionary<int, DSingleGameInfo>();

    public void InitFromStr(string jsstr)
    {

        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dSingleGameInfoDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DSingleGameInfo dSingleGameInfo = SimpleJson.SimpleJson.DeserializeObject<DSingleGameInfo>(jo.ToString());
            dSingleGameInfoDic.Add(dSingleGameInfo.progress_id,dSingleGameInfo);
        }
    }
}
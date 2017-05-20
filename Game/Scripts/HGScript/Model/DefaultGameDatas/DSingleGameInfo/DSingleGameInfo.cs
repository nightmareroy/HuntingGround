using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class DSingleGameInfo
{
    public int progress_id;
    public string name;
    public string win_condition;
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
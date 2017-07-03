using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;
using UnityEngine;


public class DBuilding
{
    public int building_did;
    public string name;

    public string cost_key;
    public int cost_value;

    public int max_level;


}

//[Serializable]
public class DBuildingCollection
{
    //    public List<DRole> dRoleList = new List<DRole>();
    public Dictionary<int, DBuilding> dBuildingDic = new Dictionary<int, DBuilding>();

    public void InitFromStr(string jsstr)
    {

        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dBuildingDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DBuilding dBuilding = SimpleJson.SimpleJson.DeserializeObject<DBuilding>(jo.ToString());
            dBuildingDic.Add(dBuilding.building_did,dBuilding);
        }
    }
}
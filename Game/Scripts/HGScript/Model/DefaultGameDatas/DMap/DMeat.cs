using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

//[Serializable]
public class DMeat
{
    public int meat_id;
    public int last_turn;
    public int base_food;
    public int inteligent_need;
    public string desc;

    //public DMeat(int resource_id, string desc)
    //{
    //    this.meat_id = meat_id;
    //    this.desc = desc;
    //}
}
//
//[Serializable]
public class DMeatCollection
{
    //    public List<DResource> dResourceList = new List<DResource>();
    public Dictionary<int, DMeat> dMeatDic = new Dictionary<int, DMeat>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dMeatDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DMeat dMeat = SimpleJson.SimpleJson.DeserializeObject<DMeat>(jo.ToString());

            dMeatDic.Add(dMeat.meat_id, dMeat);
        }
    }
}
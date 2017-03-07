using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


public class DLandform
{
    public int landform_id;
    public float cost;
    public string desc;

//    public DLandform(int landform_id,string desc)
//    {
//        this.landform_id = landform_id;
//        this.desc = desc;
//    }
}

//[Serializable]
public class DLandformCollection
{
//    public List<DLandform> dLandformList = new List<DLandform>();
    public Dictionary<int, DLandform> dLandformDic = new Dictionary<int, DLandform>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dLandformDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
//            int landform_id = int.Parse(jo["landform_id"].ToString());
//            string name = jo["desc"].ToString();



//            DLandform dLandform = new DLandform(landform_id,name);
            DLandform dLandform=SimpleJson.SimpleJson.DeserializeObject<DLandform>(jo.ToString());
            dLandformDic.Add(dLandform.landform_id,dLandform);
        }
    }
}

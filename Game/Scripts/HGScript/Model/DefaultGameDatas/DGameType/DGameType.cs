using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


public class DGameType
{
    public int gametype_id;
    public List<int> playercount_in_group;
    public int width;
    public int height;
    public string desc;
    public string win_condition;

    //public DGameType(int gametypeid,List<int> playercount_in_group,int width,int height,string desc)
    //{
    //    this.gametype_id = gametypeid;
    //    this.playercount_in_group = playercount_in_group;
    //    this.width = width;
    //    this.height = height;
    //    this.desc = desc;
    //}
}


public class DGameTypeCollection
{
    public Dictionary<int, DGameType> dGameTypeDic = new Dictionary<int, DGameType>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dGameTypeDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
            //int gametypeid = int.Parse(jo["gametype_id"].ToString());
            //List<int> playercount_in_group = SimpleJson.SimpleJson.DeserializeObject<List<int>>(jo["playercount_in_group"].ToString());
            //int width=int.Parse(jo["width"].ToString());
            //int height=int.Parse(jo["height"].ToString());
            //string desc = jo["desc"].ToString();

            //DGameType dGameType = new DGameType(gametypeid, playercount_in_group, width, height, desc);
            DGameType dGameType = SimpleJson.SimpleJson.DeserializeObject<DGameType>(jo.ToString());
            dGameTypeDic.Add(dGameType.gametype_id,dGameType);
        }
    }
}

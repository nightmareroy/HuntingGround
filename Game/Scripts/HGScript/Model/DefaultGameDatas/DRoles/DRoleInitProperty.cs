using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;
using UnityEngine;


public class DRole
{
    public int role_did;
    public string name;
    public int attack;
    public int defence;
    public int speed_lv;
    public int sightzoon;
    public int max_health;
    public int health_recovery;
    public float retreat_threshold;
    public int retreating;
    public int infight;


//    public DRole(int roledid,string name,int attack,int defence,int speed_lv,int sightzoon,int max_health,int health_recomvery,float retreat_threshold,int retreating,int infight)
//    {
//        this.roledid = roledid;
//        this.name = name;
//        this.attack = attack;
//        this.defence = defence;
//        this.speed_lv = speed_lv;
//        this.sightzoon = sightzoon;
//        this.max_health = max_health;
//        this.health_recovery = health_recovery;
//        this.retreat_threshold = retreat_threshold;
//        this.retreating = retreating;
//        this.infight = infight;
//
//    }

}

//[Serializable]
public class DRoleCollection
{
//    public List<DRole> dRoleList = new List<DRole>();
    public Dictionary<int, DRole> dRoleDic = new Dictionary<int, DRole>();

    public void InitFromStr(string jsstr)
    {
        
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dRoleDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {
//            int roledid = int.Parse(jo["roledid"].ToString());
//            string name = jo["name"].ToString();
//            int attack=int.Parse(jo["attack"].ToString());
//            int defence=int.Parse(jo["defence"].ToString());
//            int speed_lv=int.Parse(jo["speed_lv"].ToString());
//            int sightzoon=int.Parse(jo["sightzoon"].ToString());
//            int max_health=int.Parse(jo["max_health"].ToString());
//            int health_recovery=int.Parse(jo["health_recovery"].ToString());
//            int retreat_threshold=int.Parse(jo["retreat_threshold"].ToString());
//            int retreating=int.Parse(jo["retreating"].ToString());
//            int infight=int.Parse(jo["infight"].ToString());




            DRole dRole = SimpleJson.SimpleJson.DeserializeObject<DRole>(jo.ToString());
                //new DRole(roledid,name,attack,defence,max_health,speed_lv);
            dRoleDic.Add(dRole.role_did,dRole);
        }
    }
}


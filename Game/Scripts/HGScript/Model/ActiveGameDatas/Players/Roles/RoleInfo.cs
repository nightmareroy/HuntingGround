using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


public class RoleInfo
{
    public int roleid;
    public int roledid;
    public int uid;
    public int pos_id;
    public int health;
    public bool retreating;
    public int fighting_last_turn;
    public int left_progress;
    //public Dictionary<int, DSkill> roleid_skill_dic = new Dictionary<int, DSkill>();
    //public List<int> add_skill_list = new List<int>();
    public int direction_id;
    public List<int> direction_path;
    //public DirectionInfo directionInfo = null;

    public RoleInfo()
    {
    }
    public void InitFromJson(JsonObject jsonobj)
    {
        roleid = int.Parse(jsonobj["roleid"].ToString());
        roledid = int.Parse(jsonobj["roledid"].ToString());
        uid = int.Parse(jsonobj["uid"].ToString());
        pos_id = int.Parse(jsonobj["pos_id"].ToString());
        health = int.Parse(jsonobj["health"].ToString());
        retreating = bool.Parse(jsonobj["retreating"].ToString());
        fighting_last_turn = int.Parse(jsonobj["fighting_last_turn"].ToString());
        //left_progress = int.Parse(jsonobj["left_progress"].ToString());

        direction_id = int.Parse(jsonobj["direction_id"].ToString());
        direction_path = SimpleJson.SimpleJson.DeserializeObject<List<int>>(jsonobj["direction_path"].ToString());
    }


    //public RoleInfo(int id, int did, int playerid, int pos_id, int left_progress)
    //{
    //    this.id = id;
    //    this.did = did;
    //    this.playerid = playerid;
    //    //this.pos_x = pos_x;
    //    //this.pos_y = pos_y;
    //    this.pos_id = pos_id;
    //    this.left_progress = left_progress;
    //}
}


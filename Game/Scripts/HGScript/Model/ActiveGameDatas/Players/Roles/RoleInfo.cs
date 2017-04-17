using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


public class RoleInfo
{
//    [Inject]
//    public GameInfo gameInfo{ get; set;}
    public GameInfo gameInfo;


    public string role_id;
    public int role_did;
    public int uid;
    public int pos_id;

    //base
    public string name{get;set;}
    public int blood_sugar { get; set;}
    public int blood_sugar_max { get; set;}
    public int muscle { get; set;}
    public int fat { get; set;}
    public int amino_acid{ get; set;}
    public int inteligent { get; set; }
    public int breath { get; set;}
    public int digest { get; set;}
    public int courage { get; set;}
    public int far_sight { get; set;}
    public int see_through { get; set;}

    public int younger_left { get; set; }
    public int growup_left { get; set; }
    public int younger_left_max { get; set; }
    public int growup_left_max { get; set; }
//    public int alive { get; set;}

    //advance
    public float health
    {
        get { return (float)blood_sugar / (float)blood_sugar_max; }
    }
    public int weight
    {
        get { return muscle+fat; }
    }
//    public int attack
//    {
//        get { return muscle*(1+health); }
//    }
//    public int defence
//    {
//        get { return fat*(1+health); }
//    }


//    public int speed_lv
//    {
//        get
//        {
//            return (int)Math.Ceiling(2f*(float)health*(float)muscle/(float)weight)-1;
//        }
//    }

//    public int current_speed_lv
//    {
//        get
//        {
//            int temp_lv = speed_lv;
//            if (gameInfo.map_info.landform[pos_id] == 2)
//            {
//                temp_lv--;
//            }
//            if (temp_lv < 0)
//            {
//                temp_lv = 0;
//            }
//            return temp_lv;
//        }
//    }

    public float max_move
    {
        get
        {
//            return (int)Math.Pow(2,speed_lv);
            return 3f*(0.5f+health*0.5f)*(float)muscle/(float)weight;
        }
    }

    public float basal_metabolism
    {
        get { return (float)Math.Floor((float)muscle*0.3f+(float)fat*0.1f); }
    }




    public List<int> skill_id_list;

    public List<int> cook_skill_id_list;

//    public bool retreating;
//    public int fighting_last_turn;
//    public int left_progress;
    //public Dictionary<int, DSkill> roleid_skill_dic = new Dictionary<int, DSkill>();
    //public List<int> add_skill_list = new List<int>();
    public int direction_did;
    public List<int> direction_param;
    //public DirectionInfo directionInfo = null;

    public RoleInfo()
    {
    }
    public void InitFromJson(JsonObject jsonobj,GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;


        role_id = jsonobj["role_id"].ToString();
        role_did = int.Parse(jsonobj["role_did"].ToString());
        uid = int.Parse(jsonobj["uid"].ToString());
        pos_id = int.Parse(jsonobj["pos_id"].ToString());

        name = jsonobj["name"].ToString();
        blood_sugar = int.Parse(jsonobj["blood_sugar"].ToString());
        blood_sugar_max = int.Parse(jsonobj["blood_sugar_max"].ToString());
        muscle = int.Parse(jsonobj["muscle"].ToString());
        fat = int.Parse(jsonobj["fat"].ToString());
        inteligent = int.Parse(jsonobj["inteligent"].ToString());
        amino_acid = int.Parse(jsonobj["amino_acid"].ToString());
        breath = int.Parse(jsonobj["breath"].ToString());
        digest = int.Parse(jsonobj["digest"].ToString());

        younger_left = int.Parse(jsonobj["younger_left"].ToString());
        growup_left = int.Parse(jsonobj["growup_left"].ToString());
        younger_left_max = int.Parse(jsonobj["younger_left_max"].ToString());
        growup_left_max = int.Parse(jsonobj["growup_left_max"].ToString());

        courage = int.Parse(jsonobj["courage"].ToString());
        far_sight = int.Parse(jsonobj["far_sight"].ToString());
        see_through = int.Parse(jsonobj["see_through"].ToString());
//        alive = int.Parse(jsonobj["alive"].ToString());
        skill_id_list=SimpleJson.SimpleJson.DeserializeObject<List<int>>(jsonobj["skill_id_list"].ToString());
        cook_skill_id_list = SimpleJson.SimpleJson.DeserializeObject<List<int>>(jsonobj["cook_skill_id_list"].ToString());

//        retreating = bool.Parse(jsonobj["retreating"].ToString());
//        fighting_last_turn = int.Parse(jsonobj["fighting_last_turn"].ToString());
        //left_progress = int.Parse(jsonobj["left_progress"].ToString());

        direction_did = int.Parse(jsonobj["direction_did"].ToString());
//        Debug.Log(jsonobj["direction_param"].ToString());
        direction_param = SimpleJson.SimpleJson.DeserializeObject<List<int>>(jsonobj["direction_param"].ToString());
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

    public void GetDirections(int role_id)
    {
        
    }
}


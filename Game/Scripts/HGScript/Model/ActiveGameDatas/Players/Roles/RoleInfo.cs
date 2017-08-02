using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


public class RoleInfo
{
//    [Inject]
//    public GameInfo gameInfo{ get; set;}
    GameInfo gameInfo;
    DGameDataCollection dGameDataCollection;


    public string role_id;
    public int role_did;
    public int uid;
    public int pos_id;

    //base
    public string name;
    public int blood_sugar;
    //public int blood_sugar_max { get; set; }
    public int muscle;
    public int fat;
    //public int amino_acid{ get; set;}
    public int inteligent;
    //public int breath { get; set;}
    public int digest;
    //public int courage { get; set;}
    public int old;

    public int infight;
    public int get_branch;
    public int get_stone;

    public List<int> skill_id_list;
    public List<int> cook_skill_id_list;


    public class Advanced_property
    {
        public int far_sight;
        public int see_through;
        public int climb;
        public int hurl;
        public int due;
        public int taunt;
        public int fast_move;
        public int careful;
        public int cooking;
        public int pachydermia;
        public int coward;
        public int ferocious;
        public int hide;
        public int brandish;
        public int agressive;
        public int sweet_meat;
    }
    //Advanced_property advanced_property = new Advanced_property();

    public Advanced_property Get_advanced_property()
    {
        Advanced_property temp_advanced_property = new Advanced_property();

        foreach (int skill_id in skill_id_list)
        {
            DSkill dSkill = dGameDataCollection.dSkillCollection.dSkillDic[skill_id];
            for(int i=0;i<dSkill.keys.Count;i++)
            {
                string key=dSkill.keys[i];
                int add_value=dSkill.values[i];

                System.Reflection.FieldInfo fieldInfo=temp_advanced_property.GetType().GetField(key);
                int src_value = (int)fieldInfo.GetValue(temp_advanced_property);
                fieldInfo.SetValue(temp_advanced_property, src_value + add_value);
            }
        }

        return temp_advanced_property;
    }

    //skill
    
    


    //public int younger_left { get; set; }
    //public int growup_left { get; set; }
    //public int younger_left_max { get; set; }
    //public int growup_left_max { get; set; }
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
    //public int attack
    //{
    //    get { return muscle; }
    //}
    //public int defence
    //{
    //    get { return (int)Math.Round((float)weight/2f); }
    //}

    public int blood_sugar_max
    {
        get { return (int)Math.Round((float)weight / 3f); }
    }
    public int attack
    {
        get { return (int)Math.Round((float)muscle * health*0.1f); }
    }
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
            float move= 4f*(0.0f+health*1f)*(float)muscle/(float)weight;
            if (Get_advanced_property().fast_move > 0)
            {
                move += 0.5f;
            }
            return move;
        }
    }

    public int basal_metabolism
    {
        get { return (int)Math.Round((float)muscle*0.03f+(float)fat*0.01f); }
    }

    public int lipase
    {
        get { return (int)Math.Round(basal_metabolism * (2 - health)); }
    }

    public int now_grow_state
    {
        get
        {
            if (old < 1000)
            {
                return 0;
            }
            else if (old < 2000)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }


    

    //public int temp_direction_banana = 0;
    //public int temp_direction_meat = 0;
    //public int temp_direction_ant = 0;
    //public int temp_direction_egg = 0;
    //public int temp_direction_honey = 0;
    //public int temp_direction_branch = 0;

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
    public void InitFromJson(JsonObject jsonobj, GameInfo gameInfo, DGameDataCollection dGameDataCollection)
    {
        this.gameInfo = gameInfo;
        this.dGameDataCollection = dGameDataCollection;


        role_id = jsonobj["role_id"].ToString();
        role_did = int.Parse(jsonobj["role_did"].ToString());
        uid = int.Parse(jsonobj["uid"].ToString());
        pos_id = int.Parse(jsonobj["pos_id"].ToString());

        name = jsonobj["name"].ToString();
        blood_sugar = int.Parse(jsonobj["blood_sugar"].ToString());
        //blood_sugar_max = int.Parse(jsonobj["blood_sugar_max"].ToString());
        muscle = int.Parse(jsonobj["muscle"].ToString());
        fat = int.Parse(jsonobj["fat"].ToString());
        inteligent = int.Parse(jsonobj["inteligent"].ToString());
        //amino_acid = int.Parse(jsonobj["amino_acid"].ToString());
        //breath = int.Parse(jsonobj["breath"].ToString());
        digest = int.Parse(jsonobj["digest"].ToString());

        old = int.Parse(jsonobj["old"].ToString());

        //younger_left = int.Parse(jsonobj["younger_left"].ToString());
        //growup_left = int.Parse(jsonobj["growup_left"].ToString());
        //younger_left_max = int.Parse(jsonobj["younger_left_max"].ToString());
        //growup_left_max = int.Parse(jsonobj["growup_left_max"].ToString());

        //courage = int.Parse(jsonobj["courage"].ToString());
        //advanced_property.far_sight = int.Parse(jsonobj["far_sight"].ToString());
        //advanced_property.see_through = int.Parse(jsonobj["see_through"].ToString());

        //advanced_property.climb = int.Parse(jsonobj["climb"].ToString());
        //advanced_property.hurl = int.Parse(jsonobj["hurl"].ToString());
        //advanced_property.due = int.Parse(jsonobj["due"].ToString());
        //advanced_property.taunt = int.Parse(jsonobj["taunt"].ToString());
        //advanced_property.fast_move = int.Parse(jsonobj["fast_move"].ToString());
        //advanced_property.careful = int.Parse(jsonobj["careful"].ToString());
        //advanced_property.cooking = int.Parse(jsonobj["cooking"].ToString());
        //advanced_property.pachydermia = int.Parse(jsonobj["pachydermia"].ToString());
        //advanced_property.coward = int.Parse(jsonobj["coward"].ToString());
        //advanced_property.ferocious = int.Parse(jsonobj["ferocious"].ToString());
        //advanced_property.hide = int.Parse(jsonobj["hide"].ToString());
        //advanced_property.brandish = int.Parse(jsonobj["brandish"].ToString());
        //advanced_property.agressive = int.Parse(jsonobj["agressive"].ToString());
        //advanced_property.sweet_meat = int.Parse(jsonobj["sweet_meat"].ToString());
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SimpleJson;

[Serializable]
public class PlayerInfo
{

    public int uid=0;
    public string name;
    //public bool is_splayer = false;
    public int group_id;
    //public PlayerDRoleSkillInfo playerDRoleSkillInfo = new PlayerDRoleSkillInfo();
    //public Dictionary<int, Dictionary<int, DSkill>> roledid_skill_dic = new Dictionary<int, Dictionary<int, DSkill>>();
    public float banana=-1;
    public int direction_turn;

    //key是角色静态id
    //public Dictionary<int, List<int>> country_skill_dic = new Dictionary<int, List<int>>();

    public PlayerInfo(int id, bool is_splayer,int groupid)
    {
        this.uid = id;
        //this.is_splayer = is_splayer;
        this.group_id = groupid;
        
    }

    public PlayerInfo()
    {

    }

    public void InitFromJson(JsonObject jsonobj)//, SPlayerInfo sPlayerInfo)
    {
        uid = int.Parse(jsonobj["uid"].ToString());
        name=jsonobj["name"].ToString();
        group_id = int.Parse(jsonobj["group_id"].ToString());
        direction_turn = int.Parse(jsonobj["direction_turn"].ToString());

        if (jsonobj.ContainsKey("banana"))
        {
            banana=int.Parse(jsonobj["banana"].ToString());
        }
        //role_dic.Clear();
        //foreach (string key in (jsonobj["role_dic"] as JsonObject).Keys)
        //{
        //    RoleInfo roleInfo=SimpleJson.SimpleJson.DeserializeObject<RoleInfo>((jsonobj["role_dic"] as JsonObject)[key].ToString());
        //    role_dic.Add(int.Parse(key), roleInfo);
            

        //}
    }

    //角色技能一共有三种来源：1.某一类角色（同一个静态id）的技能；2.某一个角色（动态id）习得技能；3.某一个角色（动态id）装备的物品给予的技能（暂未实现）。
    //public List<DSkill> GetRoleAllSkillList(int roleid)
    //{
    //    Dictionary<int, DSkill> did_skill_dic=roledid_skill_dic[role_dic[roleid].did];
    //    Dictionary<int, DSkill> id_skill_dic = role_dic[roleid].roleid_skill_dic;
    //    Dictionary<int, DSkill> result_dic=new Dictionary<int,DSkill>();
    //    List<DSkill> skill_list = new List<DSkill>();
    //    foreach (DSkill dskill in did_skill_dic.Values)
    //    {
    //        if (!result_dic.Keys.Contains(dskill.id))
    //        {
    //            result_dic.Add(dskill.id,dskill);
    //            skill_list.Add(dskill);
    //        }
    //    }
    //    foreach (DSkill dskill in id_skill_dic.Values)
    //    {
    //        if (!result_dic.Keys.Contains(dskill.id))
    //        {
    //            result_dic.Add(dskill.id, dskill);
    //            skill_list.Add(dskill);
    //        }
    //    }
    //    skill_list.Sort((DSkill dskill1,DSkill dskill2) => 
    //    {
    //        int result=dskill1.id - dskill2.id;
    //        if ( result> 0)
    //            return 1;
    //        else if (result==0)
    //            return 0;
    //        else
    //            return -1;
    //    });
    //    return skill_list;

    //    //return result_dic.Union(did_skill_dic).Union(id_skill_dic).ToDictionary<int,DSkill>(,);
    //}

    //public List<DSkill> GetRoleActionSkillList(int roleid)
    //{
    //    List<DSkill> allSkillList = GetRoleAllSkillList(roleid);
    //    List<DSkill> resultList = new List<DSkill>();
    //    foreach (DSkill dskill in allSkillList)
    //    {
    //        if (dskill.isdirection == true)
    //        {
    //            resultList.Add(dskill);
                
    //        }
    //    }
    //    return resultList;
    //}


}


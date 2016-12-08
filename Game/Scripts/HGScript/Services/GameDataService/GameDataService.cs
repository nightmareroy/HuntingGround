using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataService
{
    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    #region skill
    public List<int> GetAllSkillIds(int roleid)
    {
        RoleInfo roleInfo=gameInfo.role_dic[roleid];
        List<int> resultList = new List<int>();

        //增加国家技能
        //if (gameInfo.allplayers_dic[playerid].country_skill_dic.ContainsKey(roleInfo.did))
        //{
        //    List<int> countrySkills = gameInfo.allplayers_dic[playerid].country_skill_dic[roleInfo.did];
        //    foreach (int id in countrySkills)
        //    {
        //        if (!resultList.Contains(id))
        //        {
        //            resultList.Add(id);
        //        }
        //    }
        //}

        //增加原生技能
        List<int> originalSkills = dGameDataCollection.dRoleCollection.dRoleDic[roleInfo.roledid].original_skills;
        foreach (int id in originalSkills)
        {
            if (!resultList.Contains(id))
            {
                resultList.Add(id);
            }
        }

        ////增加角色技能
        //List<int> roleSkills = roleInfo.add_skill_list;
        //foreach (int id in roleSkills)
        //{
        //    if (!resultList.Contains(id))
        //    {
        //        resultList.Add(id);
        //    }
        //}

        //增加装备技能（未实现）

        resultList.Sort((int id1,int id2) =>
        {
            int result = id1 - id2;
            if (result > 0)
                return 1;
            else if (result == 0)
                return 0;
            else
                return -1;
        });
        return resultList;
    }

    public List<int> GetAllDirectionIds(int roleid)
    {
        List<int> allSkillList = GetAllSkillIds(roleid);
        List<int> resultList = new List<int>();
        foreach (int id in allSkillList)
        {
            DSkill dskill=dGameDataCollection.dSkillCollection.dSkillDic[id];
            if (dskill.directionid != 0)
            {
                resultList.Add(dskill.directionid);
            }
        }
        resultList.Sort((int id1, int id2) =>
        {
            int result = id1 - id2;
            if (result > 0)
                return 1;
            else if (result == 0)
                return 0;
            else
                return -1;
        });
        return resultList;
    }

    #endregion


    #region map
    //public RoleInfo GetRoleInMap(int x,int y)
    //{
    //    foreach (PlayerInfo playerInfo in gameInfo.allplayers_dic.Values)
    //    {
    //        foreach (RoleInfo roleInfo in playerInfo.role_dic.Values)
    //        {
    //            if (roleInfo.pos_x == x && roleInfo.pos_y == y)
    //            {
    //                return roleInfo;
    //            }
    //        }
    //    }
    //    return null;
    //}

    public RoleInfo GetRoleInMap(int id)
    {
        foreach (RoleInfo roleInfo in gameInfo.role_dic.Values)
        {
            if (roleInfo.pos_id == id)
            {
                return roleInfo;
            }
        }


        //foreach (PlayerInfo playerInfo in gameInfo.allplayers_dic.Values)
        //{
        //    foreach (RoleInfo roleInfo in playerInfo.role_dic.Values)
        //    {
        //        if (roleInfo.pos_id==id)
        //        {
        //            return roleInfo;
        //        }
        //    }
        //}
        return null;
    }

    #endregion
}


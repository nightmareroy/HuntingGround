﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameDataService
{
    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}

    #region skill
    public List<int> GetAllSkillIds(string roleid)
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
//        List<int> originalSkills = dGameDataCollection.dRoleCollection.dRoleDic[roleInfo.roledid].original_skills;
//        foreach (int id in originalSkills)
//        {
//            if (!resultList.Contains(id))
//            {
//                resultList.Add(id);
//            }
//        }

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

    public List<List<int>> GetAllDirectionDids(string role_id)
    {
//        List<int> allSkillList = GetAllSkillIds(roleid);
//        List<int> resultList = new List<int>();
//        foreach (int id in allSkillList)
//        {
//            DSkill dskill=dGameDataCollection.dSkillCollection.dSkillDic[id];
//            if (dskill.directionid != 0)
//            {
//                resultList.Add(dskill.directionid);
//            }
//        }
//        resultList.Sort((int id1, int id2) =>
//        {
//            int result = id1 - id2;
//            if (result > 0)
//                return 1;
//            else if (result == 0)
//                return 0;
//            else
//                return -1;
//        });
//        return resultList;
        RoleInfo roleInfo=gameInfo.role_dic[role_id];

        List<List<int>> resultList = new List<List<int>>();
        List<int> delayDirectionDids = new List<int>();
        List<int> noDelayDirectionDids = new List<int>();

        foreach (int direction_did in dGameDataCollection.dDirectionCollection.dDirectionDic.Keys)
        {
            DDirection dDirection = dGameDataCollection.dDirectionCollection.dDirectionDic[direction_did];
            BuildingInfo buildingInfo = GetBuildingInMap(roleInfo.pos_id);
            
            if (dDirection.role_did.Count > 0)
            {
                if (!dDirection.role_did.Contains(roleInfo.role_did))
                {
                    continue;
                }
            }
            if (dDirection.building_uid == 3)
            {
                if (buildingInfo != null)
                {
                    continue;
                }
            }
            if (dDirection.building_did.Count > 0)
            {
                if (buildingInfo == null)
                {
                    continue;
                }

                else if (!dDirection.building_did.Contains(buildingInfo.building_did))
                {
                    continue;
                }
                else if (dDirection.building_uid == 1 && buildingInfo.uid != sPlayerInfo.uid)
                {
                    continue;
                }
                else if (dDirection.building_uid == 2 && buildingInfo.uid == sPlayerInfo.uid)
                {
                    continue;
                }
                else if (dDirection.building_group == 1 && gameInfo.allplayers_dic[buildingInfo.uid].group_id != gameInfo.allplayers_dic[sPlayerInfo.uid].group_id)
                {
                    continue;
                }
                else if (dDirection.building_group == 2 && gameInfo.allplayers_dic[buildingInfo.uid].group_id == gameInfo.allplayers_dic[sPlayerInfo.uid].group_id)
                {
                    continue;
                }

            }


            if (dDirection.landform.Count > 0)
            {
                if (!dDirection.landform.Contains(gameInfo.map_info.landform[roleInfo.pos_id]))
                {
                    continue;
                }
            }
            if (dDirection.resource.Count > 0)
            {
                if (!dDirection.resource.Contains(gameInfo.map_info.resource[roleInfo.pos_id]))
                {
                    continue;
                }
            }

            
            if (dDirection.meat.Count > 0)
            {
                int meat_id = gameInfo.map_info.meat[roleInfo.pos_id] / 100;
                if (!dDirection.meat.Contains(meat_id))
                {
                    continue;
                }
            }

            if (dDirection.hide == 1)
            {
                continue;
            }

            //单人游戏要判断是否解锁了指令
            if (gameInfo.gametype_id == 1)
            {
                
                if (!gameInfo.allplayers_dic[roleInfo.uid].direction_dids.Contains(dDirection.direction_did))
                {
                    continue;
                }
            }

            //哺育指令特殊判断
            //if (dDirection.direction_did == 11)
            //{
            //    if()
            //}

            //Debug.Log(dDirection.delay);


            if (dDirection.delay == 0)
            {
                noDelayDirectionDids.Add(dDirection.direction_did);
            }
            else if (dDirection.delay == 1)
            {
                delayDirectionDids.Add(dDirection.direction_did);
            }
            //resultList.Add(dDirection.direction_did);

        }


//        int role_did = gameInfo.role_dic[role_id].role_did;
//        if (dGameDataCollection.dRoleDirectionCollection.dRoleDirectionDic.ContainsKey(role_did))
//        {
//            return dGameDataCollection.dRoleDirectionCollection.dRoleDirectionDic[role_did].direction_did_list;
//        }
//        else
//        {
//            return new List<int>();
//        }

        resultList.Add(noDelayDirectionDids);
        resultList.Add(delayDirectionDids);
        return resultList;

    }

    public struct OnShowMoney
    {
        public int banana;
        public int meat;
        public int ant;
        public int egg;
        public int honey;
        //public int branch;
    }

    public OnShowMoney GetOnShowMoney()
    {
        OnShowMoney onShowMoney = new OnShowMoney();
        //foreach (string role_id in gameInfo.role_dic.Keys)
        //{
        //    RoleInfo roleInfo = gameInfo.role_dic[role_id];
        //    onShowMoney.banana += roleInfo.temp_direction_banana;
        //    onShowMoney.meat += roleInfo.temp_direction_meat;
        //    onShowMoney.ant += roleInfo.temp_direction_ant;
        //    onShowMoney.egg += roleInfo.temp_direction_egg;
        //    onShowMoney.honey += roleInfo.temp_direction_honey;
        //}
        PlayerInfo playerInfo = gameInfo.allplayers_dic[sPlayerInfo.uid];
        onShowMoney.banana += playerInfo.banana;
        onShowMoney.meat += playerInfo.meat;
        onShowMoney.ant += playerInfo.ant;
        onShowMoney.egg += playerInfo.egg;
        onShowMoney.honey += playerInfo.honey;

        return onShowMoney;
    }


//    public List<int> GetAllBuildingDirectionDids(string building_id)
//    {
//        BuildingInfo buildingInfo=gameInfo.building_dic[building_id];
//
//        List<int> allBuildingDirectionDids = new List<int>();
//
//        foreach (int building_direction_did in dGameDataCollection.dBuildingDirectionCollection.dBuildingDirectionDic.Keys)
//        {
//            DBuildingDirection dBuildingDirection = dGameDataCollection.dBuildingDirectionCollection.dBuildingDirectionDic[building_direction_did];
//
////            Debug.Log(dBuildingDirection.building_did.IndexOf(buildingInfo.building_did));
//            if (dBuildingDirection.building_did.IndexOf(buildingInfo.building_did) != -1)
//            {
//                allBuildingDirectionDids.Add(dBuildingDirection.direction_did);
//            }
//        }
//        return allBuildingDirectionDids;
//    }

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

    public BuildingInfo GetBuildingInMap(int id)
    {
        foreach (BuildingInfo buildingInfo in gameInfo.building_dic.Values)
        {
            if (buildingInfo.pos_id == id)
            {
                return buildingInfo;
            }
        }
        return null;
    }

    #endregion
}


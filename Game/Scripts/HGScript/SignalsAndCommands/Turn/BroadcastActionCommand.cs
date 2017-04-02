using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;

public class BroadcastActionCommand:Command
{
//    [Inject]
//    public RoleActionList roleActionList { get; set; }
    [Inject]
    public JsonArray actionListJS{ get; set;}

    [Inject]
    public BootstrapView bootstrapView { get; set; }

    [Inject]
    public ResourceService resourceService{ get; set;}

    [Inject]
    public DoRoleActionAnimSignal doRoleActionAnimSignal { get; set; }

    [Inject]
    public DoBuildingActionAnimSignal doBuildingActionAnimSignal { get; set; }

    [Inject]
    public DoMapUpdateSignal doMapUpdateSignal { get; set; }

    [Inject]
    public DoBananaUpdateSignal doBananaUpdateSignal{ get; set;}

    [Inject]
    public DoSightzoonUpdateSignal doSightzoonUpdateSignal{ get; set;}


    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public PlayerStateChangeQueue playerStateChangeQueue{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}
    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get;set; }

    [Inject]
    public CheckUserStateQueueSignal checkUserStateQueueSignal { get; set; }

    


    //回合动画时间
    float step_time=0.5f;
    float food_step_time = 0.2f;


    public override void Execute()
    {
        updateDirectionTurnSignal.Dispatch(-1);
        bootstrapView.StartCoroutine(updateDataAndBroadcaseAction());


//        for (int step = 0; step < 7; step++)
//        {
//            switch (step)
//            {
//                case 0:
//                    break;
//                case 1:
//                case 2:
//                case 3:
//                case 4:
//                    foreach (int role_id in roleActionList.addRolesList[step-1].Keys)
//                    {
//                        gameInfo.role_dic.Add(role_id, roleActionList.addRolesList[step - 1][role_id]);
//                    }
//
//                    foreach (int role_id in roleActionList.deleteRolesList[step-1])
//                    {
//                        gameInfo.role_dic.Remove(role_id);
//                    }
//
//                    foreach (int role_id in roleActionList.moveList[step-1].Keys)
//                    {
//                        gameInfo.role_dic[role_id].pos_id = roleActionList.moveList[step - 1][role_id];
//                    }
//
//                    break;
//                case 5:
//                    break;
//                case 6:
//                    break;
//            }
//        }

    }

    IEnumerator updateDataAndBroadcaseAction()
    {
        actionAnimStartSignal.Dispatch();

        gameInfo.current_turn++;



        for (int step = 0; step < actionListJS.Count; step++)
        {
            
            JsonObject stepJS = actionListJS[step] as JsonObject;
            Debug.Log(step);
            //Debug.Log(stepJS);
            switch (step)
            {
                case 0:
                    yield return new WaitForSeconds(0);
                    break;
                //移动
                case 1:
                    JsonArray modifiesAR=stepJS["modifies"] as JsonArray;
                    foreach (object modifyObj in modifiesAR)
                    {
                        JsonObject modifyJS = modifyObj as JsonObject;
                        DoSightModefiedAction(modifyJS);
                        yield return new WaitForSeconds(step_time);
                    }
                    break;

                //角色死亡
                case 3:
                //特殊指令中的角色、建筑、视野变化
                case 5:
                    
                    DoSightModefiedAction(stepJS);

                    //yield return new WaitForSeconds(step_time);
                    break;
                //攻击+掉血
                case 2:
                    JsonObject attackJS = stepJS["attack"] as JsonObject;
                    
                    foreach (string role_id in attackJS.Keys)
                    {
                        JsonObject attackInfoJS = attackJS[role_id] as JsonObject;
                        int type = int.Parse(attackInfoJS["type"].ToString());
                        int pos_id = int.Parse( attackInfoJS["enemy_id"].ToString());

                        DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        switch (type)
                        {
                            case 1:
                            case 2:
                                doActionAnimSignalParam.type = 5;
                                doActionAnimSignalParam.role_id = role_id;
                                doActionAnimSignalParam.value = pos_id;
                                doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                                break;
                            case 3:
                                
                                doActionAnimSignalParam.type = 6;
                                doActionAnimSignalParam.role_id = role_id;
                                doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                                break;
                        }

                    }
                    yield return new WaitForSeconds(step_time);
                    JsonObject damageJS=stepJS["damage"] as JsonObject;
                    foreach (string role_id in damageJS.Keys)
                    {
                        int damage = int.Parse(damageJS[role_id].ToString());
                        gameInfo.role_dic[role_id].blood_sugar -= damage;

                        DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        doActionAnimSignalParam.type = 3;
                        doActionAnimSignalParam.role_id = role_id;
                        doActionAnimSignalParam.value = damage;
                        doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }
                    yield return new WaitForSeconds(step_time);
                    break;
                //回血
                case 4:
                    JsonObject recoveryJS=stepJS["recovery"] as JsonObject;
                    foreach (string role_id in recoveryJS.Keys)
                    {
                        int recovery=int.Parse(recoveryJS[role_id].ToString());
                        gameInfo.role_dic[role_id].blood_sugar += recovery;

                        DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        doActionAnimSignalParam.type = 4;
                        doActionAnimSignalParam.role_id = role_id;
                        doActionAnimSignalParam.value = recovery;
                        doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }
                    yield return new WaitForSeconds(step_time);
                    break;
                //更新猴子的与家距离
                case 6:
                    JsonObject distanceJS = stepJS["distance"] as JsonObject;
                    foreach (string building_id in distanceJS.Keys)
                    {
                        int distance=int.Parse(distanceJS[building_id].ToString());
                        BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
                        buildingInfo.distance_from_home = distance;

                    }
                    //yield return new WaitForSeconds(step_time);
                    break;
                //不造成角色、建筑、视野变化的指令
                case 7:
                    foreach (string role_id in (stepJS["action"] as JsonObject).Keys)
                    {
                        JsonObject turnActionJS = (stepJS["action"] as JsonObject)[role_id] as JsonObject;

                        int direction_did = int.Parse(turnActionJS["direction_did"].ToString());
                        JsonObject param = turnActionJS["param"] as JsonObject;
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];
//                        RoleActionList.TurnAction turnAction = roleActionList.turnActionDic[role_id];
                        switch (direction_did)
                        {
                            //采集
                            case 3:
                                int banana = int.Parse(param["cost"].ToString());
                                //gameInfo.map_info.resource[roleInfo.pos_id] = 2;

                                if (roleInfo.uid == sPlayerInfo.uid)
                                {
                                    gameInfo.allplayers_dic[roleInfo.uid].banana += banana;
//                                    doBananaUpdateSignal.Dispatch();

                                }
                                DoRoleActionAnimSignal.Param doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                                doRoleActionAnimSignalParam.type = 7;
                                doRoleActionAnimSignalParam.role_id = roleInfo.role_id;
                                doRoleActionAnimSignalParam.value = banana;

                                doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam);

                                //DoMapUpdateSignal.Param doMapUpdateSignalParam = new DoMapUpdateSignal.Param();
                                //doMapUpdateSignalParam.landformList = new Dictionary<int, int>();
                                //doMapUpdateSignalParam.resourceList = new Dictionary<int, int>();
                                //doMapUpdateSignalParam.resourceList.Add(roleInfo.pos_id, 2);
                                //doMapUpdateSignal.Dispatch(doMapUpdateSignalParam);



                                break;

                            //招募猴子
                            case 5:
                                int cost_5 = int.Parse(param["cost"].ToString());
                                if (roleInfo.uid == sPlayerInfo.uid)
                                {
                                    gameInfo.allplayers_dic[roleInfo.uid].banana -= cost_5;
//                                    doBananaUpdateSignal.Dispatch();
                                }
                                break;

                            //升级
                            case 7:
                                string building_id = param["cost"].ToString();
                                gameInfo.building_dic[building_id].level++;
                                break;

                            //搭窝
                            case 9:
                                int cost_9 = int.Parse(param["cost"].ToString());
                                if (roleInfo.uid == sPlayerInfo.uid)
                                {
                                    gameInfo.allplayers_dic[roleInfo.uid].banana -= cost_9;
                                    //                                    doBananaUpdateSignal.Dispatch();
                                }
                                break;
                            //哺育
                            case 11:
                                int damage_11 = int.Parse(param["damage"].ToString());
                                gameInfo.role_dic[role_id].blood_sugar -= damage_11;
                                DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                                doActionAnimSignalParam.type = 3;
                                doActionAnimSignalParam.role_id = role_id;
                                doActionAnimSignalParam.value = damage_11;
                                doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                                yield return new WaitForSeconds(step_time);
                                break;
                        }

                    }

                

                    
                    yield return new WaitForSeconds(step_time);
                    break;
                //吃料理
                case 8:
                    JsonObject foodJS = stepJS["food"] as JsonObject;
                    

                    foreach (string role_id in foodJS.Keys)
                    {
                        JsonObject param = (foodJS[role_id] as JsonObject)["param"] as JsonObject;
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];

                        DoRoleActionAnimSignal.Param doRoleActionAnimSignalParam_8 = new DoRoleActionAnimSignal.Param();
                        
                        doRoleActionAnimSignalParam_8.role_id = role_id;
                        

                        int blood_sugar = int.Parse(param["blood_sugar"].ToString());
                        roleInfo.blood_sugar_max += blood_sugar;
                        doRoleActionAnimSignalParam_8.type = 9;
                        doRoleActionAnimSignalParam_8.value = blood_sugar;
                        doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        yield return new WaitForSeconds(food_step_time);

                        int fat = int.Parse(param["fat"].ToString());
                        roleInfo.fat += fat;
                        doRoleActionAnimSignalParam_8.type = 10;
                        doRoleActionAnimSignalParam_8.value = blood_sugar;
                        doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        yield return new WaitForSeconds(food_step_time);

                        int inteligent = int.Parse(param["inteligent"].ToString());
                        roleInfo.inteligent += inteligent;
                        doRoleActionAnimSignalParam_8.type = 11;
                        doRoleActionAnimSignalParam_8.value = inteligent;
                        doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        yield return new WaitForSeconds(food_step_time);

                        int amino_acid = int.Parse(param["amino_acid"].ToString());
                        roleInfo.amino_acid += amino_acid;
                        doRoleActionAnimSignalParam_8.type = 12;
                        doRoleActionAnimSignalParam_8.value = amino_acid;
                        doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        yield return new WaitForSeconds(food_step_time);

                        int digest = int.Parse(param["digest"].ToString());
                        roleInfo.digest += digest;
                        doRoleActionAnimSignalParam_8.type = 13;
                        doRoleActionAnimSignalParam_8.value = digest;
                        doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        yield return new WaitForSeconds(food_step_time);

                        int skill_type = int.Parse(param["skill_type"].ToString());
                        int skill_id = int.Parse(param["skill_id"].ToString());
                        if (skill_type == 1)
                        {
                            roleInfo.skill_id_list.Add(skill_id);
                            doRoleActionAnimSignalParam_8.type = 14;
                            doRoleActionAnimSignalParam_8.value = skill_id;
                        }
                        else if (skill_type == 2)
                        {
                            roleInfo.cook_skill_id_list.Add(skill_id);
                            doRoleActionAnimSignalParam_8.type = 15;
                            doRoleActionAnimSignalParam_8.value = skill_id;
                            doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam_8);
                        }
                        yield return new WaitForSeconds(food_step_time);
                    }
                    break;
                //猴子收益
                case 9:
                    JsonObject bananaJS = stepJS["banana"] as JsonObject;
                    foreach (string building_id in bananaJS.Keys)
                    {
                        int value = int.Parse(bananaJS[building_id].ToString());
                        BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
                        gameInfo.allplayers_dic[buildingInfo.uid].banana += value;
                        DoBuildingActionAnimSignal.Param param=new DoBuildingActionAnimSignal.Param();
                        param.type=2;
                        param.building_id=building_id;
                        param.banana=value;
                        doBuildingActionAnimSignal.Dispatch(param);

                    }
                    yield return new WaitForSeconds(step_time);
                    break;
            }


            
            
        }

        //banana统一更新视图
        doBananaUpdateSignal.Dispatch();
        

        //while (playerStateChangeQueue.player_id_queue.Count > 0)
        //{
        //    int uid = playerStateChangeQueue.player_id_queue.Dequeue();
        //    int type = playerStateChangeQueue.change_type_queue.Dequeue();

        //    //switch()

        //    foreach (string role_id in gameInfo.role_dic.Keys)
        //    {
        //        RoleInfo roleInfo = gameInfo.role_dic[role_id];
        //        if (roleInfo.uid == uid)
        //        {
        //            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
        //            doActionAnimSignalParam.type = 2;
        //            doActionAnimSignalParam.role_id = roleInfo.role_id;
        //            doActionAnimSignal.Dispatch(doActionAnimSignalParam);
        //        }
        //    }

        //    foreach (string building_id in gameInfo.building_dic.Keys)
        //    {
        //        BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
        //        if (buildingInfo.uid == uid)
        //        {
        //            DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
        //            doBuildingActionAnimSignalParam.type = 1;
        //            doBuildingActionAnimSignalParam.building_id = buildingInfo.building_id;
        //            doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);
        //        }
        //    }
        //    userStateChangeSignal.Dispatch(uid,2);
        //    yield return new WaitForSeconds(step_time);
        //}

        ResetAllRoleDirection();
        //gameInfo.anim_lock--;
        actionAnimFinishSignal.Dispatch();

        checkUserStateQueueSignal.Dispatch();
        //Debug.Log("ActionAnimFinishSignal");
    }


    void DoSightModefiedAction(JsonObject jo)
    {
        JsonObject posJS=jo["pos"] as JsonObject;
        JsonObject addRolesJS=jo["add_roles"] as JsonObject;
        JsonArray deleteRolesJS=jo["delete_roles"] as JsonArray;
        JsonObject landformJS = jo["landform_map"] as JsonObject;
        JsonObject resourceJS = jo["resource_map"] as JsonObject;
        JsonObject meatJS = jo["meat_map"] as JsonObject;
        JsonObject addBuildingsJS=jo["add_buildings"] as JsonObject;
        JsonArray deleteBuildingsJS=jo["delete_buildings"] as JsonArray;

        foreach (string role_id in posJS.Keys)
        {
            int pos_id = int.Parse(posJS[role_id].ToString());
            gameInfo.role_dic[role_id].pos_id = pos_id;

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 0;
            doActionAnimSignalParam.role_id = role_id;
            doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
        }

        foreach (string role_id in addRolesJS.Keys)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.InitFromJson(addRolesJS[role_id] as JsonObject,gameInfo);

            gameInfo.role_dic.Add(role_id, roleInfo);

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 1;
            doActionAnimSignalParam.role_id = role_id;
            doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);

            GameObject roleobj = resourceService.Spawn("role/" + gameInfo.role_dic[role_id].role_did);
            roleobj.name = "role_" + role_id;
        }

        foreach (string role_id in deleteRolesJS)
        {
            

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 2;
            doActionAnimSignalParam.role_id = role_id;
            doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);

            gameInfo.role_dic.Remove(role_id);
        }





        foreach (string building_id in addBuildingsJS.Keys)
        {
            BuildingInfo buildingInfo = SimpleJson.SimpleJson.DeserializeObject<BuildingInfo>(addBuildingsJS[building_id].ToString());
            gameInfo.building_dic.Add(buildingInfo.building_id, buildingInfo);

            DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
            doBuildingActionAnimSignalParam.type = 0;
            doBuildingActionAnimSignalParam.building_id = buildingInfo.building_id;
            doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);

            GameObject buildingObj = resourceService.Spawn("building/" + buildingInfo.building_did);
            buildingObj.name = "building_" + buildingInfo.building_id;


            if (buildingInfo.building_did == 1)
            {

            }

        }

        foreach (string building_id in deleteBuildingsJS)
        {
            gameInfo.building_dic.Remove(building_id);

            DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
            doBuildingActionAnimSignalParam.building_id = building_id;
            doBuildingActionAnimSignalParam.type = 1;
            doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);


        }

        foreach (string pos_id_s in landformJS.Keys)
        {
            int pos_id = int.Parse(pos_id_s);
            int value = int.Parse(landformJS[pos_id_s].ToString());
            gameInfo.map_info.landform[pos_id] = value;
        }

        foreach (string pos_id_s in resourceJS.Keys)
        {
            int pos_id = int.Parse(pos_id_s);
            int value = int.Parse(resourceJS[pos_id_s].ToString());
            gameInfo.map_info.resource[pos_id] = value;
        }

        foreach (string pos_id_s in meatJS.Keys)
        {
            int pos_id = int.Parse(pos_id_s);
            int value = int.Parse(meatJS[pos_id_s].ToString());
            gameInfo.map_info.meat[pos_id] = value;
        }

        DoMapUpdateSignal.Param doMapUpdateSignalParam = new DoMapUpdateSignal.Param();
        doMapUpdateSignalParam.InitFromJson(landformJS, resourceJS,meatJS);
        doMapUpdateSignal.Dispatch(doMapUpdateSignalParam);

        doSightzoonUpdateSignal.Dispatch(doMapUpdateSignalParam.landformList);

    }
        
    //将所有role的指令设为防御
    void ResetAllRoleDirection()
    {
        foreach (string role_id in gameInfo.role_dic.Keys)
        {
            RoleInfo roleInfo=gameInfo.role_dic[role_id];
            roleInfo.direction_did = 2;
            roleInfo.direction_param.Clear();
        }
    }

    //void GetDistanceFromNearistHome(string building_id)
    //{
    //    BuildingInfo buildingInfo=gameInfo.building_dic[building_id];

    //}

    
//int get_distance(int pos_id_a,int pos_id_b)
//{
//    // console.log(pos_id_a==pos_id_b)
//    if(pos_id_a==pos_id_b)
//    {
//        return 0;
//    }
//    DGameType gametype=dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameInfo.gametype_id];
//    int distance=0;

//    int b_x=pos_id_b%gametype.width;
//    int b_y=pos_id_b/gametype.width;

//    int nearist_distance;
//    int nearist_pos_id=pos_id_a;

//    do{
//        var neibourids=getneibourids(gametype.width,gametype.height,nearist_pos_id);
//        // console.log(neibourids)
//        for(i in neibourids)
//        {
//            var neibour_pos_id=neibourids[i];
//            var neibour_x=neibour_pos_id%gametype.width;
//            var neibour_y=Math.floor(neibour_pos_id/gametype.width);

//            var temp_distance=Math.abs(b_x-neibour_x)+Math.abs(b_y-neibour_y);
//            // console.log(temp_distance)
//            if(nearist_distance==undefined)
//            {
//                nearist_distance=temp_distance;
//                nearist_pos_id=neibour_pos_id;
//            }
//            else if(temp_distance<nearist_distance)
//            {
//                nearist_distance=temp_distance;
//                nearist_pos_id=neibour_pos_id;
//            }
				

//        }
//        distance++;

//    }while(nearist_distance>0)
//    // console.log(distance)
//    return distance;

//}

//exports.get_nearist_home_distance=function(gameinfo,uid,pos_id)
//{
//    var distance=-1;
//    // console.log(gameinfo.buildings)
//    for(building_id in gameinfo.buildings)
//    {
//        // console.log('e')
//        var building=gameinfo.buildings[building_id];
//        // console.log(building)
//        if(building.building_did==1&&building.uid==uid)//树窝
//        {
//            var temp_distance=get_distance(gameinfo,pos_id,building.pos_id);
			
//            if(distance==-1)
//            {
//                distance=temp_distance;
//            }
//            else if(temp_distance<distance)
//            {
//                distance=temp_distance;
//            }
//        }
//    }

//    // console.log(distance)
//    return distance;
//}

}

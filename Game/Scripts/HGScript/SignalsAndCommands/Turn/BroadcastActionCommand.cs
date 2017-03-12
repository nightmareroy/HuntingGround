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
    public DoRoleActionAnimSignal doActionAnimSignal { get; set; }

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
    public PlayerFailQueue playerFailQueue{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}
    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}


    //回合动画时间
    float step_time=0.5f;


    public override void Execute()
    {
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
//        actionAnimStartSignal.Dispatch();

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

                    yield return new WaitForSeconds(step_time);
                    break;
                //攻击+掉血
                case 2:
                    JsonObject attackJS = stepJS["attack"] as JsonObject;
                    
                    foreach (string role_id in attackJS.Keys)
                    {
                        int type = int.Parse(attackJS["type"].ToString());
                        int enemy_id=int.Parse(attackJS["enemy_id"].ToString());

                        DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        switch (type)
                        {
                            case 1:
                            case 2:
                                doActionAnimSignalParam.type = 5;
                                doActionAnimSignalParam.role_id = role_id;
                                doActionAnimSignalParam.value = (float)enemy_id;
                                doActionAnimSignal.Dispatch(doActionAnimSignalParam);
                                break;
                            case 3:
                                
                                doActionAnimSignalParam.type = 6;
                                doActionAnimSignalParam.role_id = role_id;
                                doActionAnimSignal.Dispatch(doActionAnimSignalParam);
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
                        doActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }
                    //yield return new WaitForSeconds(step_time);
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
                        doActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }
                    yield return new WaitForSeconds(step_time);
                    break;
                //不造成角色、建筑、视野变化的指令
                case 6:
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
                                gameInfo.map_info.resource[roleInfo.pos_id] = 2;

                                if (roleInfo.uid == sPlayerInfo.uid)
                                {
                                    gameInfo.allplayers_dic[roleInfo.uid].banana += banana;
//                                    doBananaUpdateSignal.Dispatch();
                                }

                                DoMapUpdateSignal.Param doMapUpdateSignalParam = new DoMapUpdateSignal.Param();
                                doMapUpdateSignalParam.landformList = new Dictionary<int, int>();
                                doMapUpdateSignalParam.resourceList = new Dictionary<int, int>();
                                doMapUpdateSignalParam.resourceList.Add(roleInfo.pos_id, 2);
                                doMapUpdateSignal.Dispatch(doMapUpdateSignalParam);



                                break;

                            //招募猴子
                            case 5:
                                float cost_5 = float.Parse(param["cost"].ToString());
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
                            //进食
                            case 8:
                                break;
                            //搭窝
                            case 9:
                                float cost_9 = float.Parse(param["cost"].ToString());
                                if (roleInfo.uid == sPlayerInfo.uid)
                                {
                                    gameInfo.allplayers_dic[roleInfo.uid].banana -= cost_9;
                                    //                                    doBananaUpdateSignal.Dispatch();
                                }
                                break;
                            //抢夺
                            case 11:
                                
                                break;
                        }
                    }

                    //banana统一更新视图
                    doBananaUpdateSignal.Dispatch();
                    yield return new WaitForSeconds(step_time);
                    break;
            }



            
        }

        while (playerFailQueue.fail_id_queue.Count > 0)
        {
            int fail_player_id = playerFailQueue.fail_id_queue.Dequeue();

            foreach (string role_id in gameInfo.role_dic.Keys)
            {
                RoleInfo roleInfo = gameInfo.role_dic[role_id];
                if (roleInfo.uid == fail_player_id)
                {
                    DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                    doActionAnimSignalParam.type = 2;
                    doActionAnimSignalParam.role_id = roleInfo.role_id;
                    doActionAnimSignal.Dispatch(doActionAnimSignalParam);
                }
            }

            foreach (string building_id in gameInfo.building_dic.Keys)
            {
                BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
                if (buildingInfo.uid == fail_player_id)
                {
                    DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
                    doBuildingActionAnimSignalParam.type = 1;
                    doBuildingActionAnimSignalParam.building_id = buildingInfo.building_id;
                    doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);
                }
            }
            yield return new WaitForSeconds(step_time);
        }

        ResetAllRoleDirection();
        actionAnimFinishSignal.Dispatch();

        //Debug.Log("ActionAnimFinishSignal");
    }


    void DoSightModefiedAction(JsonObject jo)
    {
        JsonObject posJS=jo["pos"] as JsonObject;
        JsonObject addRolesJS=jo["add_roles"] as JsonObject;
        JsonArray deleteRolesJS=jo["delete_roles"] as JsonArray;
        JsonObject landformJS = jo["landform_map"] as JsonObject;
        JsonObject resourceJS = jo["resource_map"] as JsonObject;
        JsonObject addBuildingsJS=jo["add_buildings"] as JsonObject;
        JsonArray deleteBuildingsJS=jo["delete_buildings"] as JsonArray;

        foreach (string role_id in posJS.Keys)
        {
            int pos_id = int.Parse(posJS[role_id].ToString());
            gameInfo.role_dic[role_id].pos_id = pos_id;

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 0;
            doActionAnimSignalParam.role_id = role_id;
            doActionAnimSignal.Dispatch(doActionAnimSignalParam);
        }

        foreach (string role_id in addRolesJS.Keys)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.InitFromJson(addRolesJS[role_id] as JsonObject,gameInfo);

            gameInfo.role_dic.Add(role_id, roleInfo);

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 1;
            doActionAnimSignalParam.role_id = role_id;
            doActionAnimSignal.Dispatch(doActionAnimSignalParam);

            GameObject roleobj = resourceService.Spawn("role/" + gameInfo.role_dic[role_id].role_did);
            roleobj.name = "role_" + role_id;
        }

        foreach (string role_id in deleteRolesJS)
        {
            gameInfo.role_dic.Remove(role_id);

            DoRoleActionAnimSignal.Param doActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
            doActionAnimSignalParam.type = 2;
            doActionAnimSignalParam.role_id = role_id;
            doActionAnimSignal.Dispatch(doActionAnimSignalParam);
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

        DoMapUpdateSignal.Param doMapUpdateSignalParam = new DoMapUpdateSignal.Param();
        doMapUpdateSignalParam.InitFromJson(landformJS, resourceJS);
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
}

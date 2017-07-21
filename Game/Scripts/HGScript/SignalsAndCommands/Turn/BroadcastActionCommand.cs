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
    public JsonObject dataJO{ get; set;}

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
    public DoMoneyUpdateSignal doMoneyUpdateSignal{ get; set;}

    [Inject]
    public DoSightzoonUpdateSignal doSightzoonUpdateSignal{ get; set;}


    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public GameStateChangeQueue playerStateChangeQueue{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}
    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get;set; }

    [Inject]
    public CheckGameStateQueueSignal checkUserStateQueueSignal { get; set; }

    [Inject]
    public DoGroupGeneUpdateSignal doGroupGeneUpdateSignal { get; set; }

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal { get;set; }

    [Inject]
    public UpdateWeightsSignal updateWeightsSignal { get;set; }

    [Inject]
    public UpdateCurrentTurnSignal updateCurrentTurnSignal { get; set; }

    [Inject]
    public UpdateNextturnTimeSignal updateNextturnTimeSignal { get; set; }

    [Inject]
    public UpdateRoleFaceSignal updateRoleFaceSignal { get; set; }

    [Inject]
    public FindFreeRoleSignal findFreeRoleSignal { get; set; }


    //回合动画时间
    float step_time=0.5f;
    float food_step_time = 0.4f;


    public override void Execute()
    {
        JsonArray actionListJS = dataJO["action_list_dic"] as JsonArray;
        long nexttime = long.Parse(dataJO["nexttime"].ToString());
        gameInfo.nexttime = nexttime;
        updateNextturnTimeSignal.Dispatch();

        updateDirectionTurnSignal.Dispatch(-1);
        bootstrapView.StartCoroutine(updateDataAndBroadcaseAction(actionListJS));


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

    IEnumerator updateDataAndBroadcaseAction(JsonArray actionListJS)
    {
        actionAnimStartSignal.Dispatch();

        Debug.Log(111);



        for (int step = 0; step < actionListJS.Count; step++)
        {
            
            JsonObject stepJS = actionListJS[step] as JsonObject;
            Debug.Log(step);
            //Debug.Log(stepJS);
            switch (step)
            {
                case 0:
                    yield return null;
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
                //角色死亡，生成尸体
                case 3:
                //特殊指令中的角色、建筑、视野变化
                case 5:
                //角色死亡，生成尸体
                case 9:
                    
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
                        int pos_id = int.Parse( attackInfoJS["enemy_pos_id"].ToString());

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
                        doActionAnimSignalParam.type = 7;
                        doActionAnimSignalParam.role_id = role_id;
                        doActionAnimSignalParam.value = -1*damage;
                        doRoleActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }
                    yield return new WaitForSeconds(step_time);
                    break;

                //属性变化
                case 8:
                    JsonObject rolesJO = stepJS["roles"] as JsonObject;


                    List<DoRoleActionAnimSignal.Param> blood_sugarParamList = new List<DoRoleActionAnimSignal.Param>();
                    //List<DoRoleActionAnimSignal.Param> blood_sugar_maxParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> muscleParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> fatParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> inteligentParamList = new List<DoRoleActionAnimSignal.Param>();
                    //List<DoRoleActionAnimSignal.Param> amino_acidParamList = new List<DoRoleActionAnimSignal.Param>();
                    //List<DoRoleActionAnimSignal.Param> breathParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> digestParamList = new List<DoRoleActionAnimSignal.Param>();
                    //List<DoRoleActionAnimSignal.Param> courageParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> lifeParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> skillParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> directionParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> bananaParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> meatParamList = new List<DoRoleActionAnimSignal.Param>();
                    List<DoRoleActionAnimSignal.Param> branchParamList = new List<DoRoleActionAnimSignal.Param>();

                    foreach (string role_id in rolesJO.Keys)
                    {
                        JsonObject roleJO = rolesJO[role_id] as JsonObject;
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];

                        

                        DoRoleActionAnimSignal.Param doRoleActionAnimSignalParam;
                        
                        
                        

                        int blood_sugar = int.Parse(roleJO["blood_sugar"].ToString());
                        roleInfo.blood_sugar += blood_sugar;
                        if (blood_sugar != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 7;
                            doRoleActionAnimSignalParam.value = blood_sugar;
                            blood_sugarParamList.Add(doRoleActionAnimSignalParam);
                        }

                        //int blood_sugar_max = int.Parse(roleJO["blood_sugar_max"].ToString());
                        //roleInfo.blood_sugar_max += blood_sugar_max;
                        //if (blood_sugar_max != 0)
                        //{
                        //    doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        //    doRoleActionAnimSignalParam.role_id = role_id;
                        //    doRoleActionAnimSignalParam.type = 8;
                        //    doRoleActionAnimSignalParam.value = blood_sugar_max;
                        //    blood_sugar_maxParamList.Add(doRoleActionAnimSignalParam);
                        //}

                        int muscle = int.Parse(roleJO["muscle"].ToString());
                        roleInfo.muscle += muscle;
                        if (muscle != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 9;
                            doRoleActionAnimSignalParam.value = muscle;
                            muscleParamList.Add(doRoleActionAnimSignalParam);
                        }


                        int fat = int.Parse(roleJO["fat"].ToString());
                        roleInfo.fat += fat;
                        if (fat != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 10;
                            doRoleActionAnimSignalParam.value = fat;
                            fatParamList.Add(doRoleActionAnimSignalParam);
                        }


                        int inteligent = int.Parse(roleJO["inteligent"].ToString());
                        roleInfo.inteligent += inteligent;
                        if (inteligent != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 11;
                            doRoleActionAnimSignalParam.value = inteligent;
                            inteligentParamList.Add(doRoleActionAnimSignalParam);
                        }


                        //int amino_acid = int.Parse(roleJO["amino_acid"].ToString());
                        //roleInfo.amino_acid += amino_acid;
                        //if (amino_acid != 0)
                        //{
                        //    doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        //    doRoleActionAnimSignalParam.role_id = role_id;
                        //    doRoleActionAnimSignalParam.type = 12;
                        //    doRoleActionAnimSignalParam.value = amino_acid;
                        //    amino_acidParamList.Add(doRoleActionAnimSignalParam);
                        //}


                        //int breath = int.Parse(roleJO["breath"].ToString());
                        //roleInfo.breath += breath;
                        //if (breath != 0)
                        //{
                        //    doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        //    doRoleActionAnimSignalParam.role_id = role_id;
                        //    doRoleActionAnimSignalParam.type = 13;
                        //    doRoleActionAnimSignalParam.value = breath;
                        //    breathParamList.Add(doRoleActionAnimSignalParam);
                        //}

                        int digest = int.Parse(roleJO["digest"].ToString());
                        roleInfo.digest += digest;
                        if (digest != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 14;
                            doRoleActionAnimSignalParam.value = digest;
                            digestParamList.Add(doRoleActionAnimSignalParam);
                        }

                        //int courage = int.Parse(roleJO["courage"].ToString());
                        //roleInfo.courage += courage;
                        //if (courage != 0)
                        //{
                        //    doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        //    doRoleActionAnimSignalParam.role_id = role_id;
                        //    doRoleActionAnimSignalParam.type = 15;
                        //    doRoleActionAnimSignalParam.value = courage;
                        //    courageParamList.Add(doRoleActionAnimSignalParam);
                        //}

                        int old = int.Parse(roleJO["old"].ToString());
                        roleInfo.old += old;
                        if (old != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 16;
                            doRoleActionAnimSignalParam.value = old;
                            lifeParamList.Add(doRoleActionAnimSignalParam);
                        }
                        //doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        //doRoleActionAnimSignalParam.role_id = role_id;
                        //doRoleActionAnimSignalParam.type = 15;
                        //doRoleActionAnimSignalParam.value = courage;
                        //digestParamList.Add(doRoleActionAnimSignalParam);

                        JsonObject skillJO = roleJO["skill"] as JsonObject;
                        int skill_type = int.Parse(skillJO["type"].ToString());
                        int skill_id = int.Parse(skillJO["id"].ToString());
                        if (skill_type == 1)
                        {
                            roleInfo.skill_id_list.Add(skill_id);
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 17;
                            doRoleActionAnimSignalParam.value = skill_id;
                            skillParamList.Add(doRoleActionAnimSignalParam);
                        }
                        else if (skill_type == 2)
                        {
                            roleInfo.cook_skill_id_list.Add(skill_id);
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 18;
                            doRoleActionAnimSignalParam.value = skill_id;
                            skillParamList.Add(doRoleActionAnimSignalParam);

                        }

                        int direction_did = int.Parse(roleJO["direction_did"].ToString());
                        //List<int> direction_param = SimpleJson.SimpleJson.DeserializeObject<List<int>>(roleJO["direction_param"].ToString());
                        roleInfo.direction_did = direction_did;
                        roleInfo.direction_param.Clear();
                        doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                        doRoleActionAnimSignalParam.role_id = role_id;
                        doRoleActionAnimSignalParam.type = 19;
                        doRoleActionAnimSignalParam.value = direction_did;
                        directionParamList.Add(doRoleActionAnimSignalParam);
                        //updateRoleDirectionSignal.Dispatch(roleInfo.role_id);


                        int banana = int.Parse(roleJO["banana"].ToString());
                        //gameInfo.allplayers_dic[sPlayerInfo.uid].banana += banana;
                        if (banana != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 20;
                            doRoleActionAnimSignalParam.value = banana;
                            bananaParamList.Add(doRoleActionAnimSignalParam);
                        }

                        int meat = int.Parse(roleJO["meat"].ToString());
                        //gameInfo.allplayers_dic[sPlayerInfo.uid].meat += meat;
                        if (meat != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 21;
                            doRoleActionAnimSignalParam.value = meat;
                            meatParamList.Add(doRoleActionAnimSignalParam);
                        }

                        int branch = int.Parse(roleJO["branch"].ToString());
                        //gameInfo.allplayers_dic[sPlayerInfo.uid].branch += branch;
                        if (branch != 0)
                        {
                            doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.role_id = role_id;
                            doRoleActionAnimSignalParam.type = 22;
                            doRoleActionAnimSignalParam.value = branch;
                            branchParamList.Add(doRoleActionAnimSignalParam);
                        }

                    }

                    


                    foreach (DoRoleActionAnimSignal.Param param in blood_sugarParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    //foreach (DoRoleActionAnimSignal.Param param in blood_sugar_maxParamList)
                    //{
                    //    doRoleActionAnimSignal.Dispatch(param);
                    //}
                    //yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in muscleParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in fatParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in inteligentParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    //foreach (DoRoleActionAnimSignal.Param param in amino_acidParamList)
                    //{
                    //    doRoleActionAnimSignal.Dispatch(param);
                    //}
                    //yield return new WaitForSeconds(food_step_time);
                    //foreach (DoRoleActionAnimSignal.Param param in breathParamList)
                    //{
                    //    doRoleActionAnimSignal.Dispatch(param);
                    //}
                    //yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in digestParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    //foreach (DoRoleActionAnimSignal.Param param in courageParamList)
                    //{
                    //    doRoleActionAnimSignal.Dispatch(param);
                    //}
                    //yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in lifeParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in skillParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in bananaParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in meatParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in branchParamList)
                    {
                        doRoleActionAnimSignal.Dispatch(param);
                    }
                    yield return new WaitForSeconds(food_step_time);
                    foreach (DoRoleActionAnimSignal.Param param in directionParamList)
                    {
                        updateRoleDirectionSignal.Dispatch(param.role_id);
                        //doRoleActionAnimSignal.Dispatch(param);
                    }
                    break;
                //猴子收益
                //case 10:
                //    JsonObject bananaJS = stepJS["banana"] as JsonObject;
                //    foreach (string building_id in bananaJS.Keys)
                //    {
                //        int value = int.Parse(bananaJS[building_id].ToString());
                //        BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
                //        gameInfo.allplayers_dic[buildingInfo.uid].banana += value;
                //        DoBuildingActionAnimSignal.Param param=new DoBuildingActionAnimSignal.Param();
                //        param.type=2;
                //        param.building_id=building_id;
                //        param.banana=value;
                //        doBuildingActionAnimSignal.Dispatch(param);

                //    }
                //    yield return new WaitForSeconds(step_time);
                //    break;

                

                //同步金钱
                case 10:
                    JsonObject moneyJS = stepJS["money"] as JsonObject;

                    int banana_new = int.Parse(moneyJS["banana"].ToString());
                    int meat_new = int.Parse(moneyJS["meat"].ToString());
                    int branch_new = int.Parse(moneyJS["branch"].ToString());

                    JsonObject groupJO = moneyJS["group"] as JsonObject;

                    gameInfo.allplayers_dic[sPlayerInfo.uid].banana = banana_new;
                    gameInfo.allplayers_dic[sPlayerInfo.uid].meat = meat_new;
                    gameInfo.allplayers_dic[sPlayerInfo.uid].branch = branch_new;
                    foreach (string role_id in gameInfo.role_dic.Keys)
                    {
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];
                        roleInfo.temp_direction_banana = 0;
                        roleInfo.temp_direction_meat = 0;
                        roleInfo.temp_direction_branch = 0;

                    }

                    doGroupGeneUpdateSignal.Dispatch(groupJO);
                    doMoneyUpdateSignal.Dispatch();
                    //yield return new WaitForSeconds(step_time);
                    break;
                //更新重量
                case 11:
                    JsonObject weight_dicJS = stepJS["weight_dic"] as JsonObject;
                    updateWeightsSignal.Dispatch(weight_dicJS);
                    break;
            }


            
            
        }

        
        

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

        //ResetAllRoleDirection();
        //gameInfo.anim_lock--;
        updateRoleFaceSignal.Dispatch();

        gameInfo.current_turn++;
        updateCurrentTurnSignal.Dispatch();

        actionAnimFinishSignal.Dispatch();

        checkUserStateQueueSignal.Dispatch();
        //Debug.Log("ActionAnimFinishSignal");

        findFreeRoleSignal.Dispatch();
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

            updateRoleDirectionSignal.Dispatch(role_id);
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
            //Debug.Log(addBuildingsJS[building_id].ToString());
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

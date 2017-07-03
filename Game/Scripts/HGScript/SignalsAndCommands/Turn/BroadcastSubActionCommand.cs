using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;

public class BroadcastSubActionCommand:Command
{
    [Inject]
    public JsonObject actionDicJO { get; set; }

    [Inject]
    public BootstrapView bootstrapView { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public DoRoleActionAnimSignal doRoleActionAnimSignal { get; set; }

    [Inject]
    public DoBuildingActionAnimSignal doBuildingActionAnimSignal { get; set; }

    [Inject]
    public DoMapUpdateSignal doMapUpdateSignal { get; set; }

    [Inject]
    public DoMoneyUpdateSignal doMoneyUpdateSignal { get; set; }

    [Inject]
    public DoSightzoonUpdateSignal doSightzoonUpdateSignal { get; set; }

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal { get; set; }

    [Inject]
    public UpdateWeightsSignal updateWeightsSignal { get; set; }


    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public UpdateRoleFaceSignal updateRoleFaceSignal { get; set; }

    [Inject]
    public FindFreeRoleSignal findFreeRoleSignal { get; set; }


    //回合动画时间
    float food_step_time = 0.2f;

    public override void Execute()
    {
        bootstrapView.StartCoroutine(updateDataAndBroadcaseAction());
    }

    IEnumerator updateDataAndBroadcaseAction()
    {
        JsonObject modifiedDicJO = actionDicJO["modified_dic"] as JsonObject;
        JsonObject propertyDicJO = actionDicJO["property_dic"] as JsonObject;
        JsonObject weight_dicJS = actionDicJO["weight_dic"] as JsonObject;
        int builing_home_count = int.Parse(actionDicJO["builing_home_count"].ToString());
        updateWeightsSignal.Dispatch(weight_dicJS);

        DoSightModefiedAction(modifiedDicJO);

        List<DoRoleActionAnimSignal.Param> blood_sugarParamList = new List<DoRoleActionAnimSignal.Param>();
        //List<DoRoleActionAnimSignal.Param> blood_sugar_maxParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> muscleParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> fatParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> inteligentParamList = new List<DoRoleActionAnimSignal.Param>();
        //List<DoRoleActionAnimSignal.Param> amino_acidParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> breathParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> digestParamList = new List<DoRoleActionAnimSignal.Param>();
        //List<DoRoleActionAnimSignal.Param> courageParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> lifeParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> skillParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> directionParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> bananaParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> meatParamList = new List<DoRoleActionAnimSignal.Param>();
        List<DoRoleActionAnimSignal.Param> branchParamList = new List<DoRoleActionAnimSignal.Param>();

        foreach (string role_id in propertyDicJO.Keys)
        {
            JsonObject roleJO = propertyDicJO[role_id] as JsonObject;
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


            int breath = int.Parse(roleJO["breath"].ToString());
            roleInfo.breath += breath;
            if (breath != 0)
            {
                doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                doRoleActionAnimSignalParam.role_id = role_id;
                doRoleActionAnimSignalParam.type = 13;
                doRoleActionAnimSignalParam.value = breath;
                breathParamList.Add(doRoleActionAnimSignalParam);
            }

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

            if (roleJO.ContainsKey("direction_did"))
            {
                int direction_did = int.Parse(roleJO["direction_did"].ToString());
                //List<int> direction_param = SimpleJson.SimpleJson.DeserializeObject<List<int>>(roleJO["direction_param"].ToString());
                roleInfo.direction_did = direction_did;
                roleInfo.direction_param.Clear();
                doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                doRoleActionAnimSignalParam.role_id = role_id;
                doRoleActionAnimSignalParam.type = 19;
                doRoleActionAnimSignalParam.value = direction_did;
                directionParamList.Add(doRoleActionAnimSignalParam);
            }
            //updateRoleDirectionSignal.Dispatch(roleInfo.role_id);

            int banana = int.Parse(roleJO["banana"].ToString());
            gameInfo.allplayers_dic[sPlayerInfo.uid].banana += banana;
            if (banana != 0)
            {
                doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                doRoleActionAnimSignalParam.role_id = role_id;
                doRoleActionAnimSignalParam.type = 20;
                doRoleActionAnimSignalParam.value = banana;
                bananaParamList.Add(doRoleActionAnimSignalParam);
            }

            int meat = int.Parse(roleJO["meat"].ToString());
            gameInfo.allplayers_dic[sPlayerInfo.uid].meat += meat;
            if (meat != 0)
            {
                doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                doRoleActionAnimSignalParam.role_id = role_id;
                doRoleActionAnimSignalParam.type = 21;
                doRoleActionAnimSignalParam.value = meat;
                meatParamList.Add(doRoleActionAnimSignalParam);
            }

            int branch = int.Parse(roleJO["branch"].ToString());
            gameInfo.allplayers_dic[sPlayerInfo.uid].branch += branch;
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
        foreach (DoRoleActionAnimSignal.Param param in breathParamList)
        {
            doRoleActionAnimSignal.Dispatch(param);
        }
        yield return new WaitForSeconds(food_step_time);
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
        //yield return new WaitForSeconds(food_step_time);
        //foreach (DoRoleActionAnimSignal.Param param in directionParamList)
        //{
        //    updateRoleDirectionSignal.Dispatch(param.role_id);
        //}

        gameInfo.allplayers_dic[sPlayerInfo.uid].builing_home_count = builing_home_count;

        updateRoleFaceSignal.Dispatch();

        doMoneyUpdateSignal.Dispatch();

        //findFreeRoleSignal.Dispatch();

    }

    void DoSightModefiedAction(JsonObject jo)
    {
        JsonObject posJS = jo["pos"] as JsonObject;
        JsonObject addRolesJS = jo["add_roles"] as JsonObject;
        JsonArray deleteRolesJS = jo["delete_roles"] as JsonArray;
        JsonObject landformJS = jo["landform_map"] as JsonObject;
        JsonObject resourceJS = jo["resource_map"] as JsonObject;
        JsonObject meatJS = jo["meat_map"] as JsonObject;
        JsonObject addBuildingsJS = jo["add_buildings"] as JsonObject;
        JsonArray deleteBuildingsJS = jo["delete_buildings"] as JsonArray;
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
            roleInfo.InitFromJson(addRolesJS[role_id] as JsonObject, gameInfo);

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
        doMapUpdateSignalParam.InitFromJson(landformJS, resourceJS, meatJS);
        doMapUpdateSignal.Dispatch(doMapUpdateSignalParam);

        doSightzoonUpdateSignal.Dispatch(doMapUpdateSignalParam.landformList);

    }

}

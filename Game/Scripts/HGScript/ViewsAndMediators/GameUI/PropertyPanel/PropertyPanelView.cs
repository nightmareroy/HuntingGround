using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

public class PropertyPanelView :View
{
//    public Transform NodePropertyT;
//    public Transform RolePropertyT;
//    public Transform BuildingPropertyT;

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public ActiveGameDataService activeGameDataService{ get; set;}

    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}


    

    //node
    //public Transform nodePanelT;
    public Text landform;
    public Text resource;
    public Text meat;

    //role
    public Transform rolePanelT;

    public Text role_name;
    public Text blood_sugar;
    public Text muscle;
    public Text fat;
    public Text inteligent;
    public Text amino_acid;
    public Text breath;
    public Text digest;
    public Text courage;

    public Text health;
    public Text move;
    public Text weight;
    public Text basal_metabolism;
    public Text younger_left_max;
    public Text growup_left_max;
    public Text now_grow_state;
    

    public Transform roleSkillsRootT;
    public Transform roleSkillItemT;

    public Transform roleCookSkillsRootT;
    public Transform roleCookSkillItemT;


    //direction list
    public Transform roleDirections;
    public Transform roleDirectionBtnTpl;

    //building
    public Transform buildingPanelT;
    public Text building_name;
    public Text building_level;
    public Text building_dstance;




    public Action<int> onDirectionClick;
//    public Action<int> onBuildingDirectionClick;


    public void ClearNodePanel()
    {
        landform.text = "";
        resource.text = "";
        meat.text = "";
    }

    //public void ClearRolePanel()
    //{
    //    role_name.text = "";
    //    blood_sugar.text = "";
    //    muscle.text = "";
    //    fat.text = "";
    //    amino_acid.text = "";
    //    breath.text = "";
    //    digest.text = "";
    //    courage.text = "";
    //    health.text = "";
    //    move.text = "";
    //    weight.text = "";
    //}

    //public void ClearBuildingPanel()
    //{
    //    building_panel.gameObject.SetActive(false);
    //    building_name.text = "";
    //    building_level.text = "";
    //    building_dstance.text = "";
    //}

    public void SetRolePanelVisible(bool visible)
    {
        rolePanelT.gameObject.SetActive(visible);
    }

    public void SetBuildingPanelVisible(bool visible)
    {
        buildingPanelT.gameObject.SetActive(visible);
    }

    //public void SetNodePanelVisible(bool visible)
    //{
    //    nodePanelT.gameObject.SetActive(visible);
    //}

    public void SetRolePanelValue(RoleInfo roleInfo)
    {
        if (roleInfo == null)
        {
            //ClearRolePanel();
            //ClearRoleDirections();
            SetRolePanelVisible(false);
            return;
        }
        SetRolePanelVisible(true);
        role_name.text = roleInfo.name;//"猩猩";//roleInfo.role_id.ToString();
        blood_sugar.text = roleInfo.blood_sugar+"/"+roleInfo.blood_sugar_max;
        muscle.text = roleInfo.muscle.ToString();
        fat.text = roleInfo.fat.ToString();
        inteligent.text = roleInfo.inteligent.ToString();
        amino_acid.text = roleInfo.amino_acid.ToString();
        breath.text = roleInfo.breath.ToString();
        digest.text = roleInfo.digest.ToString();
        courage.text = roleInfo.courage.ToString();

        health.text = roleInfo.health.ToString();
        move.text = roleInfo.max_move.ToString();
        weight.text = roleInfo.weight.ToString();
        basal_metabolism.text = roleInfo.basal_metabolism.ToString();
        younger_left_max.text = ((float)roleInfo.younger_left_max / 100f).ToString();
        growup_left_max.text = ((float)roleInfo.growup_left_max / 100f).ToString();
        if (roleInfo.younger_left > 0)
        {
            now_grow_state.text = "幼年期(" + (float)roleInfo.younger_left / 100f + ")";
        }
        else if (roleInfo.growup_left > 0)
        {
            now_grow_state.text = "成长期(" + (float)roleInfo.growup_left / 100f + ")";
        }
        else
        {
            now_grow_state.text = "成熟期";
        }
        

        Tools.ClearChildren(roleSkillsRootT);
        foreach (int skill_id in roleInfo.skill_id_list)
        {
            DSkill dSkill = dGameDataCollection.dSkillCollection.dSkillDic[skill_id];

            GameObject skillItem = GameObject.Instantiate<GameObject>(roleSkillItemT.gameObject);

            skillItem.transform.SetParent(roleSkillsRootT);
            skillItem.transform.localPosition = Vector3.zero;
            skillItem.transform.localScale = Vector3.one;
            skillItem.transform.localRotation = Quaternion.identity;
            skillItem.SetActive(true);

            Text skillName = skillItem.transform.FindChild("SkillName").GetComponent<Text>();
            Text skillDesc = skillItem.transform.FindChild("SkillDesc").GetComponent<Text>();

            skillName.text = dSkill.name;
            skillDesc.text = dSkill.desc;
        }

        Tools.ClearChildren(roleCookSkillsRootT);
        foreach (int cook_skill_id in roleInfo.cook_skill_id_list)
        {
            DCookSkill dCookSkill = dGameDataCollection.dCookSkillCollection.dCookSkillDic[cook_skill_id];

            GameObject cookSkillItem = GameObject.Instantiate<GameObject>(roleCookSkillItemT.gameObject);

            cookSkillItem.transform.SetParent(roleCookSkillsRootT);
            cookSkillItem.transform.localPosition = Vector3.zero;
            cookSkillItem.transform.localScale = Vector3.one;
            cookSkillItem.transform.localRotation = Quaternion.identity;
            cookSkillItem.SetActive(true);

            Text skillName = cookSkillItem.transform.FindChild("SkillName").GetComponent<Text>();
            //Text skillDesc = cookSkillItem.transform.FindChild("SkillDesc").GetComponent<Text>();

            skillName.text = dCookSkill.name;
            //skillDesc.text = dCookSkill.desc;
        }
    }

    public void SetNodePanel(int pos_id)
    {
        landform.text = dGameDataCollection.dLandformCollection.dLandformDic[gameInfo.map_info.landform[pos_id]].desc;
        resource.text = dGameDataCollection.dResourceCollection.dResourceDic[gameInfo.map_info.resource[pos_id]].desc;

        int meat_id=gameInfo.map_info.meat[pos_id] / 100;
        int meat_left=gameInfo.map_info.meat[pos_id] % 100;
        //Debug.Log(gameInfo.map_info.meat[pos_id]);

        if (meat_id == 2)
        {
            meat.text = dGameDataCollection.dMeatCollection.dMeatDic[meat_id].desc + "(" + meat_left + ")";
        }
        else
        {
            meat.text = dGameDataCollection.dMeatCollection.dMeatDic[meat_id].desc;
        }

        //switch (gameInfo.map_info.landform[pos_id])
        //{
        //    case 0:
        //        landform.text = "未探索";
        //        break;
        //    case 1:
        //        landform.text = "平原";
        //        break;
        //    case 2:
        //        landform.text = "山地";
        //        break;
                
        //}

        //switch (gameInfo.map_info.resource[pos_id])
        //{
        //    case 0:
        //        resource.text = "未探索";
        //        break;
        //    case 1:
        //        resource.text = "无";
        //        break;
        //    case 2:
        //        resource.text = "香蕉树";
        //        break;
        //    case 3:
        //        resource.text = "香蕉";
        //        break;

        //}
    }

    public void SetRoleDirections(string role_id)
    {
        Tools.ClearChildren(roleDirections);

        List<int> allDirectionDids = activeGameDataService.GetAllDirectionDids(role_id);
        for (int i = 0; i < allDirectionDids.Count; i++)
        {
            int directionDid = allDirectionDids[i];
            DDirection dDirection = dGameDataCollection.dDirectionCollection.dDirectionDic[directionDid];
            GameObject btnObj = GameObject.Instantiate(roleDirectionBtnTpl.gameObject as UnityEngine.Object) as GameObject;

            btnObj.transform.SetParent(roleDirections);
            btnObj.transform.localScale = Vector3.one;
            btnObj.transform.localRotation = Quaternion.identity;
//            btnObj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            btnObj.transform.localPosition=Vector3.zero;
            btnObj.SetActive(true);
            btnObj.name = directionDid.ToString();
            btnObj.transform.FindChild("Text").GetComponent<Text>().text = dDirection.name;
            btnObj.GetComponent<Button>().onClick.AddListener(()=>{
                if(onDirectionClick!=null)
                    onDirectionClick(directionDid);
            });


        }
    }

    public void SetBuildingPanel(BuildingInfo buildingInfo)
    {
        if (buildingInfo == null)
        {
            //ClearBuildingPanel();
            SetBuildingPanelVisible(false);
            
        }
        else
        {
            SetBuildingPanelVisible(true);
            DBuilding dBuilding = dGameDataCollection.dBuildingCollection.dBuildingDic[buildingInfo.building_did];

            building_name.text = dBuilding.name;
            building_level.text = buildingInfo.level.ToString();
            building_dstance.text = buildingInfo.distance_from_home.ToString();
//            if (buildingInfo.building_direction_did != 0)
//            {
//                building_current_direction.text = dBuilding.name;
//            }
//            else
//            {
//                
//            }
//
//            ClearBuildingPanel();
//            List<int> buildingDirectionDids = activeGameDataService.GetAllBuildingDirectionDids(buildingInfo.building_id);
//            foreach (int buildingDirectionDid in buildingDirectionDids)
//            {
//                GameObject buildingDirectionBtnObj = GameObject.Instantiate(buildingDirectionBtnTpl.gameObject as UnityEngine.Object) as GameObject;
//                DBuildingDirection dBuildingDirection=dGameDataCollection.dBuildingDirectionCollection.dBuildingDirectionDic[buildingDirectionDid];
//                buildingDirectionBtnObj.transform.SetParent(buildingDirections);
//                buildingDirectionBtnObj.transform.localScale = Vector3.one;
//                buildingDirectionBtnObj.transform.localRotation = Quaternion.identity;
////                buildingDirectionBtnObj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
//                buildingDirectionBtnObj.transform.localPosition=Vector3.zero;
//                buildingDirectionBtnObj.SetActive(true);
//                buildingDirectionBtnObj.name = buildingDirectionDid.ToString();
//                buildingDirectionBtnObj.transform.FindChild("Text").GetComponent<Text>().text = dBuildingDirection.name;
//                buildingDirectionBtnObj.GetComponent<Button>().onClick.AddListener(()=>{
//                    if(onBuildingDirectionClick!=null)
//                        onBuildingDirectionClick(buildingDirectionDid);
//                });
//            }

        }




    }

    public void ClearRoleDirections()
    {
        Tools.ClearChildren(roleDirections);
    }
        
}

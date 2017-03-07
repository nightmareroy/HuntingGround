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
    public Text landform;
    public Text resource;

    //role
    public Text role_name;
    public Text blood_sugar;
    public Text muscle;
    public Text fat;
    public Text amino_acid;
    public Text breath;
    public Text digest;
    public Text courage;
    public Text basal_metabolism;
    public Text move;
    public Text weight;

    //direction list
    public Transform roleDirections;
    public Transform roleDirectionBtnTpl;

    //building
    public Text building_name;
    public Text building_level;




    public Action<int> onDirectionClick;
//    public Action<int> onBuildingDirectionClick;


    public void ClearNodePanel()
    {
        landform.text = "";
        resource.text = "";
    }

    public void ClearRolePanel()
    {
        role_name.text = "";
        blood_sugar.text = "";
        muscle.text = "";
        fat.text = "";
        amino_acid.text = "";
        breath.text = "";
        digest.text = "";
        courage.text = "";
        basal_metabolism.text = "";
        move.text = "";
        weight.text = "";
    }

    public void ClearBuildingPanel()
    {
        building_name.text = "";
        building_level.text = "";
    }

    public void SetRolePanel(RoleInfo roleInfo)
    {
        if (roleInfo == null)
        {
            ClearRolePanel();
            ClearRoleDirections();
            return;
        }
        role_name.text = roleInfo.name;//"猩猩";//roleInfo.role_id.ToString();
        blood_sugar.text = roleInfo.blood_sugar+"/"+roleInfo.blood_sugar_max;
        muscle.text = roleInfo.muscle.ToString();
        fat.text = roleInfo.fat.ToString();
        amino_acid.text = roleInfo.amino_acid.ToString();
        breath.text = roleInfo.breath.ToString();
        digest.text = roleInfo.digest.ToString();
        courage.text = roleInfo.courage.ToString();
        basal_metabolism.text = roleInfo.basal_metabolism.ToString();
        move.text = roleInfo.max_move.ToString();
        weight.text = roleInfo.weight.ToString();
    }

    public void SetNodePanel(int pos_id)
    {
        switch (gameInfo.map_info.landform[pos_id])
        {
            case 0:
                landform.text = "未探索";
                break;
            case 1:
                landform.text = "平原";
                break;
            case 2:
                landform.text = "山地";
                break;
                
        }

        switch (gameInfo.map_info.resource[pos_id])
        {
            case 0:
                resource.text = "未探索";
                break;
            case 1:
                resource.text = "无";
                break;
            case 2:
                resource.text = "香蕉树";
                break;
            case 3:
                resource.text = "香蕉";
                break;

        }
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
            ClearBuildingPanel();
        }
        else
        {
            DBuilding dBuilding = dGameDataCollection.dBuildingCollection.dBuildingDic[buildingInfo.building_did];

            building_name.text = dBuilding.name;
            building_level.text = buildingInfo.level.ToString();
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

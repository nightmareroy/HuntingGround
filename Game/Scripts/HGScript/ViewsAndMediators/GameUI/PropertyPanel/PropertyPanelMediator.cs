﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using MapNavKit;

public class PropertyPanelMediator : Mediator
{
    [Inject]
    public PropertyPanelView propertyPanelView{ get; set;}

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal{ get; set;}

    [Inject]
    public ActiveGameDataService activeGameDataService{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{get;set;}


    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal{get;set;}


    [Inject]
    public OpenFoodPanelSignal openFoodPanelSignal { get; set; }

    [Inject]
    public FindNodeSignal findNodeSignal { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }


    RoleInfo currentSelectedRole;
    BuildingInfo currentSelectedBuilding;


    public override void OnRegister()
    {
        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        propertyPanelView.ClearNodePanel();
        propertyPanelView.ClearRolePanel();
        propertyPanelView.ClearBuildingPanel();

        propertyPanelView.onDirectionClick += OnDirectionClick;
//        propertyPanelView.onBuildingDirectionClick += OnBuildingDirectionClick;

    }

    public void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
        if (mapNavNode == null)
        {
            propertyPanelView.ClearNodePanel();
            propertyPanelView.ClearRolePanel();
            propertyPanelView.ClearBuildingPanel();

            propertyPanelView.ClearRoleDirections();
            return;
        }


        //role
        currentSelectedRole = activeGameDataService.GetRoleInMap(mapNavNode.idx);
        propertyPanelView.SetRolePanel(currentSelectedRole);

        //node
        propertyPanelView.SetNodePanel(mapNavNode.idx);

        //role direction list
        if (currentSelectedRole != null)
        {
            if (currentSelectedRole.uid == sPlayerInfo.uid)
            {
                propertyPanelView.SetRoleDirections(currentSelectedRole.role_id);
            }
            else
            {
                propertyPanelView.ClearRoleDirections();
            }
        }

        //building
        currentSelectedBuilding=activeGameDataService.GetBuildingInMap(mapNavNode.idx);
        propertyPanelView.SetBuildingPanel(currentSelectedBuilding);

    }

    void OnDirectionClick(int direction_did)
    {
        if (direction_did == 8)
        {
            openFoodPanelSignal.Dispatch(currentSelectedRole.role_id);
        }
        else if (direction_did==1)
        {
            MapNavNode node = mainContext.mapRootMediator.mapRootView.NodeAt<MapNavNode>(currentSelectedRole.pos_id);
            findNodeSignal.Dispatch(node, true);
        }
        else
        {


            gameInfo.role_dic[currentSelectedRole.role_id].direction_did = direction_did;
            gameInfo.role_dic[currentSelectedRole.role_id].direction_param.Clear();
            mapNodeSelectSignal.Dispatch(null);

            updateRoleDirectionSignal.Dispatch(currentSelectedRole.role_id);
        }
    }

//    void OnBuildingDirectionClick(int building_direction_did)
//    {
//        gameInfo.building_dic[currentSelectedBuilding.building_id].building_direction_did = building_direction_did;
//
//        mapNodeSelectSignal.Dispatch(null);
//    }



    void OnDestroy()
    {
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
    }
}

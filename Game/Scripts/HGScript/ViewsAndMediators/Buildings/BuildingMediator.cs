using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using MapNavKit;

public class BuildingMediator:Mediator
{
    [Inject]
    public BuildingView buildingView{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public DoBuildingActionAnimSignal doBuildingActionAnimSignal{ get; set;}

    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    string building_id;

    BuildingUIView buildingUIView;

    MapRootMediator mapRootMediator;

    public override void OnRegister()
    {
        Debug.Log(buildingView.gameObject.name);
        string[] ids = buildingView.gameObject.name.Split('_');
        buildingView.Init();
        building_id = ids[1];


        buildingUIView = resourceService.Spawn("buildingui/buildingui").GetComponent<BuildingUIView>();
        buildingUIView.Init(gameObject, mainContext, doBuildingActionAnimSignal,building_id);

        mapRootMediator = mainContext.mapRootMediator;
        UpdateBuildingPos();
        doBuildingActionAnimSignal.AddListener(OnDoBuildingActionAnimSignal);
    }

    void UpdateBuildingPos()
    {

        if (mapRootMediator != null)
        {
            //gameObject.transform.position = mapRootMediator.mapRootView.NodeAt<MapNavNode>(roleInfo.pos_x, roleInfo.pos_y).position;
            gameObject.transform.position = mapRootMediator.mapRootView.NodeAt<MapNavNode>(gameInfo.building_dic[building_id].pos_id).position+new Vector3(0,5.8f,0);
        }
        else
            Debug.Log("mapRootMediator isn't exist!");

    }

    void OnDoBuildingActionAnimSignal(DoBuildingActionAnimSignal.Param param)
    {
        switch (param.type)
        {
            //出现
            case 0:
                
                break;
            //消失
            case 1:
                Destroy(gameObject);
                break;
        }
    }

    void OnDestroy()
    {
        doBuildingActionAnimSignal.RemoveListener(OnDoBuildingActionAnimSignal);
    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using MapNavKit;
using SimpleJson;

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

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public FindFreeRoleSignal findFreeRoleSignal { get; set; }

    [Inject]
    public FlowUpTipSignal flowUpTipSignal { get; set; }


    RoleInfo currentSelectedRole;
    BuildingInfo currentSelectedBuilding;


    public override void OnRegister()
    {
        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        propertyPanelView.ClearNodePanel();
        //propertyPanelView.ClearRolePanel();
        //propertyPanelView.ClearBuildingPanel();
        propertyPanelView.SetRolePanelVisible(false);
        propertyPanelView.SetBuildingPanelVisible(false);

        propertyPanelView.onDirectionClick += OnDirectionClick;
//        propertyPanelView.onBuildingDirectionClick += OnBuildingDirectionClick;

    }

    public void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
        if (mapNavNode == null)
        {
            propertyPanelView.ClearNodePanel();
            //propertyPanelView.ClearRolePanel();
            //propertyPanelView.ClearBuildingPanel();
            propertyPanelView.SetRolePanelVisible(false);
            propertyPanelView.SetBuildingPanelVisible(false);

            propertyPanelView.HideRoleDirections();
            return;
        }


        //role
        currentSelectedRole = activeGameDataService.GetRoleInMap(mapNavNode.idx);
        propertyPanelView.SetRolePanelValue(currentSelectedRole);

        //node
        propertyPanelView.SetNodePanel(mapNavNode.idx);

        //role direction list
        if (currentSelectedRole != null)
        {
            if (currentSelectedRole.uid == sPlayerInfo.uid)
            {
                if (currentSelectedRole.direction_did != 15)
                {
                    propertyPanelView.SetRoleDirections(currentSelectedRole.role_id);
                }
                else
                {
                    propertyPanelView.HideRoleDirections();
                }
            }
            else
            {
                propertyPanelView.HideRoleDirections();
            }
        }

        //building
        currentSelectedBuilding=activeGameDataService.GetBuildingInMap(mapNavNode.idx);
        propertyPanelView.SetBuildingPanel(currentSelectedBuilding);

    }

    void OnDirectionClick(int direction_did)
    {
        PlayerInfo playerInfo = gameInfo.allplayers_dic[currentSelectedRole.uid];
        //Debug.Log(direction_did);
        DDirection dDirection=dGameDataCollection.dDirectionCollection.dDirectionDic[direction_did];
        if (direction_did == 8)//料理
        {
            openFoodPanelSignal.Dispatch(currentSelectedRole.role_id);
        }
        else if (direction_did==1)//移动
        {
            MapNavNode node = mainContext.mapRootMediator.mapRootView.NodeAt<MapNavNode>(currentSelectedRole.pos_id);
            findNodeSignal.Dispatch(node, true);
        }
        
        else 
        {
            //异常处理
            if (dDirection.direction_did == 11)//哺育
            {
                if (currentSelectedRole.now_grow_state == 0)
                {
                    flowUpTipSignal.Dispatch(new FlowUpTipSignal.Param("幼年期猩猩不可繁育"));
                    return;
                }
                if (currentSelectedRole.health < 0.8)
                {
                    flowUpTipSignal.Dispatch(new FlowUpTipSignal.Param("猩猩的健康度不能小于80%"));
                    return;
                }
            }
            if (dDirection.direction_did == 9)//搭窝
            {

                DBuilding dBuilding = dGameDataCollection.dBuildingCollection.dBuildingDic[1];//树窝
                if (playerInfo.branch < dBuilding.cost_value * (playerInfo.builing_home_count + 1))
                {
                    flowUpTipSignal.Dispatch(new FlowUpTipSignal.Param("需要树枝：" + dBuilding.cost_value * (playerInfo.builing_home_count + 1)));
                    return;
                }
            }

            gameInfo.role_dic[currentSelectedRole.role_id].direction_did = direction_did;
            gameInfo.role_dic[currentSelectedRole.role_id].direction_param.Clear();
            
            //mapNodeSelectSignal.Dispatch(null);
            //Debug.Log(currentSelectedRole.role_id);
            updateRoleDirectionSignal.Dispatch(currentSelectedRole.role_id);

            //if (dDirection.delay == 0)//即时指令
            //{
                

            //    JsonObject form = new JsonObject();
            //    form.Add("direction_did", gameInfo.role_dic[currentSelectedRole.role_id].direction_did);
            //    form.Add("direction_param", gameInfo.role_dic[currentSelectedRole.role_id].direction_param);
            //    form.Add("role_id", currentSelectedRole.role_id);
            //    netService.Request(NetService.SubTurn, form, (msg) =>
            //    {

            //    });
            //}
            
            

            

            findFreeRoleSignal.Dispatch();

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

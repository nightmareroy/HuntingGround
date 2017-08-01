using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using SimpleJson;
using UnityEngine;

public class FoodPanelMediator:Mediator
{
    [Inject]
    public FoodPanelView foodPanelView { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public OpenFoodPanelSignal openFoodPanelSignal { get; set; }

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal{get;set;}

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal{get;set;}

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public FindFreeRoleSignal findFreeRoleSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public DoMoneyUpdateSignal doMoneyUpdateSignal { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    RoleInfo roleInfo;

    public override void OnRegister()
    {
        //Debug.Log("add");
        openFoodPanelSignal.AddListener(OnOpenFoodPanelSignal);
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);

        foodPanelView.onConfirm += OnConfirm;

    }


    void OnOpenFoodPanelSignal(string role_id)
    {
        roleInfo = gameInfo.role_dic[role_id];
        PlayerInfo playerInfo=gameInfo.allplayers_dic[roleInfo.uid];
        foodPanelView.Init(role_id, roleInfo.cook_skill_id_list, playerInfo.actived_food_ids);
    }

    void OnConfirm(string role_id,int food_id)
    {
        gameInfo.role_dic[role_id].direction_did = 8;//吃料理指令id为8
        gameInfo.role_dic[role_id].direction_param.Clear();
        gameInfo.role_dic[role_id].direction_param.Add(food_id);
        mapNodeSelectSignal.Dispatch(null);

        updateRoleDirectionSignal.Dispatch(role_id);

        List<int> paramList = new List<int>();
        paramList.Add(food_id);
        JsonObject form = new JsonObject();
        form.Add("direction_did", 8);
        form.Add("direction_param", paramList);
        form.Add("role_id", roleInfo.role_id);
        netService.Request(NetService.SubTurn, form, (msg) =>
        {
            //findFreeRoleSignal.Dispatch();
        });
        //mapNodeSelectSignal.Dispatch(null);

        //DFood dFood = dGameDataCollection.dFoodCollection.dFoodDic[food_id];

        //gameInfo.role_dic[role_id].temp_direction_banana = -dFood.banana;
        //gameInfo.role_dic[role_id].temp_direction_meat = -dFood.meat;
        //doMoneyUpdateSignal.Dispatch();

        //roleInfo.direction_did = 8;
        //roleInfo.direction_param.Clear();
        //roleInfo.direction_param.Add(food_id);
        //updateRoleDirectionSignal.Dispatch(roleInfo.role_id);

        findFreeRoleSignal.Dispatch();
    }

    void OnActionAnimStartSignal()
    {
        foodPanelView.SetPanelVisible(false);
    }

    void OnDestroy()
    {
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        openFoodPanelSignal.RemoveListener(OnOpenFoodPanelSignal);
    }
}

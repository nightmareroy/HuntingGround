using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;

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

    public override void OnRegister()
    {
        openFoodPanelSignal.AddListener(OnOpenFoodPanelSignal);

        foodPanelView.onConfirm += OnConfirm;
    }

    void OnOpenFoodPanelSignal(string role_id)
    {
        RoleInfo roleInfo = gameInfo.role_dic[role_id];
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
    }

    void OnDestroy()
    {
        openFoodPanelSignal.RemoveListener(OnOpenFoodPanelSignal);
    }
}

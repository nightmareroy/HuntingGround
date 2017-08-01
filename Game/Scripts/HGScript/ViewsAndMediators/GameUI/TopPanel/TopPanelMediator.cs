using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using SimpleJson;

public class TopPanelMediator:Mediator
{
    [Inject]
    public TopPanelView topPannelView{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public DoMoneyUpdateSignal doMoneyUpdateSignal{ get; set;}

    [Inject]
    public DoGroupGeneUpdateSignal doGroupGeneUpdateSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public UpdateCurrentTurnSignal updateCurrentTurnSignal { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection  { get; set; }

    [Inject]
    public ActiveGameDataService activeGameDataService { get; set; }

    public override void OnRegister()
    {
        doMoneyUpdateSignal.AddListener(OnDoMoneyUpdateSignal);
        doGroupGeneUpdateSignal.AddListener(OnDoGroupGeneUpdateSignal);
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
        updateCurrentTurnSignal.AddListener(OnUpdateCurrentTurn);
        OnDoMoneyUpdateSignal();
        topPannelView.UpdateGroup(gameInfo.allplayers_dic[sPlayerInfo.uid].groupInfoJO);
        topPannelView.UpdateCurrentTurn(gameInfo.current_turn);


        if (gameInfo.gametype_id == 1)
        {
            topPannelView.UpdateWinCondition(dGameDataCollection.dSingleGameInfoCollection.dSingleGameInfoDic[gameInfo.progress_id].win_condition);
        }
        else
        {
            topPannelView.UpdateWinCondition(dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameInfo.gametype_id].win_condition);
        }

    }

    void OnDoMoneyUpdateSignal()
    {
        ActiveGameDataService.OnShowMoney onShowMoney = activeGameDataService.GetOnShowMoney();
        topPannelView.UpdateMoney(onShowMoney.meat, onShowMoney.banana,onShowMoney.ant, onShowMoney.egg,onShowMoney.honey);
    }

    void OnDoGroupGeneUpdateSignal(JsonObject groupJO)
    {
        topPannelView.UpdateGroup(groupJO);
    }

    void OnActionAnimStartSignal()
    {
        topPannelView.toggle.isOn = false;
        topPannelView.toggle.interactable = false;
    }

    void OnActionAnimFinishSignal()
    {
        topPannelView.toggle.interactable = true;
    }

    void OnUpdateCurrentTurn()
    {
        topPannelView.UpdateCurrentTurn(gameInfo.current_turn);
    }


    void OnDestroy()
    {
        doMoneyUpdateSignal.RemoveListener(OnDoMoneyUpdateSignal);
        doGroupGeneUpdateSignal.RemoveListener(OnDoGroupGeneUpdateSignal);
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
        updateCurrentTurnSignal.RemoveListener(OnUpdateCurrentTurn);
    }
}


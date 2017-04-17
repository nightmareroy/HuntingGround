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
    public DoMoneyUpdateSignal doBananaUpdateSignal{ get; set;}

    [Inject]
    public DoGroupGeneUpdateSignal doGroupGeneUpdateSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    public override void OnRegister()
    {
        doBananaUpdateSignal.AddListener(OnDoBananaUpdateSignal);
        doGroupGeneUpdateSignal.AddListener(OnDoGroupGeneUpdateSignal);
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
        topPannelView.UpdateBanana(gameInfo.allplayers_dic[sPlayerInfo.uid]);
        topPannelView.UpdateGroup(gameInfo.allplayers_dic[sPlayerInfo.uid].groupInfoJO);

    }

    void OnDoBananaUpdateSignal()
    {
        topPannelView.UpdateBanana(gameInfo.allplayers_dic[sPlayerInfo.uid]);
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

    void OnDestroy()
    {
        doBananaUpdateSignal.RemoveListener(OnDoBananaUpdateSignal);
        doGroupGeneUpdateSignal.RemoveListener(OnDoGroupGeneUpdateSignal);
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
    }
}


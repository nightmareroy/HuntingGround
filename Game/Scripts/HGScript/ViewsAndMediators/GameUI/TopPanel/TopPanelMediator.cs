using System;
using UnityEngine;
using strange.extensions.mediation.impl;

public class TopPanelMediator:Mediator
{
    [Inject]
    public TopPanelView topPannelView{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public DoBananaUpdateSignal doBananaUpdateSignal{ get; set;}

    public override void OnRegister()
    {
        doBananaUpdateSignal.AddListener(OnDoBananaUpdateSignal);
        topPannelView.UpdateBanana(gameInfo.allplayers_dic[sPlayerInfo.uid].banana);
    }

    void OnDoBananaUpdateSignal()
    {
        topPannelView.UpdateBanana(gameInfo.allplayers_dic[sPlayerInfo.uid].banana);
    }

    void OnDestroy()
    {
        doBananaUpdateSignal.RemoveListener(OnDoBananaUpdateSignal);
    }
}


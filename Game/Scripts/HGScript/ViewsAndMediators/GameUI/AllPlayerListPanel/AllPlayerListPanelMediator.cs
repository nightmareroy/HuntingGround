using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using SimpleJson;

public class AllPlayerListPanelMediator : Mediator
{
    [Inject]
    public AllPlayerListPanelView allPlayerListPanelView{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get; set; }

    [Inject]
    public UserStateChangeSignal userStateChangeSignal { get; set; }

    [Inject]
    public UpdateWeightsSignal updateWeightsSignal { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }


    public override void OnRegister()
    {
        allPlayerListPanelView.UpdatePlayers(gameInfo.allplayers_dic[sPlayerInfo.uid].weight_dicJO);
        //Debug.Log(gameInfo.allplayers_dic[sPlayerInfo.uid].weight_dicJO.ToString());
        allPlayerListPanelView.UpdateWeights(gameInfo.allplayers_dic[sPlayerInfo.uid].weight_dicJO);
        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);
        userStateChangeSignal.AddListener(OnUserStateChangeSignal);
        updateWeightsSignal.AddListener(OnUpdateWeightsSignal);
    }

    void OnUpdateDirectionTurnSignal(int uid)
    {
        //Debug.Log(uid);
        if (uid == -1)
        {
            allPlayerListPanelView.ResetAllPlayerReady();
        }
        else
        {
            allPlayerListPanelView.SetPlayerReady(uid, true);
        }
    }

    void OnUserStateChangeSignal(int uid,int type)
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                allPlayerListPanelView.RemovePlayer(uid);
                break;
        }
    }

    void OnUpdateWeightsSignal(JsonObject weight_dic)
    {
        allPlayerListPanelView.UpdateWeights(weight_dic);
    }

    void OnDestroy()
    {
        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
        userStateChangeSignal.RemoveListener(OnUserStateChangeSignal);
        updateWeightsSignal.RemoveListener(OnUpdateWeightsSignal);
    }
}

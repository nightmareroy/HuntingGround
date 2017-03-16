using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

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




    public override void OnRegister()
    {
        allPlayerListPanelView.UpdatePlayers();
        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);
        userStateChangeSignal.AddListener(OnUserStateChangeSignal);
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

    void OnDestroy()
    {
        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
        userStateChangeSignal.RemoveListener(OnUserStateChangeSignal);
    }
}

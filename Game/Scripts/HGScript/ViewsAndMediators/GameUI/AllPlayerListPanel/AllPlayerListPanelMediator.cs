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




    public override void OnRegister()
    {
        allPlayerListPanelView.UpdatePlayers();
        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);
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

    void OnDestroy()
    {
        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
    }
}

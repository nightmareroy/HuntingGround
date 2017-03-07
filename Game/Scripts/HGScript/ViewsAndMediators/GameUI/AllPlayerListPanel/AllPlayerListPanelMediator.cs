using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class AllPlayerListPanelMediator : Mediator
{
    [Inject]
    public AllPlayerListPanelView allPlayerListPanelView{ get; set;}

    [Inject]
    public GameInfo gameInfo{ get; set;}




    public override void OnRegister()
    {
        allPlayerListPanelView.UpdatePlayers();
    }
}

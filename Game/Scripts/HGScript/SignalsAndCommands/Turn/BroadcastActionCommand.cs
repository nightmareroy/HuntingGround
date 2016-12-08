using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;

public class BroadcastActionCommand:Command
{
    [Inject]
    public JsonArray actionList { get; set; }

    [Inject]
    public BootstrapView bootstrapView { get; set; }

    [Inject]
    public DoActionAnimSignal doActionAnimSignal { get; set; }

    [Inject]
    public DoMapUpdateSignal doMapUpdateSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    RoleAction roleAction;

    public override void Execute()
    {
        Debug.Log(actionList.ToString());
        roleAction = new RoleAction();
        roleAction.InitFromJson(actionList);
        bootstrapView.StartCoroutine(updateDataAndBroadcaseAction());
    }

    IEnumerator updateDataAndBroadcaseAction()
    {
        actionAnimStartSignal.Dispatch();

        gameInfo.current_turn++;

        for (int step = 0; step < 7; step++)
        {
            switch (step)
            {
                case 0:

                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    foreach(int roleid in roleAction.moveList[step].Keys)
                    {
                        gameInfo.role_dic[roleid].pos_id = roleAction.moveList[step][roleid];

                        DoActionAnimSignal.Param doActionAnimSignalParam = new DoActionAnimSignal.Param();
                        doActionAnimSignalParam.type = 0;
                        doActionAnimSignalParam.roleid = roleid;
                        //doActionAnimSignalParam.pos_id = gameInfo.role_dic[roleid].pos_id;
                        doActionAnimSignal.Dispatch(doActionAnimSignalParam);
                    }

                    DoMapUpdateSignal.Param doMapUpdateSignalParam = new DoMapUpdateSignal.Param();
                    doMapUpdateSignalParam.landformList = roleAction.landformList[step];
                    doMapUpdateSignalParam.resourceList = roleAction.resourceList[step];
                    doMapUpdateSignal.Dispatch(doMapUpdateSignalParam);
                    Debug.Log("map dispatch!");
                    
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }

            
            yield return new WaitForSeconds(1);
            
        }
        actionAnimFinishSignal.Dispatch();

        //Debug.Log("ActionAnimFinishSignal");
    }
}


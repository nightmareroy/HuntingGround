using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;
using UnityEngine;

public class CheckUserStateQueueCommand:Command
{
    [Inject]
    public UserStateChangeSignal userStateChangeSignal { get;set; }

    [Inject]
    public PlayerStateChangeQueue playerStateChangeQueue { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public DoRoleActionAnimSignal doRoleActionAnimSignal { get; set; }

    [Inject]
    public DoBuildingActionAnimSignal doBuildingActionAnimSignal { get; set; }

    //[Inject]
    //public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    //[Inject]
    //public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public BootstrapView bootstrapView { get; set; }




    //bool isAnimActing = false;

    public override void Execute()
    {

        while (playerStateChangeQueue.player_id_queue.Count > 0)
        {
            int uid = playerStateChangeQueue.player_id_queue.Dequeue();
            int type = playerStateChangeQueue.change_type_queue.Dequeue();

            userStateChangeSignal.Dispatch(uid, type);

            switch (type)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    foreach (string role_id in gameInfo.role_dic.Keys)
                    {
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];
                        if (roleInfo.uid == uid)
                        {
                            DoRoleActionAnimSignal.Param doRoleActionAnimSignalParam = new DoRoleActionAnimSignal.Param();
                            doRoleActionAnimSignalParam.type = 2;
                            doRoleActionAnimSignalParam.role_id = roleInfo.role_id;
                            doRoleActionAnimSignal.Dispatch(doRoleActionAnimSignalParam);
                        }
                    }

                    foreach (string building_id in gameInfo.building_dic.Keys)
                    {
                        BuildingInfo buildingInfo = gameInfo.building_dic[building_id];
                        if (buildingInfo.uid == uid)
                        {
                            DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
                            doBuildingActionAnimSignalParam.type = 1;
                            doBuildingActionAnimSignalParam.building_id = buildingInfo.building_id;
                            doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);
                        }
                    }
                    
                    break;
            }

            


        }
        
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;

public class CheckGameStateQueueCommand:Command
{
    [Inject]
    public UserStateChangeSignal userStateChangeSignal { get;set; }

    [Inject]
    public GameStateChangeQueue playerStateChangeQueue { get; set; }

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

    [Inject]
    public GameoverSignal gameoverSignal { get;set; }




    //bool isAnimActing = false;

    public override void Execute()
    {

        while (playerStateChangeQueue.change_type_queue.Count > 0)
        {
            //int uid = playerStateChangeQueue.player_id_queue.Dequeue();
            int type = playerStateChangeQueue.change_type_queue.Dequeue();
            JsonObject data = playerStateChangeQueue.change_data_queue.Dequeue();


            switch (type)
            {
                case 0:
                    int uid_0 = int.Parse(data["uid"].ToString());
                    userStateChangeSignal.Dispatch(uid_0, type);
                    break;
                case 1:
                    int uid_1 = int.Parse(data["uid"].ToString());
                    
                    foreach (string role_id in gameInfo.role_dic.Keys)
                    {
                        RoleInfo roleInfo = gameInfo.role_dic[role_id];
                        if (roleInfo.uid == uid_1)
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
                        if (buildingInfo.uid == uid_1)
                        {
                            DoBuildingActionAnimSignal.Param doBuildingActionAnimSignalParam = new DoBuildingActionAnimSignal.Param();
                            doBuildingActionAnimSignalParam.type = 1;
                            doBuildingActionAnimSignalParam.building_id = buildingInfo.building_id;
                            doBuildingActionAnimSignal.Dispatch(doBuildingActionAnimSignalParam);
                        }
                    }
                    userStateChangeSignal.Dispatch(uid_1, type);
                    break;
                case 2:
                    gameoverSignal.Dispatch(data);
                    break;
            }

            


        }
        
    }


}

using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class UpdateDirectionPathCommand:Command
{
    [Inject]
    public UpdateDirectionPathSignal.Param param { get; set; }

    [Inject]
    public UpdateDirectionPathCallbackSignal updateDirectionPathCallbackSignal { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    public override void Execute()
    {
        //if (param.path == null)
        //    gameInfo.curr_direction_dic[param.roleid].path.Clear();

        if (gameInfo.role_dic[param.roleid].uid != sPlayerInfo.uid)
        {
            return;
        }

        gameInfo.role_dic[param.roleid].direction_path = param.path;
        //gameInfo.curr_direction_dic[param.roleid].path = param.path;
        //Debug.Log(param.roleid);
        updateDirectionPathCallbackSignal.Dispatch(param);
    }
}


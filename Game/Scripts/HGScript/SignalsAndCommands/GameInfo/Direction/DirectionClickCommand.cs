using System;
using System.Collections.Generic;
using strange.extensions.command.impl;


public class DirectionClickCommand:Command
{
    [Inject]
    public DirectionClickSignal.Param param { get;set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public DirectionClickCallbackSignal directionClickCallbackSignal { get; set; }

    public override void Execute()
    {
        //if (!gameInfo.allplayers_dic[sPlayerInfo.uid].role_dic.ContainsKey(param.roleid))
        //{
        //    gameInfo.allplayers_dic[sPlayerInfo.uid].role_dic.Add();
        //}

        //if (!gameInfo.curr_direction_dic.ContainsKey(param.roleid))
        //{
        //    gameInfo.curr_direction_dic.Add(param.roleid, new DirectionInfo(param.roleid));
        //}

        if (gameInfo.role_dic[param.roleid].uid != sPlayerInfo.uid)
        {
            return;
        }

        //if (gameInfo.role_dic[param.roleid].directionInfo == null)
        //{
        //    gameInfo.role_dic[param.roleid].directionInfo = new DirectionInfo();
        //}
        gameInfo.role_dic[param.roleid].direction_id = param.directionid;
        gameInfo.role_dic[param.roleid].direction_path.Clear();


        //gameInfo.curr_direction_dic[param.roleid].direction_id = param.directionid;
        //gameInfo.curr_direction_dic[param.roleid].path.Clear();
        directionClickCallbackSignal.Dispatch(param);
    }
}


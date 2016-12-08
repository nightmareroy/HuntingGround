using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;


public class AddPlayerCommand:Command
{
    [Inject]
    public AddPlayerSignal.Param param { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public AddPlayerCallbackSignal addPlayerCallbackSignal { get; set; }

    public override void Execute()
    {
        gameInfo.allplayers_dic.Add(param.playerInfo.uid, param.playerInfo);


        addPlayerCallbackSignal.Dispatch();
        //if(param.dataCallback!=null)
        //    param.dataCallback();
    }
}


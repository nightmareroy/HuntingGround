using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;


public class TestCommand:Command
{

    [Inject]
    public AddPlayerSignal addGamePlayerSignal { get; set; }

    [Inject]
    public AddPlayerCallbackSignal addGamePlayerCallbackSignal { get; set; }


    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    

    public override void Execute()
    {
        



        AddPlayerSignal.Param addPlayerParam = new AddPlayerSignal.Param(new PlayerInfo(-1,true,0));

        addGamePlayerCallbackSignal.AddOnce(() => 
        {



            Debug.Log(JsonUtility.ToJson(gameInfo));
        });

        addGamePlayerSignal.Dispatch(addPlayerParam);


        //Debug.Log(JsonUtility.ToJson(gameInfo));

        

        
    }
}


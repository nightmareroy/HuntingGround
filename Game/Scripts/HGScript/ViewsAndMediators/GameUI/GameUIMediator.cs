using System;
using UnityEngine;
using strange.extensions.mediation.impl;

public class GameUIMediator:Mediator
{
    [Inject]
    public GameUIView gameUIView{ get; set;}

    [Inject]
    public NetService netService{ get; set;}

    [Inject]
    public EndGameSignal endGameSignal{ get; set;}



    public override void OnRegister()
    {
        gameUIView.viewClick += OnBtnClick;
    }

    public void OnBtnClick(string btnName)
    {
        Debug.Log(btnName);
        switch (btnName)
        {
//            case "NextTurn":
//                
//
//                break;
//            case "Fail":
//                netService.Request(NetService.fail,null,(msg)=>{
////                    Debug.Log(msg.rawString);
//                    if(msg.code==200)
//                    {
//                        endGameSignal.Dispatch();
//                    }
//                });
//                break;
        }
    }
}


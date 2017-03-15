using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

public class TestBtnMediator:Mediator
{
    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public TestBtnView testBtnView { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    public override void OnRegister()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("test","test");
        testBtnView.callback = () =>
        {
            Vector3 pos = transform.position;
            Hashtable args = new Hashtable();
            args.Add("time", 3f);
            args.Add("x", pos.x+0.1f);
            args.Add("y", pos.y);
            args.Add("z", pos.z);
            args.Add("loopType", iTween.LoopType.none);
            args.Add("easyType", iTween.EaseType.punch);
            iTween.MoveTo(gameObject, args);
        };
    }
}


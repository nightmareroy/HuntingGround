using System;
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
            GameObject.Find("role_5_1").GetComponent<RoleView>().DoWalking();
        };
    }
}


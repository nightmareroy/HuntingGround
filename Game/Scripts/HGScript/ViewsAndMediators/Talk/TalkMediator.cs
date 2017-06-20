using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class TalkMediator:Mediator
{
    [Inject]
    public TalkView talkView { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    //[Inject]
    //public MainContext mainContext { get; set; }

    public override void OnRegister()
    {
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
    }

    void OnActionAnimStartSignal()
    {
        
        talkView.SetHide();
    }


    void OnActionAnimFinishSignal()
    {
        talkView.SetVisible();
    }

    void OnDestroy()
    {
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
    }
}

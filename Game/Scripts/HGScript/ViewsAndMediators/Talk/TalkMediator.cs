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

    [Inject]
    public TalkShowSignal talkShowSignal { get; set; }

    //[Inject]
    //public TalkHideSignal talkHideSignal { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    //[Inject]
    //public MainContext mainContext { get; set; }

    int current_sub_id = 0;
    List<string> content_list;

    public override void OnRegister()
    {
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
        talkShowSignal.AddListener(OnTalkShowSignal);
        //talkHideSignal.AddListener(OnTalkHideSignal);

        if (gameInfo.gametype_id == 1)
        {
            content_list = dGameDataCollection.dStoryTalkCollection.dStoryTalkDic[gameInfo.progress_id].content_list;

            talkShowSignal.Dispatch();
        }
    }

    void OnActionAnimStartSignal()
    {
        
        //talkView.SetHide();
    }


    void OnActionAnimFinishSignal()
    {
        //talkView.SetVisible();
    }

    void OnTalkShowSignal()
    {
        
        if (content_list.Count > current_sub_id)
        {
            string content = content_list[current_sub_id];

            talkView.SetVisible(content);
            current_sub_id++;
        }
        else
        {
            talkView.SetHide();
        }
        
    }

    //void OnTalkHideSignal()
    //{
    //    talkView.SetHide();
    //}

    void OnDestroy()
    {
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
        talkShowSignal.RemoveListener(OnTalkShowSignal);
    }
}

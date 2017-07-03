using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class TalkView:View
{
    [Inject]
    public TalkShowSignal talkShowSignal { get; set; }

    public GameObject talkObj;

    public Animator animatorCtrl;

    public GameObject myRoleListPanelContent;

    public Text text;

    public void Init()
    {
        
    }

    public void SetVisible(string content)
    {
        text.text = content;

        //animatorCtrl.SetBool("visible",true);
        talkObj.SetActive(true);

    }

    public void SetHide()
    {
       
        //animatorCtrl.SetBool("visible", false);
        talkObj.SetActive(false);
    }

    public void OnClick()
    {
        talkShowSignal.Dispatch();
    }
}

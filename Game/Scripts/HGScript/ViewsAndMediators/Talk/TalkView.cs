using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class TalkView:View
{
    public GameObject talkObj;

    public Animator animatorCtrl;

    public GameObject myRoleListPanelContent;

    public void Init()
    {
        
    }

    public void SetVisible()
    {
        

        animatorCtrl.SetBool("visible",true);

    }

    public void SetHide()
    {
       
        animatorCtrl.SetBool("visible", false);
    }
}

using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

public class NextturnView:View
{
    public Text nextText;

    public Action nextturndelegate;
    public void onNextturnClick()
    {
        if (nextturndelegate != null)
            nextturndelegate();
    }

    public void update_time(int nextturn_time)
    {
        
        nextText.text = "下一回合(" + nextturn_time + ")";
    }

    public void setBtnInteractable(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;
    }
}


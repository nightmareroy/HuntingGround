using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

public class NextturnView:View
{
    public Action nextturndelegate;
    public void onNextturnClick()
    {
        if (nextturndelegate != null)
            nextturndelegate();
    }

    public void setBtnInteractable(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;
    }
}


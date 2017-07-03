using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView:View
{
//    public Button nextturnBtn;
//    public Button failBtn;

    public Action<string> viewClick;

    public GameUIView()
    {
    }

    public void OnNextturnClick()
    {
        if (viewClick != null)
        {
            viewClick("NextTurn");
        }

    }

    //public void OnFailClick()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("Fail");
    //    }
    //}
}


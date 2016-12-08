using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;


public class TestBtnView:View
{
    public Action callback;

    public void onclick()
    {
        if (callback != null)
            callback();
    }
}


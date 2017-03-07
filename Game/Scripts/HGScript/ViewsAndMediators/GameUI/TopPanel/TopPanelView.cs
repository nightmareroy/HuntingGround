using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;

public class TopPanelView:View
{
    public Text bananaValue;


    public void UpdateBanana(float banana)
    {
        bananaValue.text = banana.ToString();
    }
}


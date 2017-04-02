using System;
using UnityEngine;
using strange.extensions.signal.impl;

public class DoBuildingActionAnimSignal:Signal<DoBuildingActionAnimSignal.Param>
{
    public class Param
    {
        //0:出现 1:消失  2:产出香蕉
        public int type;

        public string building_id;

        public int banana;
    }
}


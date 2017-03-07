using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using SimpleJson;

public class DoRoleActionAnimSignal:Signal<DoRoleActionAnimSignal.Param>
{
    public class Param
    {
        //0:移动  1:出现 2:消失 3:掉血 4：回血
        public int type;

        public string role_id;

        public float value = 0;
    }
}


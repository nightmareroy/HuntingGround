using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using SimpleJson;

public class DoActionAnimSignal:Signal<DoActionAnimSignal.Param>
{
    public class Param
    {
        //0:移动  1:出现 2:消失
        public int type;

        public int roleid;

        ////如果type为0，则pos_id为-1
        //public int pos_id;
    }
}


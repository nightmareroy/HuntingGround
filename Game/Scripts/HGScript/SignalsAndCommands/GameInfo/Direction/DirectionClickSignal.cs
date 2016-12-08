using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;


public class DirectionClickSignal:Signal<DirectionClickSignal.Param>
{
    public class Param
    {
        public int roleid;
        public int directionid;

        public Param(int roleid,int directionid)
        {
            this.roleid = roleid;
            this.directionid = directionid;
        }
    }
}


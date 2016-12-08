using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using MapNavKit;

/// <summary>
/// 第三个参数为directionid
/// </summary>
public class UpdateDirectionPathSignal : Signal<UpdateDirectionPathSignal.Param>
{
    public class Param
    {
        public int roleid;
        public List<int> path;

        public Param(int roleid,List<int> path)
        {
            this.roleid = roleid;
            this.path = path;
        }
    }
}


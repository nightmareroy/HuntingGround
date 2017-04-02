using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using SimpleJson;

public class DoRoleActionAnimSignal:Signal<DoRoleActionAnimSignal.Param>
{
    public class Param
    {
        //0:移动  1:出现 2:消失 3:掉血 4:回血 5:攻击 6:转圈  7:采集香蕉   8:采集肉  9:增加血量和血量上限 10:增加脂肪 11:增加智商 12:增加氨基酸 13:增加消化 13:增加战斗技能 14:增加料理机能
        public int type;

        public string role_id;

        public int value = 0;
    }
}


using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using SimpleJson;

public class DoRoleActionAnimSignal:Signal<DoRoleActionAnimSignal.Param>
{
    public class Param
    {
        //0:移动  1:出现 2:消失 3:掉血 4:回血 5:攻击 6:转圈  7:血量   8:血量上限  9:肌肉 10:脂肪 11:智商 12:氨基酸 13:呼吸 14:消化 15:勇气 16寿命 17:增加战斗技能 18:增加料理技能 19:指令did 20:香蕉  21:肉  22:树枝
        public int type;

        public string role_id;

        public int value = 0;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.signal.impl;

public class UserStateChangeSignal:Signal<int,int>//参数为uid  changeType(0：加入频道 1：离开频道 )
{
}

using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class DoMapUpdateSignal:Signal<DoMapUpdateSignal.Param>
{
    public class Param
    {
        public Dictionary<int, int> landformList;
        public Dictionary<int, int> resourceList;
    }
}

using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class ConnectSignal : Signal<ConnectSignal.Param>
{
    public class Param
    {
        public NetService.ServerConnector server;

        public Action<bool> callback;
    }
    
}


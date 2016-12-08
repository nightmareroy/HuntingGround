using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class GetServerListSignal:Signal<Action<List<NetService.ServerConnector>>>
{
    public Action<List<NetService.ServerConnector>> callback;
}


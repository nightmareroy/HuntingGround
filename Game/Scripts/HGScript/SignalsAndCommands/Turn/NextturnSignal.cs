using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class NextturnSignal : Signal<Action<bool>>
{
    public Action<bool> callback;
}


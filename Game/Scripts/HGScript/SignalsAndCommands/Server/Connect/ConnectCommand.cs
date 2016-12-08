using System;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class ConnectCommand:Command
{
    [Inject]
    public ConnectSignal connectSignal { get;set; }

    [Inject]
    public NetService netService { get; set; }

    public override void Execute()
    {
        //base.Execute();
        //netService.Connect();

    }
}


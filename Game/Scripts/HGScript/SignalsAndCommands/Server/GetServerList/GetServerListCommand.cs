using System;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class GetServerListCommand:Command
{
    [Inject]
    public GetServerListSignal getServerListSignal { get; set; }

    [Inject]
    public NetService netService { get; set; }

    public override void Execute()
    {
        //base.Execute();
        //netService.GetServerList((serverList) => {
        //    getServerListSignal.callback(serverList);
        //});
    }
}


using System;
using System.Collections.Generic;
using strange.extensions.command.impl;


public class EndGameCommand : Command
{
    [Inject]
    public AsyncSceneService asyncSceneService { get; set; }


    public override void Execute()
    {
        asyncSceneService.LoadScene("Login", () =>
            {
            }
        );
    }

}


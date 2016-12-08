using System;
using System.Collections.Generic;
using strange.extensions.command.impl;


public class EndGameCommand : Command
{
    public override void Execute()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

}


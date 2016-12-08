using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class StartAppCommand : Command {

    public override void Execute()
    {
        //不能调用父类的execute..
        //base.Execute();
        //Debug.Log("start command!");
        //Application.LoadLevel("Login");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }
}

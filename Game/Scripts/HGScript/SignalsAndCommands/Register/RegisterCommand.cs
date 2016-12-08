using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;


public class RegisterCommand:Command
{
    [Inject]
    public RegisterSignal.Param param { get;set; }

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public RegisterCallbackCommand registerCallbackCommand { get; set; }

    public override void Execute()
    {

        //WWWForm form = new WWWForm();
        //form.AddField("account", param.account);
        //form.AddField("password", param.pwd);
        //form.AddField("password2", loginView.Register_Pwd2.text);
        JsonObject form = new JsonObject();
        form["account"] = param.account;
        form["pwd"] = param.pwd;
        //form.AddField("hash", "hashcode");
        netService.Request(netService.registerRoute,form, (msg) =>
        {
            //Debug.Log(msg.rawString);
            registerCallbackCommand.Dispatch();
        });
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;
using UnityEngine;

public class MsgBoxCommand:Command
{
    [Inject]
    public string content { get; set; }

    [Inject]
    public Action callback { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public IconSpritesService iconSpritesService { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }

    GameObject boxObj;

    public override void Execute()
    {
        boxObj = resourceService.Spawn("msg_box/msg_box");
        //if (param.parent == null)
        //{

        //    param.parent = mainContext.uiCanvas.transform;

        //}
        boxObj.transform.SetParent(mainContext.uiCanvas.transform.FindChild("MsgBoxRoot"));
        boxObj.transform.localPosition = Vector3.zero;
        boxObj.transform.localScale = Vector3.one;
        boxObj.transform.localRotation = Quaternion.identity;
        boxObj.GetComponent<MsgBoxView>().Init(content,callback);
    }
}

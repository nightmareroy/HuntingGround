using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using strange.extensions.command.impl;


public class FlowUpTipCommand:Command
{
    [Inject]
    public FlowUpTipSignal.Param param { get;set; }

    [Inject]
    public ResourceService resourceService { get;set; }

    [Inject]
    public IconSpritesService iconSpritesService { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }


    GameObject tipObj;

    public override void Execute()
    {
        tipObj = resourceService.Spawn("flowuptip/FlowUpTip");
        tipObj.GetComponent<FlowUpView>().Init(param, iconSpritesService, resourceService, dGameDataCollection);
    }
}

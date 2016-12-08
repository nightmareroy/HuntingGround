using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using System;

public class BootstrapView : ContextView {

    public Action onUpdate;
	void Start()
	{
        DontDestroyOnLoad(this.gameObject);
        context = new MainContext(this);//,ContextStartupFlags.MANUAL_MAPPING);
        //signalContext.Start();
	}

    void Update()
    {
        if (onUpdate != null)
            onUpdate();
    }
	
}

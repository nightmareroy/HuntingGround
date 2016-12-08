using System;
using System.Collections.Generic;
using UnityEngine;

public class NetForm
{
    public static string sessionid;

    public WWWForm wwwform;

    public NetForm(WWWForm wwwform)
    {
        this.wwwform = wwwform;
        this.wwwform.AddField("sessionid",NetForm.sessionid);
    }
}


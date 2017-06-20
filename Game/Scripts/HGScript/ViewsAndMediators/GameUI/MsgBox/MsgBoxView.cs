using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MsgBoxView:MonoBehaviour
{
    string content;

    Action callback;

    public Text text;


    public void Init(string content,Action callback)
    {
        this.content = content;
        this.callback = callback;

        text.text = content;
    }

    public void OnConfirm()
    {
        //if (callback != null)
        callback();
        Destroy(gameObject);
    }
}

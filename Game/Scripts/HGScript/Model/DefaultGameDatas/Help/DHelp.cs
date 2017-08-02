using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;
using UnityEngine;

public class DHelp
{
    public int id;
    public string title;
    public string content;
}

public class DHelpCollection
{
    public Dictionary<int, DHelp> dHelpDic = new Dictionary<int, DHelp>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        foreach (string key in jsobj.Keys)
        {
            //int id = int.Parse(key);
            JsonObject ja = jsobj[key] as JsonObject;

            DHelp dHelp = SimpleJson.SimpleJson.DeserializeObject<DHelp>(ja.ToString());

            dHelpDic.Add(dHelp.id, dHelp);

        }
    }
}
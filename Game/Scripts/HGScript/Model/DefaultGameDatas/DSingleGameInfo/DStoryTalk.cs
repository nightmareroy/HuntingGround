using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;
using UnityEngine;

public class DStoryTalk
{
    public int process_id;
    public List<string> content_list;
}

public class DStoryTalkCollection
{
    public Dictionary<int, DStoryTalk> dStoryTalkDic = new Dictionary<int, DStoryTalk>();

    public void InitFromStr(string jsstr)
    {
        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        foreach (string key in jsobj.Keys)
        {
            int process_id = int.Parse(key);
            JsonArray ja = jsobj[key] as JsonArray;

            DStoryTalk dStoryTalk = new DStoryTalk();
            dStoryTalk.process_id = process_id;
            dStoryTalk.content_list = SimpleJson.SimpleJson.DeserializeObject<List<string>>(ja.ToString());
            dStoryTalkDic.Add(process_id,dStoryTalk);

        }
    }
}
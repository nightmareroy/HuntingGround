using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using SimpleJson;

public class DoMapUpdateSignal:Signal<DoMapUpdateSignal.Param>
{
    public class Param
    {
        public Dictionary<int, int> landformList;
        public Dictionary<int, int> resourceList;
        public Dictionary<int, int> meatList;

        public void InitFromJson(JsonObject landformJs,JsonObject resourceJs,JsonObject meatJs)
        {
            landformList = new Dictionary<int,int>();
            resourceList = new Dictionary<int, int>();
            meatList = new Dictionary<int, int>();
            foreach (string key in landformJs.Keys)
            {
                int pos_id = int.Parse(key);
                int value = int.Parse(landformJs[key].ToString());
                landformList.Add(pos_id,value);
            }

            foreach (string key in resourceJs.Keys)
            {
                int pos_id = int.Parse(key);
                int value = int.Parse(resourceJs[key].ToString());
                resourceList.Add(pos_id,value);
            }

            foreach (string key in meatJs.Keys)
            {
                int pos_id = int.Parse(key);
                int value = int.Parse(meatJs[key].ToString());
                meatList.Add(pos_id, value);
            }
        }
    }
}

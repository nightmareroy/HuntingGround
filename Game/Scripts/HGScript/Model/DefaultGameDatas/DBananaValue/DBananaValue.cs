using System;
using System.Collections.Generic;
using SimpleJson;

public class DBananaValue
{
    public int monkey_lv;
    public int value;
}

public class DBananaValueCollection
{
    public Dictionary<int, DBananaValue> dBananaValueDic = new Dictionary<int, DBananaValue>();

    public void InitFromStr(string jsstr)
    {

        JsonObject jsobj = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsstr);
        dBananaValueDic.Clear();
        foreach (JsonObject jo in jsobj.Values)
        {

            DBananaValue dBananaValue = SimpleJson.SimpleJson.DeserializeObject<DBananaValue>(jo.ToString());
            dBananaValueDic.Add(dBananaValue.monkey_lv,dBananaValue);
        }
    }
}
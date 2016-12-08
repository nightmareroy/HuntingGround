using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;
using Pomelo.DotNetClient;

[Serializable]
//public class NetData// : ISerializationCallbackReceiver
//{
//    public int status;
//    public string data;
//    public string msg;
//    public string sessionid;


//    //public void OnAfterDeserialize()
//    //{
//    //    if (data != null)
//    //    {
//    //        Debug.Log("before:" + data);
//    //        //data = data.Replace("\\", "");
//    //        Debug.Log("after" + data);
//    //    }
//    //}

//    //public void OnBeforeSerialize()
//    //{
//    //}
//}

public class NetData// : ISerializationCallbackReceiver
{
    public int code;
    public object data=null;

    //public NetData(Message message)
    //{
    //    message.
    //    if (!jo.ContainsKey("code"))
    //    {
    //        Debug.LogError("net return dosn't have the key 'code'!");
    //        return;
    //    }
    //    code = int.Parse(jo["code"].ToString());

    //    if (jo.ContainsKey("data"))
    //        data = jo["data"];

    //    if (code != 200)
    //    {
    //        Debug.LogError(jo.ToString());
    //    }

    //}


    //public string allReturnWhenError;
    //public string msg;
    //public string sessionid;
    //public NetData(JsonObject jo)
    //{
    //    if (!jo.ContainsKey("code"))
    //    {
    //        Debug.LogError("net return dosn't have the key 'code'!");
    //        return;
    //    }
    //    code = int.Parse(jo["code"].ToString());

    //    if (jo.ContainsKey("data"))
    //        data = jo["data"];

    //    if(code!=200)
    //    {
    //        Debug.LogError(jo.ToString());
    //    }

    //}



    //public void OnAfterDeserialize()
    //{
    //    if (data != null)
    //    {
    //        Debug.Log("before:" + data);
    //        //data = data.Replace("\\", "");
    //        Debug.Log("after" + data);
    //    }
    //}

    //public void OnBeforeSerialize()
    //{
    //}
}
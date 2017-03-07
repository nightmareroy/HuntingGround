using System;
using SimpleJson;
using UnityEngine;
using System.Reflection;
using strange.extensions.signal.impl;

namespace Pomelo.DotNetClient
{
    public class Message
    {
        public MessageType type;
        public string route;
        public uint id;
        public JsonObject jsonObj;
        public string rawString;
        public int code=-1;
        public object data = null;

        public Message(MessageType type, string route, JsonObject data)
        {
            this.type = type;
            this.route = route;
            this.jsonObj = data;
            GenCodeAndData();
        }

        public Message(MessageType type, uint id)
        {
            this.type = type;
            this.id = id;
            this.route = "";
            this.jsonObj = null;
        }

        public Message(MessageType type, uint id, string route, JsonObject data, string rawString)
        {
            this.type = type;
            this.id = id;
            this.route = route;
            this.jsonObj = data;
            this.rawString = rawString;
            GenCodeAndData();
        }

        void GenCodeAndData()
        {
            //try
            //{
            //    Debug.Log(jsonObj);
            //}
            //catch (Exception e)
            //{
            //    Debug.Log(e.ToString());
            //}
            
            if (jsonObj != null)
            {
                if (jsonObj.ContainsKey("code"))
                {
                    code = int.Parse(jsonObj["code"].ToString());
                }
                else
                {
                    Debug.LogError(jsonObj.ToString());
                    Debug.LogError("net return dosn't have the key 'code'!");
                }
                //Debug.Log("kkk" + jsonObj.ToString());
                if (jsonObj.ContainsKey("data"))
                {
                    data = jsonObj["data"] as object;
                    //Debug.Log("kkk" + data);
                }
//                if (type == MessageType.MSG_PUSH)
//                {
//                    Assembly assembly = Assembly.GetExecutingAssembly();
//                    Signal signal= assembly.CreateInstance(route + "PushSignal") as Signal;
//                    signal.Dispatch();
//                    Debug.Log("signal dispatched!!!");
//
//                }
            }
            
        }
    }
}
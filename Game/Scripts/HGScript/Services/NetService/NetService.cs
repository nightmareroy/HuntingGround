using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pomelo.DotNetClient;
using SimpleJson;


public class NetService
{
    public class ServerConnector
    {
        public string name;
        public string host;
        public int port;
        public int state;
    }

    [Inject]
    public BootstrapView bootstrapView { get; set; }

    [Inject]
    public LoadingSignal loadingSignal { get; set; }

    [Inject]
    public BroadcastActionSignal broadcastActionSignal { get; set; }


    //sessionid
    public string sessionid;

    //服务器ip
    string host_gate = "127.0.0.1";//"127.0.0.1:8088/huntingground/";
    int port_gate = 3014;

    Connection pclient = new Connection();

    //test
    public string test = "test/test.php";

    ///user
    //
    public string gateRoute = "gate.gateHandler.queryEntry";

    public string connectRoute = "connector.entryHandler.connect";


    //account,password
    public string loginRoute = "connector.entryHandler.login";
    
    //account,password
    public string registerRoute = "connector.entryHandler.register";

    ///game
    //
    public string isingame = "game.gameHandler.isingame";
    //
    public string nextturn = "game.gameHandler.nextturn";
    //
    public string createnewgame = "game.gameHandler.createnewgame";
    //
    public string startgame = "game.gameHandler.startgame";
    //
    public string loadgame = "game.gameHandler.loadgame";
    //
    public string updategameinfo = "game.gameHandler.updategameinfo";
    //
    public string getgameinfo = "game.gameHandler.getgameinfo";

    //
    public string doAction = "doAction";





    

    public NetService()
    {
        
        
        
        //Connect(host_gate, port_gate, (netData) =>
        //{
        //    Debug.Log(netData.ToString());
        //});
        
        //NetTest();
        

        //string host = "127.0.0.1";//(www.xxx.com/127.0.0.1/::1/localhost etc.)
        //int port = 3014;
        //PomeloClient pclient = new PomeloClient(host, port);

        //pclient.on("onChat", (jsobj) =>
        //{
        //    Debug.Log("chatmsg" + jsobj.ToString());
        //});

        //try
        //{
        //    Debug.Log("1");
        //    JsonObject user = new JsonObject();
        //    pclient.connect(user,(jsobj) =>
        //    {
        //        Debug.Log("3");
        //        Debug.Log("connect" + jsobj.Count);
        //        JsonObject msg = new JsonObject();
        //        msg["uid"] = "ly";
        //        pclient.request("gate.gateHandler.queryEntry", msg, (jsobj2) =>
        //        {

        //            Debug.Log("gate:" + jsobj2.ToString());

        //            PomeloClient pclient2 = new PomeloClient(host, int.Parse(jsobj2["port"].ToString()));
        //            Debug.Log("reconnect!");
        //            pclient2.connect((jsobj3) =>
        //            {

        //                JsonObject userMessage = new JsonObject();
        //                userMessage.Add("username", "ly");
        //                userMessage.Add("rid", 0);
        //                pclient2.request("connector.entryHandler.enter", userMessage, (data) =>
        //                {
        //                    Debug.Log("connector:" + data.ToString());

        //                    JsonObject message = new JsonObject();
        //                    message.Add("rid", 1);
        //                    message.Add("content", "asdfasdf");
        //                    message.Add("from", "ly");
        //                    message.Add("target", "*");
        //                    pclient2.request("chat.chatHandler.send", message, (data2) =>
        //                    {
        //                        Debug.Log("chat:" + data2.ToString());
        //                    });
        //                });
        //            });

        //        });

        //    });
        //    Debug.Log("2");
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("connect error" + e.ToString());
        //}

        //Debug.Log("init pomelo");
    }

    public void Init()
    {
        bootstrapView.onUpdate += () =>
        {
            try
            {
                pclient.Update();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        };

        pclient.on(Connection.DisconnectEvent, msg =>
        {
            Debug.logger.Log("Network error, reason: " + msg.jsonObj["reason"]);
        });

        pclient.on(Connection.ErrorEvent, msg =>
        {
            Debug.logger.Log("Error, reason: " + msg.jsonObj["reason"]);
        });

        pclient.on(doAction, (msg) => {
            Debug.Log((msg.data as JsonArray).ToString());
            broadcastActionSignal.Dispatch(msg.data as JsonArray);
            
        });

        //GetServerList((serverlist) => { Debug.Log("333"); });
    }


    public void GetServerConnector(Action<ServerConnector> callback)
    {

        //Connection pc = new Connection();
        //pclient.InitClient("127.0.0.1", 3014, (message) => { Debug.Log("0"); });
        //bindUpdate();
        try
        {
            loadingSignal.Dispatch(true);
            pclient.InitClient(host_gate, port_gate, (message) =>
            {
                Debug.Log("init callback");
                try
                {
                    
                    pclient.connect(null, (jsonObject) =>
                    {
                        Debug.Log("connect callback");
                        JsonObject msg = new JsonObject();
                        msg["uid"] = "";
                        try
                        {
                            pclient.request(gateRoute, msg,(message2) =>
                            {
                                Debug.Log("getroute callback");
                                //Debug.Log("2");
                                //Debug.Log("queryEntry" + message2.id);
                                //NetData netData = new NetData(message2);
                                //Debug.Log(message2.code);
                                pclient.Disconnect();
                                if (message2.code == 200)
                                {
                                    //Debug.Log("objjjj");
                                    //Debug.Log(message2.data);
                                    ServerConnector serverConnector = SimpleJson.SimpleJson.DeserializeObject<ServerConnector>(message2.data.ToString());
                                    //Debug.Log("objjjj" + serverList.Count);
                                    callback(serverConnector);
                                }
                                else
                                {
                                    callback(null);
                                }
                                loadingSignal.Dispatch(false);
                                
                            });
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                        }
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public void Connect(ServerConnector ServerConnector, Action callback)
    {
        //Debug.Log(pclient.netWorkState.ToString());

        pclient.InitClient(ServerConnector.host, ServerConnector.port, (message) =>
        {
            loadingSignal.Dispatch(true);
            try
            {
                pclient.connectCallback=callback;
                pclient.connectCallback += () => {
                    loadingSignal.Dispatch(false);
                };
                pclient.connect(null, (jsonObject) =>
                {
                    //Debug.Log("123"+jsonObject.ToString());
                    //Debug.Log("3");
                    //try
                    //{
                    //    JsonObject user = new JsonObject();
                    //    user["uid"] = "";
                    //    pclient.request(connectRoute, user, (msg) =>
                    //    {
                    //        //Debug.Log("4");
                    //        Debug.Log("connect result:" + msg.rawString);
                    //        loadingSignal.Dispatch(false);
                    //    });
                    //}
                    //catch (Exception e)
                    //{
                    //    Debug.Log(e.ToString());
                    //}
                    //Debug.Log("connect callback!");
                    pclient.isConectCallback = true;
                    
                });
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        });
    }


    //public void Login(string account, string pwd, Action<bool> callback)
    //{
    //    //pclient = new PomeloClient(host, port);
    //    JsonObject user = new JsonObject();
    //    user["uid"] = "";
    //    user["account"] = account;
    //    user["pwd"] = pwd;

    //    pclient.request(loginRoute, user, (message) =>
    //    {
    //        Debug.Log(message.rawString);
    //    });
    //}

    //public void ConnectAndLogin(string host_gate, int port_gate, Action<NetData> callback)
    //{
    //    Connect(host_gate, port_gate, (connectData) => 
    //    {
    //        if (connectData.code != 200)
    //        {
    //            Debug.LogError("Request error,all return is:" + connectData.allReturnWhenError.ToString());
    //            callback(null);
    //            return;
    //        }

    //        string host = connectData.data["host"].ToString();
    //        int port = int.Parse(connectData.data["port"].ToString());
    //        Login(host, port, (loginData) =>
    //        {
    //            callback(loginData);
    //        }
    //        );
    //    }
    //    );
    //}

    public void testt()
    {
        //pclient.InitClient(host_gate, port_gate, (message) => {
        //    Debug.Log(message.rawString);
        //    pclient.connect(null, (jsonObject) => {
        //        Debug.Log(jsonObject.ToString());
        //    });

        //});


        //GetServerConnector((connector) => {
        //    Debug.Log(connector.host+connector.port);
        //    Connect(connector, () => {
        //    });
        //});

        pclient.InitClient("192.168.0.100", 3050, (message) => {
            pclient.connect(null, (obj) => { });
        });
        
    }


    public void Request(string route, JsonObject form, Action<Message> callback, bool enableLoading = true)
    {
        if (form == null)
        {
            form = new JsonObject();
        }
        form["uid"] = "";

        if (enableLoading)
            loadingSignal.Dispatch(true);
        pclient.request(route,form, (message) =>
        {

            callback(message);
            if (enableLoading)
                loadingSignal.Dispatch(false);
        }
        );

    }






    //IEnumerator Post(string url, Action<NetData> callback, WWWForm form, bool islogin, bool enableLoading)
    //{
    //    if (enableLoading)
    //        loadingSignal.Dispatch(true);
    //    if (!islogin)
    //        form.AddField("sessionid", sessionid);
    //    WWW www = new WWW(host + url, form);
    //    yield return www;
    //    Debug.LogWarning(www.text);
    //    Debug.LogWarning(serverRoot + url);
    //    NetData netData = null;
    //    try
    //    {
    //        netData = JsonUtility.FromJson<NetData>(www.text);
    //        if (islogin)
    //            sessionid = netData.sessionid;
    //        callback(netData);
    //    }
    //    catch
    //    {
    //        Debug.LogError("json error!\nurl:" + host + url + "\ntext:" + www.text);
    //    }

    //    if (enableLoading)
    //        loadingSignal.Dispatch(false);
    //}

    //IEnumerator Get(string url, Action<NetData> callback, bool enableLoading = true)
    //{
    //    if (enableLoading)
    //        loadingSignal.Dispatch(true);
    //    WWW www = new WWW(serverRoot+url);
    //    yield return www;
    //    NetData netData=null;
    //    //Debug.Log(www.text);
    //    try
    //    {
    //        netData = JsonUtility.FromJson<NetData>(www.text);
    //        callback(netData);
            
    //    }
    //    catch
    //    {
    //        Debug.LogError("json error,url:" + serverRoot + url + "\ntext:" + www.text);
    //    }

    //    if (enableLoading)
    //        loadingSignal.Dispatch(false);
    //}

    //public void Request(string url, Action<NetData> callback, WWWForm form = null, bool islogin=false, bool enableLoading=true)
    //{
    //    if(form!=null)
    //        bootstrapView.StartCoroutine(Post(url,callback,form,islogin,enableLoading));
    //    else
    //        //bootstrapView.StartCoroutine(Get(url, callback));
    //        bootstrapView.StartCoroutine(Post(url, callback, new WWWForm(), islogin, enableLoading));
    //}
}


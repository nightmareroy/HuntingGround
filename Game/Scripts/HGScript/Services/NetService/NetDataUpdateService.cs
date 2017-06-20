using System;
using System.Collections.Generic;
using SimpleJson;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
/// <summary>
/// 从网络更新静态数据
/// </summary>
public class NetDataUpdateService
{
    [Inject]
    public NetService netService{ get; set;}

    [Inject]
    public FileIOService fileIOService{ get; set;}



    public NetDataUpdateService()
    {
        
    }

    public void StartUpdate(Action callback)
    {
        
        GetAllDataUpdateList((netDataUpdateList)=>{
            UpdateAllData(netDataUpdateList,()=>{
                
                callback();
            });
        });
    }

    void GetAllDataUpdateList(Action<List<NetDataUpdateItem>> callback)
    {
        List<NetDataUpdateItem> netDataUpdateList=new List<NetDataUpdateItem>();

        AddDefaultDataUpdateList(netDataUpdateList,()=>{
            callback(netDataUpdateList);
        });
    }

    void UpdateAllData(List<NetDataUpdateItem> netDataUpdateList,Action callback)
    {
//        Debug.Log(netDataUpdateList.Count);

        UpdateItem(netDataUpdateList,callback);

    }

    void UpdateItem(List<NetDataUpdateItem> netDataUpdateList,Action callback)
    {

        if (netDataUpdateList.Count > 0)
        {
            NetDataUpdateItem item = netDataUpdateList[0];
            netDataUpdateList.RemoveAt(0);
            netService.WWWRequest(item.url, (www) =>
                {
                    //fileIOService.WriteAllByte(www.bytes,item.filename,item.path);

                    fileIOService.WriteAllTxt(System.Text.Encoding.UTF8.GetString(www.bytes), item.filename, item.path);

//                    fileIOService.WriteAllTxt(localListJS.ToString(),);
//                    File.WriteAllBytes(Application.persistentDataPath + item.path + item.filename, www.bytes);
                    Debug.Log("Download file from:" + item.url + ",to:" + Application.persistentDataPath + item.path + ",named:" + item.filename);
                    if(item.callback!=null)
                    {
                        item.callback();
                    }
                    UpdateItem(netDataUpdateList, callback);
                });
        }
        else
        {
            callback();
        }
    }




    void AddDefaultDataUpdateList(List<NetDataUpdateItem> netDataUpdateList,Action callback)
    {
        JsonObject remoteListJS;
        JsonObject modifiedListJS;
        JsonObject localListJS;
        netService.WWWRequest(NetService.defaultDataUrl + "/DefaultDataList.txt", (www) =>
            {
                string listStr=www.text;
                remoteListJS = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(listStr);
                if (!fileIOService.FileExistOrCreate("/DefaultDataList.txt","/DefaultData"))
                {
                    fileIOService.WriteAllTxt("{}","/DefaultDataList.txt","/DefaultData");
                    localListJS=new JsonObject();
                    modifiedListJS=remoteListJS;
                }
                else
                {
                    
                    localListJS = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(fileIOService.ReadAllText("/DefaultData/DefaultDataList.txt"));
                    modifiedListJS=new JsonObject();
                    foreach(string key in remoteListJS.Keys)
                    {
                        if(!localListJS.ContainsKey(key))
                        {
                            modifiedListJS.Add(key,remoteListJS[key]);
                        }
                        else if(localListJS[key].ToString()!=remoteListJS[key].ToString())
                        {
                            modifiedListJS[key]=remoteListJS[key];
                        }
                    }
                }

//                {
//                    
//                    fileIOService.WriteAllTxt(listStr,"/DefaultDataList.txt","/DefaultData");
//
//                }

                foreach(string key in modifiedListJS.Keys)
                {
                    string key_local=key;
                    NetDataUpdateItem netDataUpdateItem=new NetDataUpdateItem(NetService.defaultDataUrl+"/defaultdata/"+key_local+".txt","/"+key_local+".txt","/DefaultData/Data",()=>{
                        if(localListJS.ContainsKey(key_local))
                        {
                            localListJS[key_local]=modifiedListJS[key_local];

                        }
                        else
                        {
                            localListJS.Add(key_local,modifiedListJS[key_local]);

                        }
                        fileIOService.WriteAllTxt(localListJS.ToString(),"/DefaultDataList.txt","/DefaultData");
                        Debug.Log("write:"+localListJS.ToString());
                    });
                    netDataUpdateList.Add(netDataUpdateItem);
                }



                callback();


            }
        );
    }

}


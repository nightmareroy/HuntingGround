using System;
using System.Collections.Generic;

public class NetDataUpdateItem
{
//    public enum ItemType
//    {
//        text,
//        texture,
//        assetbundle,
//        vedio,
//        audio
//    }

    public string filename;
//    public ItemType itemType;
    public string path;
    public string url;

    //每更新完一个item需要做一些事情，比如更新本地的资源md5表
    public Action callback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="filename">Filename.需要加"/"</param>
    /// <param name="path">Path.需要加"/"</param>
    public NetDataUpdateItem(string url,string filename,string path="",Action callback=null)
    {
        this.url = url;
//        this.itemType = itemType;
        this.filename = filename;
        this.path = path;
        this.callback = callback;
    }
}

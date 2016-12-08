using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;


/// <summary>
/// 主要用于读写静态数据，因此采用整个文件一起读写的方式
/// </summary>
public class StaticFileIOService {

    /// <summary>
    /// 读取整个文本文件
    /// </summary>
    /// <param name="name">文件名，必须带"/"</param>
    /// <param name="path">文件路径，必须带"/"</param>
    /// <returns>文本内容</returns>
    public string ReadAllText(string name,string path="")
    {
        return File.ReadAllText(Application.persistentDataPath + path + name);
    }

    /// <summary>
    /// 重写整个文本文件
    /// <param name="content">文本内容</param>
    /// <param name="name">文件名，必须带"/"</param>
    /// <param name="path">文件路径，必须带"/"</param>
    /// </summary>
    public void WriteAllTxt(string content, string name, string path="")
    {

        if (!Directory.Exists(Application.persistentDataPath + path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
            //Debug.Log("create directory:" + path);
        }

        //if (!File.Exists(Application.persistentDataPath + path + name))
        //{
        //    File.Create(Application.persistentDataPath + path + name);
        //    Debug.Log("create file:" + path+name);
        //}

        File.WriteAllText(Application.persistentDataPath + path + name, content);
        //Debug.Log("write contents in" + path + name);

    }

    public void CopyFromStreamingToPersistant(string name)
    {
        string content = File.ReadAllText(Application.streamingAssetsPath + name);
        File.WriteAllText(Application.persistentDataPath+name, content);
    }

    //name只是文件名，不包括"/"和".txt"
    public void CopyFromResourceToPersistant(string name)
    {
        string content = (Resources.Load("temp/"+name) as TextAsset).text;
        File.WriteAllText(Application.persistentDataPath + "/"+name+".txt", content);
    }
}

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;


/// <summary>
/// 主要用于读写静态数据，因此采用整个文件一起读写的方式
/// </summary>
public class FileIOService {

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
        }

        File.WriteAllText(Application.persistentDataPath + path + name, content);
    }

    /// <summary>
    /// 重写整个文件
    /// <param name="content">内容</param>
    /// <param name="name">文件名，必须带"/"</param>
    /// <param name="path">文件路径，必须带"/"</param>
    /// </summary>
    public void WriteAllByte(byte[] content, string name, string path="")
    {

        if (!Directory.Exists(Application.persistentDataPath + path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
        }

        File.WriteAllBytes(Application.persistentDataPath + path + name, content);

    }

//    /// <summary>
//    /// 判断文件是否存在，若存在则返回true，若不存在则返回false
//    /// <param name="name">文件名，必须带"/"</param>
//    /// <param name="path">文件路径，必须带"/"</param>
//    /// </summary>
//    public bool FileExist(string name,string path="")
//    {
//        bool created = false;
//        if (!Directory.Exists(Application.persistentDataPath + path))
//        {
//            created = true;
//        }
//        if (!File.Exists(Application.persistentDataPath + path + name))
//        {
//            created = true;
//        }
//        return !created;
//    }


    /// <summary>
    /// 判断文件是否存在，若存在则返回true，若不存在则创建，并返回false
    /// <param name="name">文件名，必须带"/"</param>
    /// <param name="path">文件路径，必须带"/"</param>
    /// </summary>
    public bool FileExistOrCreate(string name,string path="")
    {
        bool created = false;
        if (!Directory.Exists(Application.persistentDataPath + path))
        {
            Debug.LogWarning("create directory");
            Directory.CreateDirectory(Application.persistentDataPath + path);
            created = true;
        }
        if (!File.Exists(Application.persistentDataPath + path + name))
        {
            Debug.LogWarning("create file");
            File.Create(Application.persistentDataPath + path + name).Dispose();
            created = true;
        }
        return !created;
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

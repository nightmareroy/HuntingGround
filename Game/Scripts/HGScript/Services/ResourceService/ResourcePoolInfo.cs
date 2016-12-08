using UnityEngine;
using System.Collections.Generic;

public class ResourcePoolInfo{
    //public enum PoolName
    //{
    //    testrole
    //}


    public ResourcePoolInfo(string path, int max)
    {
        this.path = path;
        this.max = max;
    }


    public List<GameObject> GOList = new List<GameObject>();
    public int max;
    public string path;
    public Object resObj=null;//资源Load得到的未实例化的Object，不是GameObject

    public static Dictionary<string, ResourcePoolInfo> ResourcePool = new Dictionary<string, ResourcePoolInfo>();
    static ResourcePoolInfo()
    {
        //添加resourcePool成员
        ResourcePool.Add("role/0", new ResourcePoolInfo("role/0", 15));
        ResourcePool.Add("role/1", new ResourcePoolInfo("role/1/goku", 15));
        ResourcePool.Add("roleui/roleui",new ResourcePoolInfo("roleui/roleui",15));
        ResourcePool.Add("map/HexNode/HexNode", new ResourcePoolInfo("map/HexNode/HexNode",40));
    }
}

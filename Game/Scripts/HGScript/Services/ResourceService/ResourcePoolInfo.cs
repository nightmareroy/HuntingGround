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

        //role
        ResourcePool.Add("role/0", new ResourcePoolInfo("role/0", 15));
        ResourcePool.Add("role/1", new ResourcePoolInfo("role/1/goku", 15));
        ResourcePool.Add("role/2", new ResourcePoolInfo("role/2/chimpanzee", 20));
        ResourcePool.Add("role/3", new ResourcePoolInfo("role/2/chimpanzee", 20));
        ResourcePool.Add("role/4", new ResourcePoolInfo("role/2/chimpanzee", 20));
        ResourcePool.Add("role/7", new ResourcePoolInfo("role/7/bear", 20));

        //building
        ResourcePool.Add("building/1", new ResourcePoolInfo("building/1/home", 20));
        ResourcePool.Add("building/3", new ResourcePoolInfo("building/3/monkey", 30));
        ResourcePool.Add("building/4", new ResourcePoolInfo("building/4/bear_home", 30));

        //roleui
        ResourcePool.Add("roleui/roleui",new ResourcePoolInfo("roleui/roleui",15));
        ResourcePool.Add("buildingui/buildingui", new ResourcePoolInfo("buildingui/buildingui", 15));

        //map
        ResourcePool.Add("map/HexNode/HexNode", new ResourcePoolInfo("map/HexNode/HexNode",40));

        //flow up tip
        ResourcePool.Add("flowuptip/FlowUpTip", new ResourcePoolInfo("flowuptip/FlowUpTip", 40));

        //msg box
        ResourcePool.Add("msg_box/msg_box", new ResourcePoolInfo("msg_box/msg_box", 3));

        //icon sprites
        ResourcePool.Add("iconsprites/IconSprites", new ResourcePoolInfo("iconsprites/IconSprites", 1));
    }
}

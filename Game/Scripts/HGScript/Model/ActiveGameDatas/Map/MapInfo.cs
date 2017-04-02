using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 初始化之后，各个map的长度应该与width*height相等，这个必须人工检查
/// </summary>
[Serializable]
public class MapInfo
{
    //public enum MapSize
    //{
    //    small,
    //    big
    //}
    //[SerializeField]
    public List<int> landform=new List<int>();

    //[SerializeField]
    public List<int> resource=new List<int>();

    public List<int> meat = new List<int>();

    public List<int> sightzoon=new List<int>();

    

    //[SerializeField]
    //public List<int> detective_map=new List<int>();

    //[SerializeField]
//    public int width=0;
//
//    //[SerializeField]
//    public int height=0;



    ////数据量大，不需要序列化
    //public List<List<RoleInfo>> rolePosMap = new List<List<RoleInfo>>();
    //public List<List<Bui>> buildingPosMap = new List<List<int>>();

//    public MapInfo(int width, int height, List<int> landform_map, List<int> resource_map)
//    {
//        this.width = width;
//        this.height = height;
//        this.landform = landform_map;
//        this.resource = resource_map;
//        //for (int i = 0; i < width * height; i++)
//        //{
//        //    rolePosMap.Add(new List<int>());
//        //    buildingPosMap.Add(new List<int>());
//        //}
//    }
//
//    public MapInfo(int width, int height)
//    {
//        this.width = width;
//        this.height = height;
//
//        for (int i = 0; i < width * height; i++)
//        {
//            landform.Add(0);
//            resource.Add(0);
//        }
//    }
//
//    public MapInfo()
//    {
//        width = 20;
//        height = 40;
//        for (int i = 0; i < width * height; i++)
//        {
//            landform.Add(0);
//            resource.Add(0);
//            //rolePosMap.Add(new List<int>());
//            //buildingPosMap.Add(new List<int>());
//        }
//
//    }

    //public void SetSize(MapSize mapSize)
    //{
    //    switch (mapSize)
    //    {
    //        case MapSize.small:
    //            width = 20;
    //            height = 40;
    //            break;
    //        case MapSize.big:
    //            width = 30;
    //            height = 60;
    //            break;
    //    }
    //    landform_map = new List<int>();
    //    resource_map = new List<int>();
    //    explorestatus_map = new List<int>();
    //    

    //}
}


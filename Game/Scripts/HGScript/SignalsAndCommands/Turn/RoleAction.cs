using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class RoleAction
{

    public JsonArray preActionList;
    public JsonArray afterActionList;
    public List<Dictionary<int, int>> moveList=new List<Dictionary<int,int>>();
    public List<Dictionary<int, int>> landformList = new List<Dictionary<int, int>>();
    public List<Dictionary<int, int>> resourceList = new List<Dictionary<int, int>>();
    public List<List<int>> insightRoleList = new List<List<int>>();

    public void InitFromJson(JsonArray jsar)
    {
        preActionList = null;
        afterActionList = null;
        moveList.Clear();
        landformList.Clear();
        resourceList.Clear();
        insightRoleList.Clear();

        

        for (int step = 0; step < jsar.Count; step++)
        {
            Dictionary<int, int> moveDic = new Dictionary<int, int>();
            Dictionary<int, int> landformDic = new Dictionary<int, int>();
            Dictionary<int, int> resourceDic = new Dictionary<int, int>();
            List<int> subInsightRoleList = new List<int>();

            JsonObject stepJs=jsar[step] as JsonObject;
            switch (step)
            {
                case 0:
                    //在第0step的位置添加一个空的字典，用于占位
                    moveList.Add(new Dictionary<int,int>());

                    //landformList
                    foreach (string key in (stepJs["landform_map"] as JsonObject).Keys)
                    {
                        int posid = int.Parse(key);
                        int mapValue = int.Parse(((stepJs["landform_map"] as JsonObject)[key]).ToString());
                        landformDic.Add(posid, mapValue);
                    }
                    landformList.Add(landformDic);

                    //resourceList
                    foreach (string key in (stepJs["resource_map"] as JsonObject).Keys)
                    {
                        int posid = int.Parse(key);
                        int mapValue = int.Parse(((stepJs["resource_map"] as JsonObject)[key]).ToString());
                        resourceDic.Add(posid, mapValue);
                    }
                    resourceList.Add(resourceDic);

                    //insightRoleList
                    foreach (object value in (stepJs["insight_roles"] as JsonArray))
                    {
                        int roleid = int.Parse(value.ToString());
                        subInsightRoleList.Add(roleid);
                    }
                    insightRoleList.Add(subInsightRoleList);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    //moveList
                    foreach(string key in (stepJs["pos"] as JsonObject).Keys)
                    {
                        int roleid = int.Parse(key);
                        int pos=int.Parse(((stepJs["pos"] as JsonObject)[key]).ToString());
                        moveDic.Add(roleid, pos);
                    }
                    moveList.Add(moveDic);

                    //landformList
                    foreach(string key in (stepJs["landform_map"] as JsonObject).Keys)
                    {
                        int posid = int.Parse(key);
                        int mapValue = int.Parse(((stepJs["landform_map"] as JsonObject)[key]).ToString());
                        landformDic.Add(posid, mapValue);
                    }
                    landformList.Add(landformDic);

                    //resourceList
                    foreach (string key in (stepJs["resource_map"] as JsonObject).Keys)
                    {
                        int posid = int.Parse(key);
                        int mapValue = int.Parse(((stepJs["resource_map"] as JsonObject)[key]).ToString());
                        resourceDic.Add(posid, mapValue);
                    }
                    resourceList.Add(resourceDic);

                    //insightRoleList
                    foreach (object value in (stepJs["insight_roles"] as JsonArray))
                    {
                        int roleid = int.Parse(value.ToString());
                        subInsightRoleList.Add(roleid);
                    }
                    insightRoleList.Add(subInsightRoleList);
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }
        }
    }
}

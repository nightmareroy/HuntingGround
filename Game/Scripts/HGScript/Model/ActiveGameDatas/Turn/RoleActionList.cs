//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SimpleJson;

//public class RoleActionList
//{
//    [Inject]
//    public GameInfo gameInfo{ get; set;}

//    public JsonArray preActionList;
//    public JsonArray afterActionList;
//    public List<Dictionary<string, int>> moveList=new List<Dictionary<string,int>>();
//    public List<Dictionary<string, RoleInfo>> addRolesList=new List<Dictionary<string,RoleInfo>>();
//    public List<List<string>> deleteRolesList=new List<List<string>>();
//    public List<Dictionary<int, int>> landformList = new List<Dictionary<int, int>>();
//    public List<Dictionary<int, int>> resourceList = new List<Dictionary<int, int>>();
////    public List<List<int>> insightRoleList = new List<List<int>>();
//    public Dictionary<string,int> damageDic=new Dictionary<string, int>();
//    public Dictionary<string,int> recoveryDic=new Dictionary<string, int>();
//    public Dictionary<string,TurnAction> turnActionDic = new Dictionary<string, TurnAction>();

//    public class TurnAction
//    {
//        public int direction_did;
//        public object param;
//    }

//    public void InitFromJson(JsonArray jsar)
//    {
//        preActionList = null;
//        afterActionList = null;
//        moveList.Clear();
//        addRolesList.Clear();
//        deleteRolesList.Clear();
//        landformList.Clear();
//        resourceList.Clear();
////        insightRoleList.Clear();
//        damageDic.Clear();
//        recoveryDic.Clear();

        

//        for (int step = 0; step < jsar.Count; step++)
//        {
//            Dictionary<string, int> moveDic = new Dictionary<string, int>();
//            Dictionary<string,RoleInfo> addRolesDic = new Dictionary<string, RoleInfo>();
//            List<string> deleteRolesSubList = new List<string>();
//            Dictionary<int, int> landformDic = new Dictionary<int, int>();
//            Dictionary<int, int> resourceDic = new Dictionary<int, int>();


//            JsonObject stepJs=jsar[step] as JsonObject;

//            switch (step)
//            {
//                case 0:
//                    //在第0step的位置添加一个空的字典，用于占位
////                    moveList.Add(new Dictionary<int,int>());

////                    //landformList
////                    foreach (string key in (stepJs["landform_map"] as JsonObject).Keys)
////                    {
////                        int posid = int.Parse(key);
////                        int mapValue = int.Parse(((stepJs["landform_map"] as JsonObject)[key]).ToString());
////                        landformDic.Add(posid, mapValue);
////                    }
////                    landformList.Add(landformDic);
////
////                    //resourceList
////                    foreach (string key in (stepJs["resource_map"] as JsonObject).Keys)
////                    {
////                        int posid = int.Parse(key);
////                        int mapValue = int.Parse(((stepJs["resource_map"] as JsonObject)[key]).ToString());
////                        resourceDic.Add(posid, mapValue);
////                    }
////                    resourceList.Add(resourceDic);
////
////                    //insightRoleList
////                    foreach (object value in (stepJs["insight_roles"] as JsonArray))
////                    {
////                        int roleid = int.Parse(value.ToString());
////                        subInsightRoleList.Add(roleid);
////                    }
////                    insightRoleList.Add(subInsightRoleList);
////                    break;
//                case 1:
//                case 2:

//                case 4:
//                    //moveList
//                    foreach (string key in (stepJs["pos"] as JsonObject).Keys)
//                    {
//                        string role_id = key;
//                        int pos = int.Parse(((stepJs["pos"] as JsonObject)[key]).ToString());
//                        moveDic.Add(role_id, pos);
//                    }
//                    moveList.Add(moveDic);

//                    //addRoles
//                    foreach (string key in (stepJs["add_roles"] as JsonObject).Keys)
//                    {
//                        string role_id = key;
//                        RoleInfo roleInfo_t = new RoleInfo();
//                        roleInfo_t.InitFromJson((stepJs["add_roles"] as JsonObject)[key] as JsonObject,gameInfo);
//                        addRolesDic.Add(role_id, roleInfo_t);
//                    }
//                    addRolesList.Add(addRolesDic);

//                    //deleteRoles
////                    Debug.Log(stepJs["delete_roles"].ToString());
//                    foreach (object key in (stepJs["delete_roles"] as JsonArray))
//                    {
//                        string role_id = key.ToString();
//                        deleteRolesSubList.Add(role_id);
//                    }
//                    deleteRolesList.Add(deleteRolesSubList);

//                    //landformList
//                    foreach(string key in (stepJs["landform_map"] as JsonObject).Keys)
//                    {
//                        int pos_id = int.Parse(key);
//                        int mapValue = int.Parse(((stepJs["landform_map"] as JsonObject)[key]).ToString());
//                        landformDic.Add(pos_id, mapValue);
//                    }
//                    landformList.Add(landformDic);

//                    //resourceList
//                    foreach (string key in (stepJs["resource_map"] as JsonObject).Keys)
//                    {
//                        int pos_id = int.Parse(key);
//                        int mapValue = int.Parse(((stepJs["resource_map"] as JsonObject)[key]).ToString());
//                        resourceDic.Add(pos_id, mapValue);
//                    }
//                    resourceList.Add(resourceDic);

//                    break;
//                case 3:
//                    foreach (string key in (stepJs["damage"] as JsonObject).Keys)
//                    {
//                        string role_id=key;
//                        int damageValue = int.Parse(((stepJs["damage"] as JsonObject)[key]).ToString());
//                        damageDic.Add(role_id,damageValue);
//                    }
//                    break;
//                case 5:
//                    foreach (string key in (stepJs["recovery"] as JsonObject).Keys)
//                    {
//                        string role_id=key;
//                        int recoveryValue = int.Parse(((stepJs["recovery"] as JsonObject)[key]).ToString());
//                        recoveryDic.Add(role_id,recoveryValue);
//                    }
//                    break;
//                case 6:
//                    foreach (string key in (stepJs["action"] as JsonObject).Keys)
//                    {
//                        string role_id=key;
////                        string temp = ((stepJs["action"] as JsonObject)[key]).ToString();
////                        Debug.Log(temp);
//                        TurnAction turnAction = SimpleJson.SimpleJson.DeserializeObject<TurnAction>(((stepJs["action"] as JsonObject)[key]).ToString());//((stepJs["action"] as JsonObject)[key]) as object;
//                        turnActionDic.Add(role_id,turnAction);
//                    }
//                    break;
//            }
//        }
//    }
//}

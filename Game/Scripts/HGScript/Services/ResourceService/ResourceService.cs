using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class ResourceService{


    

    [Inject]
    public BootstrapView bootstrapView { get; set; }



    public ResourceService()
    {
        
    }

    public Object LoadResource(string path)
    {
        return Resources.Load(path);
    }

    public void AsyncLoadResource(string path, System.Action<Object> callback)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync(path);
        Object resourceobj = resourceRequest.asset;
        bootstrapView.StartCoroutine(loadCoroutine(resourceRequest, callback, resourceobj));
    }

    IEnumerator loadCoroutine(YieldInstruction yieldInstruction, System.Action<Object> callback, Object resourceobj)
    {
        yield return yieldInstruction;
        callback(resourceobj);
    }

    /// <summary>
    /// 从资源池中取出资源并实例化
    /// </summary>
    /// <param name="path">路径由 资源类型/资源静态id 组成，如"role/0"，并与Resource下面的文件组织结构对应</param>
    /// <returns></returns>
    public GameObject Spawn(string path)
    {
        if (ResourcePoolInfo.ResourcePool.ContainsKey(path))
        {
            ResourcePoolInfo resourcePoolInfo = ResourcePoolInfo.ResourcePool[path];
            GameObject go;
            if (resourcePoolInfo.GOList.Count > 0)
            {
                go = resourcePoolInfo.GOList[0];
                resourcePoolInfo.GOList.RemoveAt(0);
            }
            else
            {
                if (resourcePoolInfo.resObj == null)
                    resourcePoolInfo.resObj = Resources.Load(resourcePoolInfo.path);
                go = GameObject.Instantiate(resourcePoolInfo.resObj as GameObject);
            }
            go.SetActive(true);
            return go;
        }
        else
        {
            Debug.LogError("ResourcePool don't have that resource!");
            return null;
        }
        
    }

    /// <summary>
    /// 异步从资源池中取出资源并实例化
    /// </summary>
    public void AsyncSpawn(string path,System.Action<Object> callback)
    {
        if (ResourcePoolInfo.ResourcePool.ContainsKey(path))
        {
            ResourcePoolInfo resourcePoolInfo = ResourcePoolInfo.ResourcePool[path];
            GameObject go;
            if (resourcePoolInfo.GOList.Count > 0)
            {
                go = resourcePoolInfo.GOList[0];
                resourcePoolInfo.GOList.RemoveAt(0);
                go.SetActive(true);
                callback(go);
            }
            else
            {
                if (resourcePoolInfo.resObj == null)
                {
                    AsyncLoadResource(resourcePoolInfo.path, (Object obj) =>
                    {
                        resourcePoolInfo.resObj = obj;
                        go = GameObject.Instantiate(resourcePoolInfo.resObj as GameObject);
                        go.SetActive(true);
                        callback(go);
                    });
                }
                else
                {
                    go = GameObject.Instantiate(resourcePoolInfo.resObj as GameObject);
                    go.SetActive(true);
                    callback(go);
                }
            }
        }
        else
        {
            Debug.LogError("ResourcePool don't have that resource!");
        }
        
        
    }

    /// <summary>
    /// 将资源放回资源池
    /// </summary>
    public void Despawn(string path,GameObject gameObject)
    {
        if (ResourcePoolInfo.ResourcePool.ContainsKey(path))
        {
            ResourcePoolInfo resourcePoolInfo = ResourcePoolInfo.ResourcePool[path];
            if (resourcePoolInfo.GOList.Count < resourcePoolInfo.max)
            {
                resourcePoolInfo.GOList.Add(gameObject);
                gameObject.transform.SetParent(null);
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("ResourcePool don't have that resource!");
        }
    }

    /// <summary>
    /// 清空所有资源池
    /// </summary>
    public void ClearAllPool()
    {
        foreach (string path in ResourcePoolInfo.ResourcePool.Keys)
        {
            ResourcePoolInfo resourcePoolInfo=ResourcePoolInfo.ResourcePool[path];
            resourcePoolInfo.GOList.Clear();
        }
    }
}

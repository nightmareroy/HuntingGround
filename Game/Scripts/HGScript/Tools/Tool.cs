using System;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static void ClearChildren(Transform parent)
    {
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject obj = parent.GetChild(i).gameObject;
            toDestroy.Add(obj);

        }

        foreach(GameObject item in toDestroy)
        {
            item.transform.SetParent(null);
            item.SetActive(false);
            MonoBehaviour.Destroy(item);
        }
    }

    public static void Destroy(Transform t)
    {
        t.gameObject.SetActive(false);
        t.SetParent(null);
        MonoBehaviour.Destroy(t.gameObject);
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static void ClearChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            MonoBehaviour.Destroy(parent.GetChild(i));
        }
    }
}


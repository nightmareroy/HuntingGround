using System;
using System.Collections.Generic;
using UnityEngine;

public class Page
{
    GameObject self = null;
    public Page parent = null;
    public List<Page> childList = new List<Page>();

    public Page(GameObject self)
    {
        this.self = self;
    }
    
    public void SetActive(bool active)
    {
        self.SetActive(active);
    }

    //public void SetParent(Page parent)
    //{
    //    this.parent = parent;
    //    parent.childList.Add(this);
    //}

    public void SetChild(Page child)
    {
        childList.Add(child);
        child.parent = this;
    }
}


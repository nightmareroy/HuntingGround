using System;
using System.Collections.Generic;
using UnityEngine;

public class PageManager
{
    public Page activePage;
    public PageManager(Page activePage)
    {
        this.activePage = activePage;
        activePage.SetActive(true);
    }

    public bool JumpDown(Page childPage)
    {
        if (!activePage.childList.Contains(childPage))
        {
            return false;
        }
        else
        {
            activePage.SetActive(false);
            childPage.SetActive(true);
            activePage = childPage;
            return true;
        }
    }
    public bool JumpUp()
    {
        if (activePage.parent == null)
        {
            return false;
        }
        else
        {
            activePage.SetActive(false);
            activePage.parent.SetActive(true);
            activePage = activePage.parent;
            return true;
        }
    }
}

